using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Dev4Tech
{
    public class MembroEquipe
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public Image FotoPerfil { get; set; }
        public string Cargo { get; set; }
        public int Pontos { get; internal set; }
    }

    public class EquipesRanking : conexao
    {
        private string conexaoString = "server=localhost;database=Dev4Tech;uid=root;pwd=;";
        private string baseFolder = @"C:\xampp\htdocs\dev4tech";

        // Métodos auxiliares para processar fotos (iguais aos da PesquisaEquipes)
        private string TryDecodeUtf8(byte[] bytes)
        {
            try
            {
                string s = Encoding.UTF8.GetString(bytes).Trim('\0').Trim();
                return s;
            }
            catch
            {
                return null;
            }
        }

        private bool LooksLikePath(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.ToLowerInvariant();
            if (s.Contains("img/") || s.Contains("img\\") || s.Contains(".jpg") ||
                s.Contains(".jpeg") || s.Contains(".png") || s.Contains(".bmp"))
                return true;
            return false;
        }

        private string ResolveStoredPathToFullPath(string stored)
        {
            if (string.IsNullOrWhiteSpace(stored)) return null;

            try
            {
                stored = stored.Trim().Trim('"').Trim('\'');
                string normalized = stored.Replace('/', Path.DirectorySeparatorChar)
                                         .Replace('\\', Path.DirectorySeparatorChar);

                if (Path.IsPathRooted(normalized))
                {
                    return normalized;
                }

                string prefix = "img" + Path.DirectorySeparatorChar;
                if (normalized.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    string withoutLeading = normalized.Substring(prefix.Length);
                    return Path.Combine(baseFolder, "img", withoutLeading);
                }

                if (normalized.Equals("img", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Path.Combine(baseFolder, "img");
                }

                if (!normalized.Contains(Path.DirectorySeparatorChar))
                {
                    return Path.Combine(baseFolder, "img", normalized);
                }

                return Path.Combine(baseFolder, normalized.TrimStart(Path.DirectorySeparatorChar));
            }
            catch
            {
                return null;
            }
        }

        public DataTable BuscarEquipesComPontuacao()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
    e.id_equipe,
    e.nome_equipe,
    COALESCE(SUM(
        CASE t.dificuldade
            WHEN 'Fácil' THEN 10
            WHEN 'Média' THEN 20
            WHEN 'Difícil' THEN 30
            ELSE 0
        END
    ), 0) AS pontos
FROM equipes e
LEFT JOIN equipes_membros em ON em.id_equipe = e.id_equipe
LEFT JOIN entregastarefa et ON et.FuncionarioId = em.FuncionarioId AND et.id_equipe = em.id_equipe AND et.entregue = 1
LEFT JOIN tarefas t ON et.id_tarefa = t.id_tarefa
GROUP BY e.id_equipe, e.nome_equipe
ORDER BY pontos DESC;";

            using (var conn = new MySqlConnection(conexaoString))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable BuscarEquipePorId(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT 
            e.id_equipe, 
            e.nome_equipe, 
            c.nome_categoria AS categoria,
            (SELECT COALESCE(SUM(pf.pontos), 0)
                FROM equipes_membros em
                JOIN pontuacaofuncionario pf ON em.FuncionarioId = pf.id_funcionario
                WHERE em.id_equipe = e.id_equipe
                  AND EXISTS (
                      SELECT 1 FROM entregastarefa et
                      WHERE et.FuncionarioId = em.FuncionarioId
                        AND et.id_equipe = em.id_equipe
                        AND et.entregue = 1
                  )
            ) AS pontos
        FROM Equipes e
        LEFT JOIN Categorias c ON e.id_categoria = c.id_categoria
        WHERE e.id_equipe = @idEquipe;";

            using (var conn = new MySqlConnection(conexaoString))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public List<MembroEquipe> BuscarMembrosEquipe(int idEquipe)
        {
            List<MembroEquipe> membros = new List<MembroEquipe>();
            string query = @"
            SELECT f.FuncionarioId, f.Nome, f.Cargo, f.foto_perfil
            FROM Equipes_Membros em
            INNER JOIN Funcionarios f ON f.FuncionarioId = em.FuncionarioId
            WHERE em.id_equipe = @idEquipe
            ORDER BY f.Nome";

            using (var conn = new MySqlConnection(conexaoString))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MembroEquipe membro = new MembroEquipe();
                        membro.FuncionarioId = reader.GetInt32("FuncionarioId");
                        membro.Nome = reader["Nome"].ToString();
                        membro.Cargo = reader["Cargo"] != DBNull.Value ? reader["Cargo"].ToString() : "Desenvolvedor de software";

                        // NOVA LÓGICA PARA CARREGAR FOTOS
                        object fotoData = reader["foto_perfil"];
                        Image fotoMembro = null;

                        if (fotoData != null && fotoData != DBNull.Value)
                        {
                            if (fotoData is byte[] imageData)
                            {
                                // É um blob - tentar carregar como imagem diretamente
                                try
                                {
                                    using (var ms = new MemoryStream(imageData))
                                    {
                                        fotoMembro = Image.FromStream(ms);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erro ao carregar imagem do blob: {ex.Message}");
                                    // Tentar como string se falhar como imagem
                                    try
                                    {
                                        string possivelCaminho = TryDecodeUtf8(imageData);
                                        if (!string.IsNullOrEmpty(possivelCaminho) && LooksLikePath(possivelCaminho))
                                        {
                                            string fullPath = ResolveStoredPathToFullPath(possivelCaminho);
                                            if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                                            {
                                                fotoMembro = Image.FromFile(fullPath);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        // Se tudo falhar, fotoMembro permanece null
                                    }
                                }
                            }
                            else if (fotoData is string caminhoRelativo)
                            {
                                // É um caminho
                                string fullPath = ResolveStoredPathToFullPath(caminhoRelativo);
                                if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                                {
                                    try
                                    {
                                        fotoMembro = Image.FromFile(fullPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                                    }
                                }
                            }
                        }

                        // Se não conseguiu carregar a foto, usa a padrão
                        membro.FotoPerfil = fotoMembro ?? Properties.Resources.icon_perfil;
                        membros.Add(membro);
                    }
                }
            }
            return membros;
        }
    }
}
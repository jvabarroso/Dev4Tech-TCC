using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.IO;

namespace Dev4Tech
{
    public class MembroEquipe
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public Image FotoPerfil { get; set; }
        public string Cargo { get; set; }
    }

    public class EquipesRanking : conexao
    {
        private string conexaoString = "server=localhost;database=Dev4Tech;uid=root;pwd=;";
        private string baseFolder = @"C:\xampp\htdocs\dev4tech\";

        public DataTable BuscarEquipesComPontuacao()
        {
            DataTable dt = new DataTable();
            string query = @"
                    SELECT e.id_equipe, e.nome_equipe, 
                           IFNULL(SUM(pf.pontos),0) AS pontos
                    FROM Equipes e
                    LEFT JOIN Equipes_Membros em ON em.id_equipe = e.id_equipe
                    LEFT JOIN PontuacaoFuncionario pf ON pf.id_funcionario = em.FuncionarioId
                    GROUP BY e.id_equipe, e.nome_equipe
                    ORDER BY pontos DESC, e.data_criacao ASC
                ";
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
        SELECT e.id_equipe, e.nome_equipe, 
               IFNULL(SUM(pf.pontos),0) AS pontos,
               c.nome_categoria  -- ADICIONE ESTA COLUNA
        FROM Equipes e
        LEFT JOIN Equipes_Membros em ON em.id_equipe = e.id_equipe
        LEFT JOIN PontuacaoFuncionario pf ON pf.id_funcionario = em.FuncionarioId
        INNER JOIN Categorias c ON e.id_categoria = c.id_categoria  -- ADICIONE ESTE JOIN
        WHERE e.id_equipe = @idEquipe
        GROUP BY e.id_equipe, e.nome_equipe, c.nome_categoria";

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

                        // VERIFICAR SE É BLOB OU CAMINHO (MESMA LÓGICA DAS OUTRAS TELAS)
                        object fotoData = reader["foto_perfil"];
                        Image fotoMembro = null;

                        if (fotoData != null && fotoData != DBNull.Value)
                        {
                            if (fotoData is byte[] imageData)
                            {
                                // É um blob
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
                                }
                            }
                            else if (fotoData is string caminhoRelativo)
                            {
                                // É um caminho (para compatibilidade com registros antigos)
                                string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));

                                if (File.Exists(caminhoCompleto))
                                {
                                    try
                                    {
                                        fotoMembro = Image.FromFile(caminhoCompleto);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Erro ao carregar imagem: {ex.Message}");
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
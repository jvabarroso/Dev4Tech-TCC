using System;
using System.Data;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class PesquisaIntegrantes : conexao
    {
        private readonly string caminhoBaseImagens = @"C:\xampp\htdocs\dev4tech\";

        public DataTable BuscarEquipesComCategoriaEMembros()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT e.id_equipe, e.nome_equipe, c.nome_categoria
                FROM Equipes e
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                ORDER BY e.nome_equipe";
            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                finally
                {
                    fecharConexao();
                }
            }
            return dt;
        }

        public DataTable BuscarMembrosDaEquipe(int idEquipe, string filtroNome = "")
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT f.FuncionarioId, f.Nome, f.Email, f.Telefone, f.foto_perfil
                FROM Equipes_Membros em
                INNER JOIN Funcionarios f ON em.FuncionarioId = f.FuncionarioId
                WHERE em.id_equipe = @idEquipe";
            if (!string.IsNullOrWhiteSpace(filtroNome))
            {
                query += " AND f.Nome LIKE @filtroNome";
            }
            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    if (!string.IsNullOrWhiteSpace(filtroNome))
                        cmd.Parameters.AddWithValue("@filtroNome", "%" + filtroNome + "%");
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);

                    // Agora para cada linha do DataTable:
                    foreach (DataRow row in dt.Rows)
                    {
                        // Verificar se é um caminho (string) ou blob (byte[])
                        object fotoData = row["foto_perfil"];

                        if (fotoData != null && fotoData != DBNull.Value)
                        {
                            if (fotoData is string caminhoRelativo && !string.IsNullOrEmpty(caminhoRelativo))
                            {
                                // É um caminho de arquivo
                                string caminhoCompleto = Path.Combine(caminhoBaseImagens, caminhoRelativo.Replace("/", @"\"));

                                if (File.Exists(caminhoCompleto))
                                {
                                    // Manter o caminho relativo para compatibilidade com outras partes do código
                                    row["foto_perfil"] = caminhoRelativo;
                                }
                                else
                                {
                                    // arquivo não encontrado, pode manter imagem padrão ou nulo
                                    row["foto_perfil"] = DBNull.Value;
                                }
                            }
                            // Se for byte[] (blob), manter como está para compatibilidade
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return dt;
        }
    }
}
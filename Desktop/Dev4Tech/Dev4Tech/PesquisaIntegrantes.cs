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
                        string caminhoRelativo = row["foto_perfil"] as string;
                        if (!string.IsNullOrEmpty(caminhoRelativo))
                        {
                            string caminhoCompleto = Path.Combine(caminhoBaseImagens, caminhoRelativo.Replace("/", @"\"));
                            if (File.Exists(caminhoCompleto))
                            {
                                // Opcional: carregar imagem para algum controle ou armazenar caminho para exibição
                                 row["foto_perfil"] = caminhoCompleto; // se quiser alterar para caminho completo
                            }
                            else
                            {
                                // arquivo não encontrado, pode manter imagem padrão ou nulo
                                 row["foto_perfil"] = null;
                            }
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

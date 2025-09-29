using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Dev4Tech
{
    class PesquisaIntegrantes : conexao
    {
        private readonly string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";

        public DataTable BuscarEquipesComCategoriaEMembros()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT e.id_equipe, e.nome_equipe, c.nome_categoria, e.foto_equipe
                FROM Equipes e
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                ORDER BY e.nome_equipe";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
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

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    if (!string.IsNullOrWhiteSpace(filtroNome))
                        cmd.Parameters.AddWithValue("@filtroNome", "%" + filtroNome + "%");

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable BuscarEquipePorId(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT e.id_equipe, e.nome_equipe, c.nome_categoria, e.foto_equipe
                FROM Equipes e
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                WHERE e.id_equipe = @idEquipe";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class FiltroEquipes
    {
        private readonly string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";

        // ✅ MÉTODO PRINCIPAL — agora com filtro por nome e categoria
        public DataTable ObterEquipesDoUsuario(string filtroCategoria, string filtroNome)
        {
            DataTable dt = new DataTable();
            int? idUsuario = null;
            bool isAdmin = false;

            if (Sessao.FuncionarioLogado != null)
            {
                idUsuario = Convert.ToInt32(Sessao.FuncionarioLogado.getFuncionarioId());
            }
            else if (Sessao.AdminLogado != null)
            {
                idUsuario = Convert.ToInt32(Sessao.AdminLogado.getAdminId());
                isAdmin = true;
            }

            if (idUsuario == null) return dt;

            string query = @"
                SELECT DISTINCT
                    e.id_equipe,
                    e.nome_equipe,
                    e.foto_equipe,
                    c.nome_categoria,
                    ua.ultima_atividade
                FROM Equipes e
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                LEFT JOIN UltimaAtividadeEquipe ua ON ua.id_equipe = e.id_equipe
                WHERE 
                    (@filtroCategoria IS NULL OR @filtroCategoria = 'Todos' OR c.nome_categoria = @filtroCategoria)
                    AND (@filtroNome IS NULL OR e.nome_equipe LIKE CONCAT('%', @filtroNome, '%'))
                    AND (
                        (@isAdmin = TRUE AND e.AdminId = @idUsuario)
                        OR
                        (@isAdmin = FALSE AND e.id_equipe IN (
                            SELECT em2.id_equipe 
                            FROM Equipes_Membros em2 
                            WHERE em2.FuncionarioId = @idUsuario
                        ))
                    )
                ORDER BY e.nome_equipe;
            ";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Categoria
                    cmd.Parameters.AddWithValue("@filtroCategoria",
                        string.IsNullOrEmpty(filtroCategoria) || filtroCategoria == "Todos" ? null : filtroCategoria);

                    // Nome
                    cmd.Parameters.AddWithValue("@filtroNome",
                        string.IsNullOrEmpty(filtroNome) ? null : filtroNome);

                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@isAdmin", isAdmin);

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // ✅ Novo método — carrega as categorias da tabela Categorias
        public DataTable ObterCategorias()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT nome_categoria FROM Categorias ORDER BY nome_categoria;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        // ✅ Método original mantido
        public DataTable ObterMembrosDaEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();

            string query = @"
                SELECT 
                    f.FuncionarioId,
                    f.Nome AS nome_funcionario, 
                    f.foto_perfil
                FROM Equipes_Membros em
                INNER JOIN Funcionarios f ON f.FuncionarioId = em.FuncionarioId
                WHERE em.id_equipe = @idEquipe
                ORDER BY f.Nome;
            ";

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

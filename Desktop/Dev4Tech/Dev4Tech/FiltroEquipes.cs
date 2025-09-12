using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class FiltroEquipes
    {
        private readonly string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";

        public DataTable ObterEquipesComMembrosComFotos(string filtroCategoria)
        {
            DataTable dt = new DataTable();
            string query = @"
            SELECT 
                e.id_equipe, e.nome_equipe, c.nome_categoria,
                f.FuncionarioId, 
                f.Nome AS nome_funcionario, f.foto_perfil,
                ua.ultima_atividade
            FROM Equipes e
            INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
            INNER JOIN Equipes_Membros em ON em.id_equipe = e.id_equipe
            INNER JOIN Funcionarios f ON f.FuncionarioId = em.FuncionarioId
            LEFT JOIN UltimaAtividadeEquipe ua ON ua.id_equipe = e.id_equipe
            WHERE (@filtroCategoria IS NULL OR @filtroCategoria = 'Todos' OR c.nome_categoria = @filtroCategoria)
            ORDER BY e.nome_equipe, f.Nome;
        ";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@filtroCategoria", filtroCategoria);
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

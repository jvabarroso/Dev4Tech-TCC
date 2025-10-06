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

            // Determina o ID e tipo do usuário logado
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

            // PRIMEIRO: Encontra as equipes do usuário
            string queryEquipesUsuario = @"
        SELECT DISTINCT e.id_equipe
        FROM Equipes e
        INNER JOIN Equipes_Membros em ON e.id_equipe = em.id_equipe
        WHERE 
            (@isAdmin = TRUE AND e.AdminId = @idUsuario)
            OR 
            (@isAdmin = FALSE AND em.FuncionarioId = @idUsuario)
    ";

            // SEGUNDO: Busca todas as informações, incluindo todos os membros das equipes
            string query = $@"
        SELECT 
            e.id_equipe, e.nome_equipe, e.foto_equipe, c.nome_categoria,
            f.FuncionarioId, 
            f.Nome AS nome_funcionario, f.foto_perfil,
            ua.ultima_atividade
        FROM Equipes e
        INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
        INNER JOIN Equipes_Membros em ON em.id_equipe = e.id_equipe
        INNER JOIN Funcionarios f ON f.FuncionarioId = em.FuncionarioId
        LEFT JOIN UltimaAtividadeEquipe ua ON ua.id_equipe = e.id_equipe
        WHERE 
            (@filtroCategoria IS NULL OR @filtroCategoria = 'Todos' OR c.nome_categoria = @filtroCategoria)
            AND e.id_equipe IN ({queryEquipesUsuario})
        ORDER BY e.nome_equipe, f.Nome;
    ";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@filtroCategoria",
                        string.IsNullOrEmpty(filtroCategoria) || filtroCategoria == "Todos" ? null : filtroCategoria);
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
    }
}
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Dev4Tech
{
    class EntregaTarefa : conexao
    {

        

        public DataTable BuscarTarefasAtrasadasPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT t.*, c.nome_categoria, e.nome_equipe, e.foto_equipe
                FROM Tarefas t
                INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                WHERE t.id_equipe = @idEquipe
                AND t.data_entrega < CURDATE()
                AND NOT EXISTS (
                    SELECT 1 FROM EntregasTarefa et WHERE et.id_tarefa = t.id_tarefa AND et.id_equipe = t.id_equipe
                )
                ORDER BY t.data_entrega DESC";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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

        public List<int> ObterEquipesDoFuncionario(int idFuncionario)
        {
            List<int> equipes = new List<int>();
            string query = "SELECT id_equipe FROM Equipes_Membros WHERE FuncionarioId = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipes.Add(reader.GetInt32("id_equipe"));
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return equipes;
        }

        public string BuscarNomeEquipe(int idEquipe)
        {
            string nome = "";
            string query = "SELECT nome_equipe FROM Equipes WHERE id_equipe = @id";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@id", idEquipe);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            nome = result.ToString();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return nome;
        }

        public DataRow BuscarTarefaPorId(int idTarefa)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT t.*, c.nome_categoria, e.nome_equipe
                FROM Tarefas t
                INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                WHERE t.id_tarefa = @idTarefa";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public void RegistrarEntrega(int idTarefa, int idEquipe, int idFuncionario, string descricao, string nomeArquivo, byte[] arquivoBlob)
        {
            string query = @"
                INSERT INTO EntregasTarefa 
                (id_tarefa, id_equipe, FuncionarioId, descricao, nome_arquivo, arquivo_blob, data_entrega) 
                VALUES (@idTarefa, @idEquipe, @idFuncionario, @desc, @nomeArq, @arqBlob, NOW())";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.Parameters.AddWithValue("@desc", descricao);
                        cmd.Parameters.AddWithValue("@nomeArq", nomeArquivo);
                        cmd.Parameters.AddWithValue("@arqBlob", (object)arquivoBlob ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            else
            {
                throw new Exception("Não foi possível abrir a conexão com o banco de dados.");
            }
        }

        public bool FuncionarioEntregou(int idTarefa, int idFuncionario)
        {
            bool entregou = false;
            string query = @"
                SELECT COUNT(*) FROM EntregasTarefa 
                WHERE id_tarefa = @idTarefa AND FuncionarioId = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);

                        var result = cmd.ExecuteScalar();
                        int count = Convert.ToInt32(result);
                        entregou = (count > 0);
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }

            return entregou;
        }

        public bool TodosEntregaram(int idTarefa, int idEquipe)
        {
            bool todosEntregaram = false;
            string query = @"
                SELECT 
                    (SELECT COUNT(*) FROM Equipes_Membros WHERE id_equipe = @idEquipe) AS total_funcionarios,
                    (SELECT COUNT(DISTINCT FuncionarioId) FROM EntregasTarefa WHERE id_tarefa = @idTarefa AND id_equipe = @idEquipe) AS total_entregas";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int totalFuncionarios = 0;
                            int totalEntregas = 0;

                            if (reader.Read())
                            {
                                totalFuncionarios = Convert.ToInt32(reader["total_funcionarios"]);
                                totalEntregas = Convert.ToInt32(reader["total_entregas"]);
                            }

                            todosEntregaram = (totalFuncionarios > 0 && totalFuncionarios == totalEntregas);
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }

            return todosEntregaram;
        }

        public DataTable BuscarTarefasPendentesPorEquipeFuncionario(int idEquipe, int idFuncionario)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT t.*, c.nome_categoria, e.nome_equipe, e.foto_equipe
                FROM Tarefas t
                INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                LEFT JOIN EntregasTarefa et ON t.id_tarefa = et.id_tarefa AND et.FuncionarioId = @idFuncionario
                WHERE t.id_equipe = @idEquipe
                AND et.id_entrega IS NULL
                AND t.data_entrega >= CURDATE()
                ORDER BY t.data_entrega ASC";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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

        public DataTable BuscarTarefasCompletadasPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT DISTINCT t.*, c.nome_categoria, e.nome_equipe, e.foto_equipe
                FROM Tarefas t
                INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
                INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
                INNER JOIN EntregasTarefa et ON t.id_tarefa = et.id_tarefa
                WHERE t.id_equipe = @idEquipe
                ORDER BY t.data_entrega DESC";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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

        public void RemoverEntrega(int idTarefa, int idFuncionario)
        {
            string query = @"
                DELETE FROM EntregasTarefa
                WHERE id_tarefa = @idTarefa AND FuncionarioId = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            else
            {
                throw new Exception("Erro ao conectar ao banco.");
            }
        }

        public DataTable BuscarEntregasPorTarefa(int idTarefa)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT DISTINCT FuncionarioId
                FROM EntregasTarefa
                WHERE id_tarefa = @idTarefa";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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
        // ADICIONAR ESTE MÉTODO À CLASSE ENTREGATAREFA
        public DataTable BuscarTarefasCompletadasComFoto(int idFuncionario, string filtroNome = "")
        {
            DataTable dt = new DataTable();

            // Primeiro obtém as equipes do funcionário
            List<int> equipesFuncionario = ObterEquipesDoFuncionario(idFuncionario);

            if (equipesFuncionario.Count == 0)
                return dt;

            // Constrói a query para múltiplas equipes
            string query = @"
        SELECT DISTINCT t.*, c.nome_categoria, e.nome_equipe, e.foto_equipe
        FROM Tarefas t
        INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
        INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
        INNER JOIN EntregasTarefa et ON t.id_tarefa = et.id_tarefa
        WHERE t.id_equipe IN (" + string.Join(",", equipesFuncionario) + @")";

            if (!string.IsNullOrWhiteSpace(filtroNome))
            {
                query += " AND t.nomeTarefa LIKE @filtroNome";
            }

            query += " ORDER BY t.data_entrega DESC";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        if (!string.IsNullOrWhiteSpace(filtroNome))
                            cmd.Parameters.AddWithValue("@filtroNome", "%" + filtroNome + "%");

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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
        // ADICIONAR ESTE MÉTODO À CLASSE ENTREGATAREFA
        public DataTable BuscarTarefasPendentesComFoto(int idFuncionario, string filtroNome = "")
        {
            DataTable dt = new DataTable();

            // Primeiro obtém as equipes do funcionário
            List<int> equipesFuncionario = ObterEquipesDoFuncionario(idFuncionario);

            if (equipesFuncionario.Count == 0)
                return dt;

            // Constrói a query para múltiplas equipes
            string query = @"
        SELECT t.*, c.nome_categoria, e.nome_equipe, e.foto_equipe
        FROM Tarefas t
        INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
        INNER JOIN Categorias c ON e.id_categoria = c.id_categoria
        LEFT JOIN EntregasTarefa et ON t.id_tarefa = et.id_tarefa AND et.FuncionarioId = @idFuncionario
        WHERE t.id_equipe IN (" + string.Join(",", equipesFuncionario) + @")
        AND et.id_entrega IS NULL
        AND t.data_entrega >= CURDATE()";

            if (!string.IsNullOrWhiteSpace(filtroNome))
            {
                query += " AND t.nomeTarefa LIKE @filtroNome";
            }

            query += " ORDER BY t.data_entrega ASC";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        if (!string.IsNullOrWhiteSpace(filtroNome))
                            cmd.Parameters.AddWithValue("@filtroNome", "%" + filtroNome + "%");

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
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
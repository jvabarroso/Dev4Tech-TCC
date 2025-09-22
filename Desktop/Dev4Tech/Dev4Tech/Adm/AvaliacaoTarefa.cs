using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace Dev4Tech
{
    class AvaliacaoTarefa : conexao
    {
        public void SalvarAvaliacao(int idTarefa, bool aceita, bool? atrasoJustificado)
        {
            string query = @"
        INSERT INTO AvaliacaoTarefa (id_tarefa, aceita, atraso_justificado)
        VALUES (@idTarefa, @aceita, @atrasoJustificado)
        ON DUPLICATE KEY UPDATE aceita = VALUES(aceita), atraso_justificado = VALUES(atraso_justificado)";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                    cmd.Parameters.AddWithValue("@aceita", aceita);
                    if (atrasoJustificado.HasValue)
                        cmd.Parameters.AddWithValue("@atrasoJustificado", atrasoJustificado.Value);
                    else
                        cmd.Parameters.AddWithValue("@atrasoJustificado", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    // Atualizar o status de entrega na tabela EntregasTarefa
                    AtualizarStatusEntrega(idTarefa, aceita);
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        private void AtualizarStatusEntrega(int idTarefa, bool aceita)
        {
            string query = "UPDATE EntregasTarefa SET entregue = @entregue WHERE id_tarefa = @idTarefa";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@entregue", aceita);
                    cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        public void ComputarPontosSeAprovado(int idTarefa)
        {
            AvaliacaoTarefa avaliacao = new AvaliacaoTarefa();
            var avaliacaoInfo = avaliacao.BuscarAvaliacaoPorTarefa(idTarefa);

            if (avaliacaoInfo != null && avaliacaoInfo.Aceita == true)
            {
                bool? atrasoJustificado = avaliacaoInfo.AtrasoJustificado;
                if (atrasoJustificado == false)
                    return; // Não pontua se atraso não justificado

                EntregaTarefa entregaTarefa = new EntregaTarefa();
                var entregas = entregaTarefa.BuscarEntregasPorTarefa(idTarefa);
                if (entregas == null || entregas.Rows.Count == 0) return;
                var tarefa = entregaTarefa.BuscarTarefaPorId(idTarefa);
                if (tarefa == null) return;

                string dificuldade = (tarefa["dificuldade"]?.ToString().ToLower()) ?? "facil";
                int pontos = 5;
                if (dificuldade == "fácil") pontos = 10;
                else if (dificuldade == "média" || dificuldade == "mediana") pontos = 20;
                else if (dificuldade == "difícil") pontos = 30;

                foreach (DataRow row in entregas.Rows)
                {
                    int idFuncionario = Convert.ToInt32(row["FuncionarioId"]);
                    avaliacao.AtualizarPontuacaoFuncionario(idFuncionario, pontos);
                }
            }
        }

        private void PontuarFuncionarios(int idTarefa, bool? atrasoJustificado)
        {
            if (atrasoJustificado == false)
            {
                // atraso não justificado, não computar pontos
                return;
            }

            // Buscar entregas da tarefa
            EntregaTarefa entregaTarefa = new EntregaTarefa();
            var entregas = entregaTarefa.BuscarEntregasPorTarefa(idTarefa);

            if (entregas == null || entregas.Rows.Count == 0) return;

            // Buscar dificuldade da tarefa
            var tarefa = entregaTarefa.BuscarTarefaPorId(idTarefa);
            if (tarefa == null) return;

            string dificuldade = tarefa["dificuldade"]?.ToString().ToLower() ?? "facil";
            int pontos = 5; // padrão

            if (dificuldade == "fácil") pontos = 10;
            else if (dificuldade == "média" || dificuldade == "mediana") pontos = 20;
            else if (dificuldade == "difícil") pontos = 30;

            // Atualiza pontos no banco
            foreach (System.Data.DataRow row in entregas.Rows)
            {
                int idFuncionario = Convert.ToInt32(row["FuncionarioId"]);
                AtualizarPontuacaoFuncionario(idFuncionario, pontos);
            }
        }

        public void AtualizarPontuacaoFuncionario(int idFuncionario, int pontos)
        {
            if (abrirConexao())
            {
                try
                {
                    string queryCheck = "SELECT pontos FROM PontuacaoFuncionario WHERE id_funcionario = @idFuncionario";
                    MySqlCommand cmdCheck = new MySqlCommand(queryCheck, conectar);
                    cmdCheck.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    var result = cmdCheck.ExecuteScalar();

                    if (result == null)
                    {
                        string queryInsert = "INSERT INTO PontuacaoFuncionario (id_funcionario, pontos) VALUES (@idFuncionario, 0)";
                        MySqlCommand cmdInsert = new MySqlCommand(queryInsert, conectar);
                        cmdInsert.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmdInsert.ExecuteNonQuery();
                    }

                    string queryUpdate = "UPDATE PontuacaoFuncionario SET pontos = pontos + @pontos WHERE id_funcionario = @idFuncionario";
                    MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, conectar);
                    cmdUpdate.Parameters.AddWithValue("@pontos", pontos);
                    cmdUpdate.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    cmdUpdate.ExecuteNonQuery();
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        public DataTable BuscarEntregasPorTarefa(int idTarefa)
        {
            DataTable dt = new DataTable();
            string query = "SELECT DISTINCT FuncionarioId FROM EntregasTarefa WHERE id_tarefa = @idTarefa";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
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

        public DataRow BuscarTarefaPorId(int idTarefa)
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Tarefas WHERE id_tarefa = @idTarefa";

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

            if (dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

        public DataTable BuscarTarefasNaoAvaliadasPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = @"
            SELECT DISTINCT t.id_tarefa, t.nomeTarefa, t.dificuldade, e.nome_equipe, t.data_entrega
            FROM Tarefas t
            INNER JOIN Equipes e ON t.id_equipe = e.id_equipe
            INNER JOIN EntregasTarefa et ON et.id_tarefa = t.id_tarefa
            WHERE t.id_equipe = @idEquipe 
            AND (et.entregue IS NULL OR et.entregue = FALSE)
            AND NOT EXISTS (
                SELECT 1 FROM AvaliacaoTarefa av 
                WHERE av.id_tarefa = t.id_tarefa
            )";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
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

        public class AvaliacaoInfo
        {
            public bool? Aceita { get; set; }
            public bool? AtrasoJustificado { get; set; }
        }

        public AvaliacaoInfo BuscarAvaliacaoPorTarefa(int idTarefa)
        {
            AvaliacaoInfo avaliacao = null;
            string query = "SELECT aceita, atraso_justificado FROM AvaliacaoTarefa WHERE id_tarefa = @idTarefa";
            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                avaliacao = new AvaliacaoInfo();
                                if (!reader.IsDBNull(0))
                                    avaliacao.Aceita = reader.GetBoolean(0);
                                else
                                    avaliacao.Aceita = null;
                                if (!reader.IsDBNull(1))
                                    avaliacao.AtrasoJustificado = reader.GetBoolean(1);
                                else
                                    avaliacao.AtrasoJustificado = null;
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return avaliacao;
        }
    }
}
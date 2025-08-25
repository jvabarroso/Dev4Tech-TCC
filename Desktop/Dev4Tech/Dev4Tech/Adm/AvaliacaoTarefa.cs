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
                VALUES (@idTarefa, @aceita, @atraso)
                ON DUPLICATE KEY UPDATE aceita = @aceita, atraso_justificado = @atraso";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                    cmd.Parameters.AddWithValue("@aceita", aceita);
                    if (atrasoJustificado.HasValue)
                        cmd.Parameters.AddWithValue("@atraso", atrasoJustificado.Value);
                    else
                        cmd.Parameters.AddWithValue("@atraso", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    // Se a tarefa for aceita (e atraso justificado ou dentro do prazo), computa pontos do funcionário
                    if (aceita)
                    {
                        PontuarFuncionarios(idTarefa, atrasoJustificado);
                    }
                }
                finally
                {
                    fecharConexao();
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

    }
}

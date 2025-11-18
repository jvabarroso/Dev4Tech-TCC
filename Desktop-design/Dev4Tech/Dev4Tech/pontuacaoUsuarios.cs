using System;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class pontuacaoUsuarios : conexao
    {
        public void AdicionarPontos(int idFuncionario, int pontos)
        {
            string querySelect = "SELECT pontos FROM PontuacaoFuncionario WHERE id_funcionario = @idFuncionario";
            string queryInsert = "INSERT INTO PontuacaoFuncionario (id_funcionario, pontos) VALUES (@idFuncionario, @pontos)";
            string queryUpdate = "UPDATE PontuacaoFuncionario SET pontos = pontos + @pontos WHERE id_funcionario = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmdSelect = new MySqlCommand(querySelect, conectar);
                    cmdSelect.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    var result = cmdSelect.ExecuteScalar();

                    if (result == null) // não existe registro, insere
                    {
                        MySqlCommand cmdInsert = new MySqlCommand(queryInsert, conectar);
                        cmdInsert.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmdInsert.Parameters.AddWithValue("@pontos", pontos);
                        cmdInsert.ExecuteNonQuery();
                    }
                    else // já existe, atualiza
                    {
                        MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, conectar);
                        cmdUpdate.Parameters.AddWithValue("@pontos", pontos);
                        cmdUpdate.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        public int ObterPontos(int idFuncionario)
        {
            int pontos = 0;
            string query = @"
        SELECT 
            COALESCE(SUM(
                CASE t.dificuldade
                    WHEN 'Fácil' THEN 10
                    WHEN 'Média' THEN 20
                    WHEN 'Difícil' THEN 30
                    ELSE 0
                END
            ), 0) AS pontos
        FROM EntregasTarefa et
        JOIN Tarefas t ON et.id_tarefa = t.id_tarefa
        WHERE et.FuncionarioId = @idFuncionario AND et.entregue = 1;
    ";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        pontos = Convert.ToInt32(result);
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }

            return pontos;
        }
    }
}

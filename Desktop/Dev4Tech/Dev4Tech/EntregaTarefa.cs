using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Dev4Tech
{
    class EntregaTarefa : conexao
    {
        // Busca a tarefa da equipe selecionada
        public DataRow BuscarTarefaPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Tarefas WHERE id_equipe = @idEquipe ORDER BY id_tarefa DESC LIMIT 1";
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
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        // Registra a entrega da tarefa
        public void RegistrarEntrega(int idTarefa, int idEquipe, string descricao, string nomeArquivo, byte[] arquivoBlob)
        {
            string query = "INSERT INTO EntregasTarefa (id_tarefa, id_equipe, descricao, nome_arquivo, arquivo_blob) " +
                           "VALUES (@idTarefa, @idEquipe, @desc, @nomeArq, @arqBlob)";
            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    cmd.Parameters.AddWithValue("@desc", descricao);
                    cmd.Parameters.AddWithValue("@nomeArq", nomeArquivo);
                    cmd.Parameters.AddWithValue("@arqBlob", (object)arquivoBlob ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    fecharConexao();
                }
            }
        }
        // NOVO MÉTODO: Busca todas as tarefas para uma equipe (para preencher o ComboBox)
        public DataTable BuscarTodasTarefasPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = "SELECT id_tarefa, instrucoes FROM Tarefas WHERE id_equipe = @idEquipe ORDER BY data_entrega DESC";

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


        // NOVO MÉTODO: Busca uma tarefa específica por ID (para exibir detalhes)
        public DataRow BuscarTarefaPorId(int idTarefa)
        {
            DataTable dt = new DataTable();
            // Seleciona todos os campos da tarefa para exibir os detalhes
            string query = "SELECT * FROM Tarefas WHERE id_tarefa = @idTarefa";
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
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
    }

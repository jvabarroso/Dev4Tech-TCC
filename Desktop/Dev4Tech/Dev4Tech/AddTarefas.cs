using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class AddTarefas : conexao
    {
        public string Instrucoes { get; set; }
        public int IdEquipe { get; set; }
        public DateTime DataEntrega { get; set; }
        public string NomeArquivo { get; set; }
        public byte[] ArquivoBlob { get; set; }

        // Método para inserir tarefa no banco
        public void Inserir()
        {
            string query = "INSERT INTO Tarefas (instrucoes, id_equipe, data_entrega, nome_arquivo, arquivo_blob) " +
                           "VALUES (@instr, @idEq, @data, @nomeArq, @arqBlob)";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@instr", Instrucoes);
                    cmd.Parameters.AddWithValue("@idEq", IdEquipe);
                    cmd.Parameters.AddWithValue("@data", DataEntrega);
                    cmd.Parameters.AddWithValue("@nomeArq", NomeArquivo);
                    cmd.Parameters.AddWithValue("@arqBlob", (object)ArquivoBlob ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        // Novo método para buscar equipes no banco
        public DataTable BuscarEquipes()
        {
            DataTable dt = new DataTable();

            if (abrirConexao())
            {
                try
                {
                    string query = "SELECT id_equipe, nome_equipe FROM Equipes ORDER BY nome_equipe";
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
    }
}

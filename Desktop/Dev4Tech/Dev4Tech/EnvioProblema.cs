using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class EnvioProblema : conexao
    {
        public void InserirRelato(int idTarefa, int idEquipe, int idEmpresa, string descricao)
        {
            string query = "INSERT INTO RelatoProblema (id_tarefa, id_equipe, id_empresa, descricao) VALUES (@idTarefa, @idEquipe, @idEmpresa, @descricao)";
            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

    }
}
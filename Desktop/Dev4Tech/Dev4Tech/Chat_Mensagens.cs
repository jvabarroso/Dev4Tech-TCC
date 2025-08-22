// Chat_Mensagens.cs
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class Chat_Mensagens : conexao
    {
        private string idMensagem;
        private string texto;
        private DateTime dataEnvio;
        private int idEquipe;
        private int? idFuncionario; // Nullable pois pode ser null
        private int? idAdmin;       // Nullable pois pode ser null

        public void setIdMensagem(string idMensagem) { this.idMensagem = idMensagem; }
        public void setTexto(string texto) { this.texto = texto; }
        public void setDataEnvio(DateTime dataEnvio) { this.dataEnvio = dataEnvio; }
        public void setIdEquipe(int idEquipe) { this.idEquipe = idEquipe; }
        public void setIdFuncionario(int? idFuncionario) { this.idFuncionario = idFuncionario; }
        public void setIdAdmin(int? idAdmin) { this.idAdmin = idAdmin; }

        public string getIdMensagem() { return this.idMensagem; }
        public string getTexto() { return this.texto; }
        public DateTime getDataEnvio() { return this.dataEnvio; }
        public int getIdEquipe() { return this.idEquipe; }
        public int? getIdFuncionario() { return this.idFuncionario; }
        public int? getIdAdmin() { return this.idAdmin; }

        public void inserir()
        {
            string query = "INSERT INTO MensagensChat (texto, data_envio, id_equipe, FuncionarioId, AdminId) " +
                           "VALUES (@texto, @data_envio, @id_equipe, @funcionarioId, @adminId)";
            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.Parameters.AddWithValue("@texto", getTexto());
                cmd.Parameters.AddWithValue("@data_envio", getDataEnvio().ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id_equipe", getIdEquipe());
                if (getIdFuncionario().HasValue)
                    cmd.Parameters.AddWithValue("@funcionarioId", getIdFuncionario());
                else
                    cmd.Parameters.AddWithValue("@funcionarioId", DBNull.Value);
                if (getIdAdmin().HasValue)
                    cmd.Parameters.AddWithValue("@adminId", getIdAdmin());
                else
                    cmd.Parameters.AddWithValue("@adminId", DBNull.Value);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        // Ajuste na consulta para buscar também AdminId
        public DataTable ConsultarPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            if (this.abrirConexao())
            {
                string mSQL = "SELECT * FROM MensagensChat WHERE id_equipe = @id_equipe ORDER BY data_envio";
                MySqlCommand cmd = new MySqlCommand(mSQL, conectar);
                cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                this.fecharConexao();
            }
            return dt;
        }

        // Atualizar última atividade da equipe
        public void AtualizarUltimaAtividade(int idEquipe)
        {
            string query = @"
                INSERT INTO UltimaAtividadeEquipe (id_equipe, ultima_atividade)
                VALUES (@id_equipe, NOW())
                ON DUPLICATE KEY UPDATE ultima_atividade = NOW()
            ";
            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }
    }
}

using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class Chat_Mensagens : conexao
    {
        private string idMensagem;
        //private string usuario;
        private string texto;
        private DateTime dataEnvio;

        // Setters
        public void setIdMensagem(string idMensagem)
        {
            this.idMensagem = idMensagem;
        }

        //public void setUsuario(string usuario)
        //{
           // this.usuario = usuario;
       // }

        public void setTexto(string texto)
        {
            this.texto = texto;
        }

        public void setDataEnvio(DateTime dataEnvio)
        {
            this.dataEnvio = dataEnvio;
        }

        // Getters
        public string getIdMensagem()
        {
            return this.idMensagem;
        }

        //public string getUsuario()
        //{
          //  return this.usuario;
        //}

        public string getTexto()
        {
            return this.texto;
        }

        public DateTime getDataEnvio()
        {
            return this.dataEnvio;
        }

        // Inserir mensagem no banco
        public void inserir()
        {
            string query = "INSERT INTO MensagensChat (id_mensagem, texto, data_envio) " +
                           "VALUES ('" + getIdMensagem() + "', '"  + getTexto() + "', '" + getDataEnvio().ToString("yyyy-MM-dd HH:mm:ss") + "')";

            if (this.abrirConexao() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        // Excluir mensagem por id
        public void excluir()
        {
            string query = "DELETE FROM MensagensChat WHERE id_mensagem = '" + getIdMensagem() + "'";
            if (this.abrirConexao() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        // Consultar todas as mensagens
        public DataTable Consultar()
        {
            DataTable dt = new DataTable();
            if (this.abrirConexao() == true)
            {
                string mSQL = "SELECT * FROM MensagensChat ORDER BY data_envio";
                MySqlCommand cmd = new MySqlCommand(mSQL, conectar);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                this.fecharConexao();
            }
            return dt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class ConfiguraçõesBD : conexao
    {
        private string id_config, id_usuario, tema, notificacoes, idioma;

        public void setIdConfig(string id_config)
        {
            this.id_config = id_config;
        }
        public void setIdUsuario(string id_usuario)
        {
            this.id_usuario = id_usuario;
        }
        public void setTema(string tema)
        {
            this.tema = tema;
        }
        public void setNotificacoes(string notificacoes)
        {
            this.notificacoes = notificacoes;
        }
        public void setIdioma(string idioma)
        {
            this.idioma = idioma;
        }

        public string getIdConfig()
        {
            return this.id_config;
        }
        public string getIdUsuario()
        {
            return this.id_usuario;
        }
        public string getTema()
        {
            return this.tema;
        }
        public string getNotificacoes()
        {
            return this.notificacoes;
        }

        private void txtDataNasc_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {

        }

        private void btnRanking_Click(object sender, EventArgs e)
        {

        }

        private void btnConfigurações_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void picPerfilMembro_Click(object sender, EventArgs e)
        {

        }

        public string getIdioma()
        {
            return this.idioma;
        }

        public void inserir()
        {
            string query = "INSERT INTO Configuracoes(id_usuario, tema, notificacoes, idioma) " +
                           "VALUES('" + getIdUsuario() + "','" + getTema() + "','" + getNotificacoes() + "','" + getIdioma() + "')";

            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        public void atualizar()
        {
            string query = "UPDATE Configuracoes SET tema = '" + getTema() + "', notificacoes = '" + getNotificacoes() + "', idioma = '" + getIdioma() + "' WHERE id_usuario = '" + getIdUsuario() + "'";
            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        public DataTable ConsultarConfiguracoes()
        {
            DataTable dt = new DataTable();
            string mSQL = "SELECT * FROM Configuracoes WHERE id_usuario = '" + getIdUsuario() + "'";

            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(mSQL, conectar);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                this.fecharConexao();
            }
            return dt;
        }
    }
}

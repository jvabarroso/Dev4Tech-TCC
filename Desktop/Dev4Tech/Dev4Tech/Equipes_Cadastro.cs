using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Dev4Tech
{
    class Equipes_Cadastro : conexao
    {
        private string equipeId, nome, email, categoria, img;

        public void setEquipeId(string equipeId)
        {
            this.equipeId = equipeId;
        }
        public void setNome (string nome) {
            this.nome = nome;
        }
        public void setEmail (string email)
        {
            this.email = email;
        }

        public void setCategoria(string categoria)
        {
            this.categoria = categoria;
        }
        public void setImg(string img)
        {
            this.img = img;
        }

        public string getEquipeId()
        {
            return this.equipeId;
        }
        public string getNome()
        {
            return this.nome;
        }
        public string getEmail()
        {
            return this.email;
        }
        public string getCategoria()
        {
            return this.categoria;
        }
        public string getImg()
        {
            return this.img;
        }

        //Método inserir, para mandar os dados no banco de dados
        public void inserir()
        {
            string query = "INSERT INTO equipes(equipeId, Nome, categoria, img) " +
                           "VALUES('" + getEquipeId() + "','" + getNome() + "','" + getCategoria() + "','" + getImg() + "')";

            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        //Excluir informações do banco de dados por meio da chave primária
        public void excluir()
        {
            string query = "DELETE FROM equipes WHERE equipeID = '" + getEquipeId() + "'";
            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        //Método consultar mostra todos os dados existentes na tabela
        public DataTable Consultar()
        {
            DataTable dt = new DataTable();
            string mSQL = "SELECT * FROM equipes";

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

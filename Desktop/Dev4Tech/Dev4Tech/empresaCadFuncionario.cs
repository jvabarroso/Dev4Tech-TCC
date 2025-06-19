using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Biblioteca de conexão do SQL
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Dev4Tech
{
    class empresaCadFuncionario : conexao
    {
        private string FuncionarioId, Email, Senha, Telefone, DataNascimento, CPF, Cargo, Nome;
        DateTime data_cadFunc;

        public void setData_cadFunc(DateTime data_cadFunc)
        {
            this.data_cadFunc = data_cadFunc;
        }

        public void setFuncionarioId(string FuncionarioId)
        {
            this.FuncionarioId = FuncionarioId;
        }
        public void setNome(string nome)
        {
            this.Nome = nome;
        }
        public void setCargo(string cargo)
        {
            this.Cargo = cargo;
        }
        public void setCPF(string cpf)
        {
            this.CPF = cpf;
        }
        public void setDataNascimento(string dataNascimento)
        {
            this.DataNascimento = dataNascimento;
        }
        public void setTelefone(string telefone)
        {
            this.Telefone = telefone;
        }
        public void setEmail(string email)
        {
            this.Email = email;
        }
        public void setSenha(string senha)
        {
            this.Senha = senha;
        }

        public DateTime getData_cadFunc()
        {
            return this.data_cadFunc;
        } 

        public string getFuncionarioId()
        {
            return this.FuncionarioId;
        }
        public string getNome()
        {
            return this.Nome;
        }
        public string getCargo()
        {
            return this.Cargo;
        }
        public string getCPF()
        {
            return this.CPF;
        }
        public string getDataNascimento()
        {
            return this.DataNascimento;
        }
        public string getTelefone()
        {
            return this.Telefone;
        }
        public string getEmail()
        {
            return this.Email;
        }
        public string getSenha()
        {
            return this.Senha;
        }

        //Método inserir, para mandar os dados no banco de dados
        public void inserir()
        {
            string query = "INSERT INTO Funcionarios(FuncionarioId, Nome, Cargo, CPF, Telefone, Email, Senha, data_cadFunc) " +
                           "VALUES('" + getFuncionarioId() + "','" + getNome() + "','" + getCargo() + "','" + getCPF() + "','" + getTelefone() + "','" + getEmail() + "','" + getSenha() + "','" + getData_cadFunc() + "')";

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
            string query = "DELETE FROM funcionarios WHERE FuncioanrioId = '" + getFuncionarioId() + "'";
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
            string mSQL = "SELECT * FROM funcionarios";

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Biblioteca de conexão do SQL
using MySql.Data.MySqlClient;
using System.Data;

namespace Dev4Tech
{
    class empresaCadFuncionario : conexao
    {
        private string FuncionarioId, Email, Senha, Telefone, CPF, Cargo, Nome, endereço, numero;
        DateTime data_cadFunc, DataNascimento;

        public void setData_cadFunc(DateTime data_cadFunc)
        {
            this.data_cadFunc = data_cadFunc;
        }

        public void setNumero(string numero)
        {
            this.numero = numero;
        }

        public void setEndereço (string endereço)
        {
            this.endereço = endereço;
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
        public void setDataNascimento(DateTime dataNascimento)
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

        public string getEndereço()
        {
            return this.endereço;
        }
        public string getNumero()
        {
            return this.numero;
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
        public DateTime getDataNascimento()
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

        public DateTime getData_cadFunc()
        {
            return this.data_cadFunc;
        }

        public empresaCadFuncionario ObterFuncionarioPorEmailSenha(string email, string senha)
        {
            empresaCadFuncionario func = null;

            // Monta a query concatenando diretamente os valores
            string query = "SELECT FuncionarioId, Nome, Cargo, CPF, DataNascimento, Telefone, Email, endereço, numero " +
                           "FROM Funcionarios WHERE Email = '" + getEmail() + "' AND Senha = '" + getSenha() + "' LIMIT 1";

            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            func = new empresaCadFuncionario();

                            func.setFuncionarioId(reader["FuncionarioId"].ToString());
                            func.setNome(reader["Nome"].ToString());
                            func.setCargo(reader["Cargo"].ToString());
                            func.setCPF(reader["CPF"].ToString());
                            func.setDataNascimento(Convert.ToDateTime(reader["DataNascimento"]));
                            func.setTelefone(reader["Telefone"].ToString());
                            func.setEmail(reader["Email"].ToString());
                            func.setEndereço(reader["endereço"].ToString());
                            func.setNumero(reader["numero"].ToString());
                        }
                    }
                }
                finally
                {
                    this.fecharConexao();
                }
            }

            return func;
        }



        //Método inserir, para mandar os dados no banco de dados
        public void inserir()
        {
            string query = "INSERT INTO Funcionarios(FuncionarioId, Nome, Cargo, CPF, DataNascimento, Telefone, Email, Senha, data_cadFunc, endereço, numero) " +
                           "VALUES('" + getFuncionarioId() + "','" + getNome() + "','" + getCargo() + "','" + getCPF() + "','" + getDataNascimento().ToString("yyyy-MM-dd HH:mm:ss") + "','" + getTelefone() + "','" + getEmail() + "','" + getSenha() + "','" + getData_cadFunc().ToString("yyyy-MM-dd HH:mm:ss") + "','" + getEndereço() + "','" + getNumero() + "')";

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

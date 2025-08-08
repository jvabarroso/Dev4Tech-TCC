// Classe empresaCadFuncionario.cs
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class empresaCadFuncionario : conexao
    {
        private string FuncionarioId, Email, Senha, Telefone, CPF, Cargo, Nome, endereco, numero;
        private DateTime data_cadFunc, DataNascimento;
        private int id_empresa; // novo campo
        private int AdminId;    // novo campo

        public void setData_cadFunc(DateTime data_cadFunc) { this.data_cadFunc = data_cadFunc; }
        public void setNumero(string numero) { this.numero = numero; }
        public void setEndereco(string endereco) { this.endereco = endereco; }
        public void setFuncionarioId(string FuncionarioId) { this.FuncionarioId = FuncionarioId; }
        public void setNome(string nome) { this.Nome = nome; }
        public void setCargo(string cargo) { this.Cargo = cargo; }
        public void setCPF(string cpf) { this.CPF = cpf; }
        public void setDataNascimento(DateTime dataNascimento) { this.DataNascimento = dataNascimento; }
        public void setTelefone(string telefone) { this.Telefone = telefone; }
        public void setEmail(string email) { this.Email = email; }
        public void setSenha(string senha) { this.Senha = senha; }
        public void setIdEmpresa(int idEmpresa) { this.id_empresa = idEmpresa; }
        public void setAdminId(int adminId) { this.AdminId = adminId; }

        public string getEndereco() { return this.endereco; }
        public string getNumero() { return this.numero; }
        public string getFuncionarioId() { return this.FuncionarioId; }
        public string getNome() { return this.Nome; }
        public string getCargo() { return this.Cargo; }
        public string getCPF() { return this.CPF; }
        public DateTime getDataNascimento() { return this.DataNascimento; }
        public string getTelefone() { return this.Telefone; }
        public string getEmail() { return this.Email; }
        public string getSenha() { return this.Senha; }
        public DateTime getData_cadFunc() { return this.data_cadFunc; }
        public int getIdEmpresa() { return this.id_empresa; }
        public int getAdminId() { return this.AdminId; }

        // Obter funcionário por email e senha, agora com id_empresa e AdminId
        public empresaCadFuncionario ObterFuncionarioPorEmailSenha(string email, string senha)
        {
            empresaCadFuncionario func = null;

            string query = @"SELECT FuncionarioId, Nome, Cargo, CPF, DataNascimento, Telefone, Email, endereco, numero, data_cadFunc, Senha, id_empresa, AdminId 
                             FROM Funcionarios WHERE Email = @Email AND Senha = @Senha LIMIT 1";

            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Senha", senha);

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
                            func.setEndereco(reader["endereco"].ToString());
                            func.setNumero(reader["numero"].ToString());
                            func.setData_cadFunc(Convert.ToDateTime(reader["data_cadFunc"]));
                            func.setSenha(reader["Senha"].ToString());
                            func.setIdEmpresa(Convert.ToInt32(reader["id_empresa"]));
                            func.setAdminId(Convert.ToInt32(reader["AdminId"]));
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

        // Inserção com campos novos id_empresa e AdminId
        public void inserir()
        {
            string query = @"INSERT INTO Funcionarios(FuncionarioId, Nome, Cargo, CPF, DataNascimento, Telefone, Email, Senha, data_cadFunc, endereco, numero, id_empresa, AdminId) 
                             VALUES('" + getFuncionarioId() + "','" + getNome() + "','" + getCargo() + "','" + getCPF() + "','" + getDataNascimento().ToString("yyyy-MM-dd HH:mm:ss") + "','" + getTelefone() + "','" + getEmail() + "','" + getSenha() + "','" + getData_cadFunc().ToString("yyyy-MM-dd HH:mm:ss") + "','" + getEndereco() + "','" + getNumero() + "'," + getIdEmpresa() + "," + getAdminId() + ")";

            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        // Exclusão de funcionário (mantida igual)
        public void excluir()
        {
            string query = "DELETE FROM Funcionarios WHERE FuncionarioId = @id";
            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@id", getFuncionarioId());
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    this.fecharConexao();
                }
            }
        }

        // Consulta geral (mantida igual)
        public DataTable Consultar()
        {
            DataTable dt = new DataTable();
            string mSQL = "SELECT * FROM Funcionarios";

            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(mSQL, conectar);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                finally
                {
                    this.fecharConexao();
                }
            }
            return dt;
        }
    }
}

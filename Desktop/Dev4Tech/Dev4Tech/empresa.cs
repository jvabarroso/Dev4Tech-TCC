// Classe empresa.cs
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class empresa : conexao
    {
        private string nomeEmpresa, setorEmpresarial, logradouro, bairro, complemento, CNPJ, numResidencia, email, telefone;
        private DateTime data_cadEm;

        public void setData_cadEm(DateTime data_cadEm) { this.data_cadEm = data_cadEm; }
        public void setNomeEmpresa(string nomeEmpresa) { this.nomeEmpresa = nomeEmpresa; }
        public void setSetorEmpresarial(string setorEmpresarial) { this.setorEmpresarial = setorEmpresarial; }
        public void setLogradouro(string logradouro) { this.logradouro = logradouro; }
        public void setNumResidencia(string numResidencia) { this.numResidencia = numResidencia; }
        public void setBairro(string bairro) { this.bairro = bairro; }
        public void setComplemento(string complemento) { this.complemento = complemento; }
        public void setCNPJ(string CNPJ) { this.CNPJ = CNPJ; }
        public void setEmail(string email) { this.email = email; }
        public void setTelefone(string telefone) { this.telefone = telefone; }

        public DateTime getData_cadEm() { return this.data_cadEm; }
        public string getNomeEmpresa() { return this.nomeEmpresa; }
        public string getSetorEmpresarial() { return this.setorEmpresarial; }
        public string getLogradouro() { return this.logradouro; }
        public string getNumResidencia() { return this.numResidencia; }
        public string getBairro() { return this.bairro; }
        public string getComplemento() { return this.complemento; }
        public string getCNPJ() { return this.CNPJ; }
        public string getEmail() { return this.email; }
        public string getTelefone() { return this.telefone; }

        // Método inserir para a tabela Empresas (id_empresa é auto_increment, não inserido manualmente)
        public void inserir()
        {
            string query = @"
                INSERT INTO Empresas (nome_empresa, cnpj, logradouro, numResidencia, bairro, complemento, data_cadEm, email, telefone, setorEmpresarial) 
                VALUES (@nome, @cnpj, @logradouro, @numResidencia, @bairro, @complemento, @dataCadastro, @email, @telefone, @setorEmpresarial)";

            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@nome", getNomeEmpresa());
                    cmd.Parameters.AddWithValue("@cnpj", getCNPJ());
                    cmd.Parameters.AddWithValue("@logradouro", getLogradouro());
                    cmd.Parameters.AddWithValue("@numResidencia", getNumResidencia());
                    cmd.Parameters.AddWithValue("@bairro", getBairro());
                    cmd.Parameters.AddWithValue("@complemento", getComplemento());
                    cmd.Parameters.AddWithValue("@dataCadastro", getData_cadEm().ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@email", getEmail());
                    cmd.Parameters.AddWithValue("@telefone", getTelefone());
                    cmd.Parameters.AddWithValue("@setorEmpresarial", getSetorEmpresarial());

                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    this.fecharConexao();
                }
            }
        }

        // Excluir empresa pelo nome (exemplo - adaptar chave primaria se precisar)
        public void excluir()
        {
            string query = "DELETE FROM Empresas WHERE nome_empresa = @nome";
            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@nome", getNomeEmpresa());
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    this.fecharConexao();
                }
            }
        }

        // Consultar todas as empresas
        public DataTable Consultar()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Empresas";

            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
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

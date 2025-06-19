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
    class empresa : conexao
    {
        private string nomeEmpresa, codigoId, setorEmpresarial, logradouro, bairro, complemento, CNPJ, numResidencia, email, telefone;
        DateTime data_cadEm;

        public void setData_cadEm(DateTime data_cadEm)
        {
            this.data_cadEm = data_cadEm;
        }
        public void setNomeEmpresa(string nomeEmpresa)
        {
            this.nomeEmpresa = nomeEmpresa;
        }
        public void setCodigoId(string codigoId)
        {
            this.codigoId = codigoId;
        }
        public void setCNPJ(string CNPJ)
        {
            this.CNPJ = CNPJ;
        }
        public void setSetorEmpresarial(string setorEmpresarial)
        {
            this.setorEmpresarial = setorEmpresarial;
        }
        public void setLogradouro(string logradouro)
        {
            this.logradouro = logradouro;
        }
        public void setNumResidencia(string numResidencia)
        {
            this.numResidencia = numResidencia;
        }
        public void setBairro(string bairro)
        {
            this.bairro = bairro;
        }
        public void setComplemento(string complemento)
        {
            this.complemento = complemento;
        }
        public void setEmail(string email)
        {
            this.email = email;
        }
        public void setTelefone(string telefone)
        {
            this.telefone = telefone;
        }

        public DateTime getData_cadEm()
        {
            return this.data_cadEm;
        }
        public string getNomeEmpresa()
        {
            return this.nomeEmpresa;
        }
        public string getCodigoId()
        {
            return this.codigoId;
        }
        public string getCNPJ()
        {
            return this.CNPJ;
        }
        public string getSetorEmpresarial()
        {
            return this.setorEmpresarial;
        }
        public string getLogradouro()
        {
            return this.logradouro;
        }
        public string getNumResidencia()
        {
            return this.numResidencia;
        }
        public string getBairro()
        {
            return this.bairro;
        }
        public string getComplemento()
        {
            return this.complemento;
        }
        public string getEmail()
        {
            return this.email;
        }
        public string getTelefone()
        {
            return this.telefone;
        }

        //Método inserir, para mandar os dados no banco de dados
        public void inserir()
        {
            string query = "Insert into Empresas(id_empresa, nome_empresa, cnpj, logradouro, numResidencia, bairro, complemento, data_cadEm) values ('" + getCodigoId() + "', '" + getNomeEmpresa() + "', '" + getCNPJ() + "', '" + getLogradouro() + "', '" +getNumResidencia() + "', '" + getBairro() + "', '"+ getComplemento() + "','" + getData_cadEm().ToString("yyyy-MM-dd HH:mm:ss") + "')";

            if (this.abrirConexao() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        //Excluir informações do banco de dados por meio da chave primária
        public void excluir()
        {
            string query = "delete from dadosEmpresa where CodigoId = '" + getCodigoId() + "'";
            MySqlCommand cmd = new MySqlCommand(query, conectar);
            cmd.ExecuteNonQuery();
            this.fecharConexao();
        }

        //Método cosultar mostra todos os dados existentes na tabela
        public DataTable Consultar()
        {
            this.abrirConexao();
            string mSQL = "select * from dadosEmpresa";
            MySqlCommand cmd = new MySqlCommand(mSQL, conectar);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            this.fecharConexao();
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
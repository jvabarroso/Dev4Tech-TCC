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
<<<<<<< HEAD
        DateTime data_cadEm;
=======
        private DateTime data_cad;

        public void setData_cad (DateTime data_cad)
        {
            this.data_cad = data_cad;
        }
>>>>>>> c1e5d468858d85b13d37cd5c5733fe2d1fcfd1ef

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

<<<<<<< HEAD
        public DateTime getData_cadEm()
        {
            return this.data_cadEm;
        }
=======
        public DateTime getData_cad()
        {
            return this.data_cad;
        }

>>>>>>> c1e5d468858d85b13d37cd5c5733fe2d1fcfd1ef
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
<<<<<<< HEAD
            string query = "Insert into Empresas(id_empresa, nome_empresa, cnpj, logradouro, numResidencia, bairro, complemento, data_cadEm) values ('" + getCodigoId() + "', '" + getNomeEmpresa() + "', '" + getCNPJ() + "', '" + getLogradouro() + "', '" +getNumResidencia() + "', '" + getBairro() + "', '"+ getComplemento() + "','" + getData_cadEm().ToString("yyyy-MM-dd HH:mm:ss") + "')";
=======
            string query = "Insert into Empresas(id_empresa, nome_empresa, cnpj, logradouro, numResidencia, bairro, complemento, data_cad) values ('" + getCodigoId() + "', '" + getNomeEmpresa() + "', '" + getCNPJ() + "', '" + getLogradouro() + "', '" +getNumResidencia() + "', '" + getBairro() + "', '" + getComplemento() + "', '" + getData_cad().ToString("yyyy-MM-dd HH:mm:ss") + "')";
>>>>>>> c1e5d468858d85b13d37cd5c5733fe2d1fcfd1ef

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
// AddEquipes.cs
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class AddEquipes : conexao
    {
        private string nomeEquipe;
        private string categoria;
        private string emailFuncionario;
        private string adminId; // Campo para armazenar AdminId como string
        private string fotoEquipe; // Adicionando o campo fotoEquipe
        private string idEmpresa;

        // Setters
        public void setNomeEquipe(string nomeEquipe)
        {
            this.nomeEquipe = nomeEquipe;
        }
        public void setCategoria(string categoria)
        {
            this.categoria = categoria;
        }
        public void setEmailFuncionario(string emailFuncionario)
        {
            this.emailFuncionario = emailFuncionario;
        }
        public void setAdminId(string adminId)
        {
            this.adminId = adminId;
        }
        public void setFotoEquipe(string foto)
        {
            this.fotoEquipe = foto;
        }
        public void setIdEmpresa(string idEmpresa)
        {
            this.idEmpresa = idEmpresa;
        }

        // Getters
        public string getNomeEquipe()
        {
            return this.nomeEquipe;
        }
        public string getCategoria()
        {
            return this.categoria;
        }
        public string getEmailFuncionario()
        {
            return this.emailFuncionario;
        }
        public string getAdminId()
        {
            return this.adminId;
        }
        public string getFotoEquipe()
        {
            return this.fotoEquipe;
        }
        public string getIdEmpresa()
        {
            return this.idEmpresa;
        }

        private int ObterOuInserirCategoria(string nomeCategoria)
        {
            int idCategoria = 0;
            string selectQuery = "SELECT id_categoria FROM Categorias WHERE nome_categoria = @nome AND id_empresa = @idEmpresa";
            string insertQuery = "INSERT INTO Categorias (nome_categoria, id_empresa) VALUES (@nome, @idEmpresa)";
            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmdSelect = new MySqlCommand(selectQuery, conectar);
                    cmdSelect.Parameters.AddWithValue("@nome", nomeCategoria);
                    cmdSelect.Parameters.AddWithValue("@idEmpresa", getIdEmpresa());
                    object result = cmdSelect.ExecuteScalar();
                    if (result != null)
                    {
                        idCategoria = Convert.ToInt32(result);
                    }
                    else
                    {
                        MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conectar);
                        cmdInsert.Parameters.AddWithValue("@nome", nomeCategoria);
                        cmdInsert.Parameters.AddWithValue("@idEmpresa", getIdEmpresa());
                        cmdInsert.ExecuteNonQuery();
                        idCategoria = (int)cmdInsert.LastInsertedId;
                    }
                }
                finally
                {
                    this.fecharConexao();
                }
            }
            return idCategoria;
        }

        private int ObterIdFuncionarioPorEmail(string email)
        {
            int idFuncionario = 0;
            string query = "SELECT FuncionarioId FROM Funcionarios WHERE email = @Email";
            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@Email", email);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        idFuncionario = Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Funcionário não encontrado para o email informado.");
                    }
                }
                finally
                {
                    this.fecharConexao();
                }
            }
            return idFuncionario;
        }

        public DataTable ConsultarCategorias()
        {
            DataTable dt = new DataTable();
            string query = "SELECT nome_categoria FROM Categorias ORDER BY nome_categoria";
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

        public DataTable ConsultarEmailsFuncionarios()
        {
            DataTable dt = new DataTable();
            string query = "SELECT email FROM Funcionarios ORDER BY email";
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

        public int InserirEquipeRetornandoId()
        {
            if (string.IsNullOrEmpty(getNomeEquipe()) || string.IsNullOrEmpty(getCategoria()))
                throw new Exception("Preencha todos os campos antes de salvar.");

            int idCategoria = ObterOuInserirCategoria(getCategoria());
            int idEquipe = 0;

            string query = "INSERT INTO Equipes (nome_equipe, id_categoria, AdminId, foto_equipe, id_empresa) " +
                           "VALUES (@nomeEquipe, @idCategoria, @adminId, @fotoEquipe, @idEmpresa); SELECT LAST_INSERT_ID();";

            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@nomeEquipe", getNomeEquipe());
                    cmd.Parameters.AddWithValue("@idCategoria", idCategoria);
                    cmd.Parameters.AddWithValue("@adminId", getAdminId());
                    cmd.Parameters.AddWithValue("@idEmpresa", getIdEmpresa());

                    // CORREÇÃO: Converter string para bytes antes de salvar no LONGBLOB
                    if (!string.IsNullOrEmpty(getFotoEquipe()))
                    {
                        byte[] fotoBytes = System.Text.Encoding.UTF8.GetBytes(getFotoEquipe());
                        cmd.Parameters.AddWithValue("@fotoEquipe", fotoBytes);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@fotoEquipe", DBNull.Value);
                    }

                    idEquipe = Convert.ToInt32(cmd.ExecuteScalar());
                }
                finally
                {
                    this.fecharConexao();
                }
            }
            return idEquipe;
        }

        public void InserirMembroEquipe(int idEquipe, string emailFuncionario)
        {
            int idFuncionario = ObterIdFuncionarioPorEmail(emailFuncionario);
            string query = "INSERT INTO Equipes_Membros (id_equipe, FuncionarioId) VALUES (@idEquipe, @idFuncionario)";
            if (this.abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    this.fecharConexao();
                }
            }
        }
    }
}

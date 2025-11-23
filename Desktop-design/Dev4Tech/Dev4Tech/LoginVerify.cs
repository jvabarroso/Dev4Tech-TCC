using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class LoginVerify : conexao
    {
        public bool ValidarLoginFuncionario(string email, string senha)
        {
            bool valido = false;
            string query = "SELECT Senha FROM Funcionarios WHERE email = @Email LIMIT 1";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string senhaHashArmazenada = reader["Senha"].ToString();
                                valido = SenhasHash.VerificarSenha(senha, senhaHashArmazenada);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao validar login de funcionário: {ex.Message}");
                }
                finally
                {
                    fecharConexao();
                }
            }
            return valido;
        }

        public bool ValidarLoginAdministrador(string email, string senha)
        {
            bool valido = false;
            string query = "SELECT Senha FROM Administradores WHERE email = @Email LIMIT 1";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string senhaHashArmazenada = reader["Senha"].ToString();
                                valido = SenhasHash.VerificarSenha(senha, senhaHashArmazenada);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao validar login de administrador: {ex.Message}");
                }
                finally
                {
                    fecharConexao();
                }
            }
            return valido;
        }

        public bool ValidarLoginEmpresa(string email, string senha)
        {
            bool valido = false;
            string query = "SELECT senha FROM Empresas WHERE email = @Email LIMIT 1";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string senhaHashArmazenada = reader["senha"].ToString();
                                valido = SenhasHash.VerificarSenha(senha, senhaHashArmazenada);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao validar login de empresa: {ex.Message}");
                }
                finally
                {
                    fecharConexao();
                }
            }
            return valido;
        }

        public bool EmailExiste(string email, string tipoUsuario)
        {
            bool existe = false;
            string tabela = "";

            switch (tipoUsuario.ToLower())
            {
                case "funcionario":
                    tabela = "Funcionarios";
                    break;
                case "administrador":
                    tabela = "Administradores";
                    break;
                case "empresa":
                    tabela = "Empresas";
                    break;
                default:
                    throw new ArgumentException("Tipo de usuário inválido");
            }

            string query = $"SELECT COUNT(*) FROM {tabela} WHERE email = @Email";

            if (abrirConexao())
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        long count = (long)cmd.ExecuteScalar();
                        existe = count > 0;
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return existe;
        }
    }
}
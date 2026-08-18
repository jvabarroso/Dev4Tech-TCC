using System;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class UsuarioFoto : conexao
    {
        private readonly string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";
        private string baseFolder = @"C:\xampp\htdocs\dev4tech\";

        public Image ObterFotoUsuario()
        {
            Image fotoUsuario = null;
            string idUsuario = null;
            bool ehAdmin = false;

            // Determinar qual usuário está logado
            if (Sessao.FuncionarioLogado != null)
            {
                idUsuario = Sessao.FuncionarioLogado.getFuncionarioId();
                ehAdmin = false;
            }
            else if (Sessao.AdminLogado != null)
            {
                idUsuario = Sessao.AdminLogado.getAdminId();
                ehAdmin = true;
            }
            else
            {
                return null; // Nenhum usuário logado
            }

            string query = ehAdmin
                ? "SELECT foto_perfil FROM Administradores WHERE AdminId = @idUsuario LIMIT 1"
                : "SELECT foto_perfil FROM Funcionarios WHERE FuncionarioId = @idUsuario LIMIT 1";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    var resultado = cmd.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        if (resultado is byte[] imageData && imageData.Length > 0)
                        {
                            // É um blob - tentar carregar como imagem
                            try
                            {
                                using (var ms = new MemoryStream(imageData))
                                using (var imagemTemporaria = Image.FromStream(ms))
                                {
                                    fotoUsuario = new Bitmap(imagemTemporaria);
                                }
                            }
                            catch
                            {
                                // Se falhar, tentar como caminho
                                try
                                {
                                    string caminhoRelativo = System.Text.Encoding.UTF8.GetString(imageData);
                                    string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));
                                    if (File.Exists(caminhoCompleto))
                                    {
                                        fotoUsuario = Image.FromFile(caminhoCompleto);
                                    }
                                }
                                catch
                                {
                                    // Se tudo falhar, retorna null
                                }
                            }
                        }
                        else if (resultado is string caminhoRelativo)
                        {
                            // É um caminho
                            try
                            {
                                string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));
                                if (File.Exists(caminhoCompleto))
                                {
                                    fotoUsuario = Image.FromFile(caminhoCompleto);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                            }
                        }
                    }
                }
            }
            return fotoUsuario;
        }
    }
}
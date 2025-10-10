using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace Dev4Tech
{
    class Chat_Mensagens : conexao
    {
        private string idMensagem;
        private string texto;
        private DateTime dataEnvio;
        private int idEquipe;
        private int? idFuncionario; // Nullable pois pode ser null
        private int? idAdmin;       // Nullable pois pode ser null
        private int idEmpresa;

        public void setIdMensagem(string idMensagem) { this.idMensagem = idMensagem; }
        public void setTexto(string texto) { this.texto = texto; }
        public void setDataEnvio(DateTime dataEnvio) { this.dataEnvio = dataEnvio; }
        public void setIdEquipe(int idEquipe) { this.idEquipe = idEquipe; }
        public void setIdFuncionario(int? idFuncionario) { this.idFuncionario = idFuncionario; }
        public void setIdAdmin(int? idAdmin) { this.idAdmin = idAdmin; }
        public void setIdEmpresa(int idEmpresa) { this.idEmpresa = idEmpresa; }
        public string getIdMensagem() { return this.idMensagem; }
        public string getTexto() { return this.texto; }
        public DateTime getDataEnvio() { return this.dataEnvio; }
        public int getIdEquipe() { return this.idEquipe; }
        public int? getIdFuncionario() { return this.idFuncionario; }
        public int? getIdAdmin() { return this.idAdmin; }
        public int getIdEmpresa() { return this.idEmpresa; }

        public void inserir()
        {
            this.idEmpresa = BuscarIdEmpresaPorEquipe(getIdEquipe());
            string query = "INSERT INTO MensagensChat (texto, data_envio, id_equipe, FuncionarioId, AdminId, id_empresa) " +
                           "VALUES (@texto, @data_envio, @id_equipe, @funcionarioId, @adminId, @id_empresa)";
            if (this.abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.Parameters.AddWithValue("@texto", getTexto());
                cmd.Parameters.AddWithValue("@data_envio", getDataEnvio().ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id_equipe", getIdEquipe());
                if (getIdFuncionario().HasValue)
                    cmd.Parameters.AddWithValue("@funcionarioId", getIdFuncionario());
                else
                    cmd.Parameters.AddWithValue("@funcionarioId", DBNull.Value);
                if (getIdAdmin().HasValue)
                    cmd.Parameters.AddWithValue("@adminId", getIdAdmin());
                else
                    cmd.Parameters.AddWithValue("@adminId", DBNull.Value);
                if (getIdEmpresa() > 0)
                    cmd.Parameters.AddWithValue("@id_empresa", getIdEmpresa());
                else
                    throw new Exception("id_empresa não pode ser nulo ou zero ao inserir mensagem!");
                cmd.ExecuteNonQuery();
                this.fecharConexao();
            }
        }

        public DataTable ConsultarPorEquipe(int idEquipe)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT m.*, 
                       f.nome AS nome_funcionario, f.foto_perfil AS foto_funcionario,
                       a.nome AS nome_admin, a.foto_perfil AS foto_admin
                FROM MensagensChat m
                LEFT JOIN Funcionarios f ON m.FuncionarioId = f.FuncionarioId
                LEFT JOIN Administradores a ON m.AdminId = a.AdminId
                WHERE m.id_equipe = @idEquipe
                ORDER BY m.data_envio ASC";
            if (abrirConexao())
            {
                try
                {
                    var cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    var da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                finally
                {
                    fecharConexao();
                }
            }
            return dt;
        }

        public void AtualizarUltimaAtividade(int idEquipe)
        {
            string query = @"
                INSERT INTO UltimaAtividadeEquipe (id_equipe, ultima_atividade)
                VALUES (@id_equipe, NOW())
                ON DUPLICATE KEY UPDATE ultima_atividade = NOW()
            ";
            if (abrirConexao())
            {
                MySqlCommand cmd = new MySqlCommand(query, conectar);
                cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                cmd.ExecuteNonQuery();
                fecharConexao();
            }
        }

        private int BuscarIdEmpresaPorEquipe(int idEquipe)
        {
            int idEmpresa = 0;
            string query = @"
                SELECT a.id_empresa
                FROM Equipes e
                INNER JOIN Administradores a ON e.AdminId = a.AdminId
                WHERE e.id_equipe = @id_equipe
                LIMIT 1";
            if (abrirConexao())
            {
                try
                {
                    var cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        idEmpresa = Convert.ToInt32(result);
                }
                finally
                {
                    fecharConexao();
                }
            }
            return idEmpresa;
        }

        public void MarcarMensagemVisualizada(int idMensagem, int idUsuario, string tipoUsuario, int idEquipe)
        {
            if (!VisualizacaoExistente(idMensagem, idUsuario, tipoUsuario))
            {
                InserirVisualizacao(idMensagem, idUsuario, tipoUsuario);
            }
            int totalUsuarios = BuscarTotalUsuariosEquipe(idEquipe);
            int visualizacoes = ContarVisualizacoes(idMensagem);
            string novoStatus = "enviada";
            if (visualizacoes > 0 && visualizacoes < totalUsuarios)
                novoStatus = "entregue";
            else if (visualizacoes >= totalUsuarios)
                novoStatus = "lida";
            AtualizarStatusMensagem(idMensagem, novoStatus);
        }

        private bool VisualizacaoExistente(int idMensagem, int idUsuario, string tipoUsuario)
        {
            bool exists = false;
            string query = "SELECT 1 FROM MensagensChat_Visualizacao WHERE id_mensagem = @idMensagem AND id_usuario = @idUsuario AND tipo_usuario = @tipoUsuario LIMIT 1";
            if (abrirConexao())
            {
                try
                {
                    var cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idMensagem", idMensagem);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);
                    var result = cmd.ExecuteScalar();
                    exists = result != null;
                }
                finally { fecharConexao(); }
            }
            return exists;
        }
        private void InserirVisualizacao(int idMensagem, int idUsuario, string tipoUsuario)
        {
            string query = @"INSERT INTO MensagensChat_Visualizacao (id_mensagem, id_usuario, tipo_usuario, data_visualizacao) 
                     VALUES (@idMensagem, @idUsuario, @tipoUsuario, NOW())";
            if (abrirConexao())
            {
                try
                {
                    var cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idMensagem", idMensagem);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@tipoUsuario", tipoUsuario); // 'funcionario' ou 'admin'
                    cmd.ExecuteNonQuery();
                }
                finally { fecharConexao(); }
            }
        }

        private int BuscarTotalUsuariosEquipe(int idEquipe)
        {
            int total = 0;
            string query = @"
        SELECT COUNT(*) FROM (
            SELECT FuncionarioId AS id FROM Equipes_Membros WHERE id_equipe = @idEquipe
            UNION
            SELECT AdminId AS id FROM Equipes WHERE id_equipe = @idEquipe AND AdminId IS NOT NULL
        ) AS totalUsuarios";
            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            total = Convert.ToInt32(result);
                    }
                }
                finally { fecharConexao(); }
            }
            return total;
        }




        private int ContarVisualizacoes(int idMensagem)
        {
            int total = 0;
            string query = "SELECT COUNT(*) FROM MensagensChat_Visualizacao WHERE id_mensagem = @idMensagem";
            if (abrirConexao())
            {
                try
                {
                    var cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idMensagem", idMensagem);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        total = Convert.ToInt32(result);
                }
                finally { fecharConexao(); }
            }
            return total;
        }
        private void AtualizarStatusMensagem(int idMensagem, string status)
        {
            string query = "UPDATE MensagensChat SET status = @status WHERE id_mensagem = @idMensagem";
            if (abrirConexao())
            {
                try
                {
                    var cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@idMensagem", idMensagem);
                    cmd.ExecuteNonQuery();
                }
                finally { fecharConexao(); }
            }
        }
        public Image ObterFotoEquipe(int idEquipe)
        {
            Image fotoEquipe = null;
            string query = "SELECT foto_equipe FROM Equipes WHERE id_equipe = @idEquipe LIMIT 1";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    var resultado = cmd.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        // Se for byte[] (LONGBLOB), tentar carregar como imagem
                        if (resultado is byte[] imageData)
                        {
                            try
                            {
                                using (var ms = new MemoryStream(imageData))
                                {
                                    fotoEquipe = Image.FromStream(ms);
                                }
                            }
                            catch
                            {
                                // Se falhar como imagem, tentar como string/nome de arquivo
                                try
                                {
                                    string nomeArquivo = System.Text.Encoding.UTF8.GetString(imageData);
                                    string caminhoImagemEquipe = Path.Combine(@"C:\xampp\htdocs\dev4tech\img", nomeArquivo);
                                    if (File.Exists(caminhoImagemEquipe))
                                    {
                                        fotoEquipe = Image.FromFile(caminhoImagemEquipe);
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
                                string caminhoCompleto = Path.Combine(@"C:\xampp\htdocs\dev4tech\", caminhoRelativo.Replace("/", @"\"));
                                if (File.Exists(caminhoCompleto))
                                {
                                    fotoEquipe = Image.FromFile(caminhoCompleto);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return fotoEquipe;
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Configuracoes : Form
    {
        private empresaCadFuncionario funcionario;
        private empresaCadAdmin admin;
        private string baseRoot = @"C:\xampp\htdocs\dev4tech"; // raiz do seu projeto (sem \img)
        private string baseImgFolder = @"C:\xampp\htdocs\dev4tech\img"; // pasta onde as imagens ficam

        // Construtor para funcionário
        public Configuracoes(empresaCadFuncionario func)
        {
            InitializeComponent();
            funcionario = func;
            admin = null;
            PreencherCamposFuncionario();
            this.Load += Configuracoes_Load;
            CarregarFotoUsuario();
        }

        // Construtor para administrador
        public Configuracoes(empresaCadAdmin adm)
        {
            InitializeComponent();
            admin = adm;
            funcionario = null;
            PreencherCamposAdmin();
            this.Load += Configuracoes_Load;
            CarregarFotoUsuario();
        }

        // Helper: tenta decodificar bytes para string UTF8 (trim)
        private string TryDecodeUtf8(byte[] bytes)
        {
            try
            {
                string s = Encoding.UTF8.GetString(bytes).Trim('\0').Trim();
                return s;
            }
            catch
            {
                return null;
            }
        }

        // Helper: detecta se uma string parece ser um caminho de imagem (contém img/, .jpg, .png, barras etc)
        private bool LooksLikePath(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.ToLowerInvariant();
            if (s.Contains("img/") || s.Contains("img\\") || s.Contains(".jpg") || s.Contains(".jpeg") || s.Contains(".png") || s.Contains(".bmp"))
                return true;
            return false;
        }

        // Helper: resolve o caminho completo no disco a partir do que está armazenado no banco
        private string ResolveStoredPathToFullPath(string stored)
        {
            if (string.IsNullOrWhiteSpace(stored)) return null;

            // Se for caminho absoluto já retorna normalizado
            try
            {
                // Remove quotes e espaços estranhos
                stored = stored.Trim().Trim('"').Trim('\'');

                // Normaliza barras
                string normalized = stored.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                // Se já é um caminho absoluto, retorna
                if (Path.IsPathRooted(normalized))
                {
                    return normalized;
                }

                // Se começa com "img\" -> combine com root (dev4tech)
                string prefix = "img" + Path.DirectorySeparatorChar;
                if (normalized.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    string withoutLeading = normalized.Substring(prefix.Length);
                    return Path.Combine(baseRoot, "img", withoutLeading);
                }

                // Se começa com "img" mas sem barra (ex: "imgnome.jpg") — tratamos normalmente
                if (normalized.Equals("img", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Path.Combine(baseRoot, "img");
                }

                // Se for só um nome de arquivo (ex: "nome.jpg"), combine com a pasta img
                if (!normalized.Contains(Path.DirectorySeparatorChar))
                {
                    return Path.Combine(baseImgFolder, normalized);
                }

                // Caso genérico: combine com root
                return Path.Combine(baseRoot, normalized.TrimStart(Path.DirectorySeparatorChar));
            }
            catch
            {
                return null;
            }
        }

        private string ObterFotoEquipeNomeArquivo(int idEquipe)
        {
            string nomeArquivo = null;
            string query = "SELECT foto_equipe FROM Equipes WHERE id_equipe = @idEquipe LIMIT 1";
            string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    var resultado = cmd.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        // Se veio como byte[] (provavelmente LONGBLOB)
                        if (resultado is byte[] bytes)
                        {
                            // Tenta decodificar para string
                            string possivel = TryDecodeUtf8(bytes);
                            if (LooksLikePath(possivel))
                            {
                                // devolve a string do caminho armazenado
                                nomeArquivo = possivel;
                            }
                            else
                            {
                                // Se não for caminho, talvez seja blob real de imagem -> não temos nome de arquivo, retorne null
                                nomeArquivo = null;
                            }
                        }
                        else
                        {
                            nomeArquivo = resultado.ToString();
                        }
                    }
                }
            }
            return nomeArquivo;
        }

        private void CarregarEquipesDoUsuario(int idUsuario, bool isFuncionario)
        {
            var equipes = ObterEquipesComUltimaAtividade(idUsuario, isFuncionario);
            flowLayoutPanelEquipes.Controls.Clear();
            ToolTip tt = new ToolTip();

            foreach (var equipe in equipes)
            {
                Panel pnlEquipe = new Panel
                {
                    Width = 300,
                    Height = 120,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = Color.White
                };

                PictureBox picEquipe = new PictureBox
                {
                    Width = 50,
                    Height = 50,
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                // Resolve o caminho da imagem da equipe (pode ser nome de arquivo, caminho relativo "img/..." ou null)
                if (!string.IsNullOrEmpty(equipe.NomeArquivoFoto))
                {
                    string fullPath = ResolveStoredPathToFullPath(equipe.NomeArquivoFoto);
                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        try
                        {
                            using (var ms = new MemoryStream(File.ReadAllBytes(fullPath)))
                            {
                                picEquipe.Image = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            picEquipe.Image = Properties.Resources.icon_equip;
                        }
                    }
                    else
                    {
                        picEquipe.Image = Properties.Resources.icon_equip;
                    }
                }
                else
                {
                    picEquipe.Image = Properties.Resources.icon_equip;
                }

                pnlEquipe.Controls.Add(picEquipe);

                Label lblNomeEquipe = new Label
                {
                    Text = equipe.NomeEquipe,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Location = new Point(70, 10),
                    AutoSize = true
                };
                pnlEquipe.Controls.Add(lblNomeEquipe);

                Label lblCategoria = new Label
                {
                    Text = equipe.Categoria,
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    Location = new Point(70, 35),
                    AutoSize = true
                };
                pnlEquipe.Controls.Add(lblCategoria);

                if (equipe.UltimaAtividade.HasValue)
                {
                    int dias = (DateTime.Now - equipe.UltimaAtividade.Value).Days;
                    Label lblUltimaAtividade = new Label
                    {
                        Text = $"Última atividade: há {dias} dias",
                        Font = new Font("Segoe UI", 8),
                        Location = new Point(70, 60),
                        AutoSize = true
                    };
                    pnlEquipe.Controls.Add(lblUltimaAtividade);
                }

                var membros = ObterMembrosEquipe(equipe.IdEquipe);
                FlowLayoutPanel pnlMembros = new FlowLayoutPanel
                {
                    Location = new Point(10, 75),
                    Size = new Size(280, 35),
                    AutoScroll = false,
                    WrapContents = false
                };

                foreach (var membro in membros)
                {
                    PictureBox picMembro = new PictureBox
                    {
                        Width = 30,
                        Height = 30,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Cursor = Cursors.Hand,
                        Margin = new Padding(2)
                    };

                    // Carrega a imagem do membro: se tem caminho relativo -> resolve; senão, se blob -> carrega da memória
                    if (!string.IsNullOrEmpty(membro.CaminhoFotoPerfil))
                    {
                        string fullPath = ResolveStoredPathToFullPath(membro.CaminhoFotoPerfil);
                        if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                        {
                            try
                            {
                                using (var ms = new MemoryStream(File.ReadAllBytes(fullPath)))
                                {
                                    picMembro.Image = Image.FromStream(ms);
                                }
                            }
                            catch
                            {
                                picMembro.Image = Properties.Resources.icon_perfil;
                            }
                        }
                        else
                        {
                            picMembro.Image = Properties.Resources.icon_perfil;
                        }
                    }
                    else if (membro.FotoBlob != null && membro.FotoBlob.Length > 0)
                    {
                        try
                        {
                            using (var ms = new MemoryStream(membro.FotoBlob))
                            {
                                picMembro.Image = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            picMembro.Image = Properties.Resources.icon_perfil;
                        }
                    }
                    else
                    {
                        picMembro.Image = Properties.Resources.icon_perfil;
                    }

                    tt.SetToolTip(picMembro, membro.Nome);
                    pnlMembros.Controls.Add(picMembro);
                }
                pnlEquipe.Controls.Add(pnlMembros);
                flowLayoutPanelEquipes.Controls.Add(pnlEquipe);
            }
        }

        private class EquipeInfo
        {
            public int IdEquipe { get; set; }
            public string NomeEquipe { get; set; }
            public string NomeArquivoFoto { get; set; }
            public string Categoria { get; set; }
            public DateTime? UltimaAtividade { get; set; }
        }

        private class MembroInfo
        {
            public string Nome { get; set; }
            public string CaminhoFotoPerfil { get; set; }
            public byte[] FotoBlob { get; set; }
        }

        private List<EquipeInfo> ObterEquipesComUltimaAtividade(int idUsuario, bool isFuncionario)
        {
            var listaEquipes = new List<EquipeInfo>();
            string query;

            if (isFuncionario)
            {
                query = @"
                    SELECT eq.id_equipe, eq.nome_equipe, c.nome_categoria, ua.ultima_atividade
                    FROM Equipes_Membros em
                    JOIN Equipes eq ON em.id_equipe = eq.id_equipe
                    JOIN Categorias c ON eq.id_categoria = c.id_categoria
                    LEFT JOIN UltimaAtividadeEquipe ua ON ua.id_equipe = eq.id_equipe
                    WHERE em.FuncionarioId = @idUsuario";
            }
            else
            {
                query = @"
                    SELECT eq.id_equipe, eq.nome_equipe, c.nome_categoria, ua.ultima_atividade
                    FROM Equipes eq
                    JOIN Categorias c ON eq.id_categoria = c.id_categoria
                    LEFT JOIN UltimaAtividadeEquipe ua ON ua.id_equipe = eq.id_equipe
                    WHERE eq.AdminId = @idUsuario";
            }

            string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idEquipe = reader.GetInt32("id_equipe");
                            string nomeArquivoFoto = ObterFotoEquipeNomeArquivo(idEquipe);

                            listaEquipes.Add(new EquipeInfo
                            {
                                IdEquipe = idEquipe,
                                NomeEquipe = reader.GetString("nome_equipe"),
                                NomeArquivoFoto = nomeArquivoFoto,
                                Categoria = reader.GetString("nome_categoria"),
                                UltimaAtividade = reader.IsDBNull(reader.GetOrdinal("ultima_atividade")) ? (DateTime?)null : reader.GetDateTime("ultima_atividade")
                            });
                        }
                    }
                }
            }
            return listaEquipes;
        }

        private List<MembroInfo> ObterMembrosEquipe(int idEquipe)
        {
            var membros = new List<MembroInfo>();
            string query = @"
                SELECT f.Nome, f.foto_perfil
                FROM Funcionarios f
                JOIN Equipes_Membros em ON f.FuncionarioId = em.FuncionarioId
                WHERE em.id_equipe = @idEquipe";

            string connectionString = "Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;SslMode=none;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEquipe", idEquipe);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object fotoData = reader["foto_perfil"];
                            string caminhoFoto = null;
                            byte[] blobFoto = null;

                            if (fotoData != null && fotoData != DBNull.Value)
                            {
                                if (fotoData is byte[] bytes)
                                {
                                    // Tenta decodificar como string (caminho)
                                    string possivel = TryDecodeUtf8(bytes);
                                    if (LooksLikePath(possivel))
                                    {
                                        caminhoFoto = possivel;
                                    }
                                    else
                                    {
                                        // trata como blob binário real (imagem)
                                        blobFoto = bytes;
                                    }
                                }
                                else if (fotoData is string caminho)
                                {
                                    caminhoFoto = caminho;
                                }
                            }

                            membros.Add(new MembroInfo
                            {
                                Nome = reader.GetString("Nome"),
                                CaminhoFotoPerfil = caminhoFoto,
                                FotoBlob = blobFoto
                            });
                        }
                    }
                }
            }
            return membros;
        }

        private void PreencherCamposFuncionario()
        {
            lblNomeFunc.Text = funcionario.getNome();
            lblCargo.Text = funcionario.getCargo();
            txtNome.Text = funcionario.getNome();
            txtNome.ReadOnly = true;
            txtCPF.Text = funcionario.getCPF();
            txtCPF.ReadOnly = true;
            txtDataNascFunc.Text = funcionario.getDataNascimento().ToString("dd/MM/yyyy");
            txtDataNascFunc.ReadOnly = true;
            txtTelefone.Text = funcionario.getTelefone();
            txtTelefone.ReadOnly = false;
            txtEmail.Text = funcionario.getEmail();
            txtEmail.ReadOnly = true;
            textBox1.Text = $"{funcionario.getEndereco()}, {funcionario.getNumero()}";

            pontuacaoUsuarios ptFunc = new pontuacaoUsuarios();
            int idFunc = int.Parse(funcionario.getFuncionarioId());
            int pontos = ptFunc.ObterPontos(idFunc);
            lblPontos.Text = $"{pontos}";
        }

        private void PreencherCamposAdmin()
        {
            lblNomeFunc.Text = admin.getNome();
            lblCargo.Text = admin.getNome();
            txtNome.Text = admin.getNome();
            lblCargo.Text = admin.getCargo();
            txtCPF.Text = admin.getCPF();
            txtDataNascFunc.Text = admin.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = admin.getTelefone();
            txtEmail.Text = admin.getEmail();
            textBox1.Text = $"{admin.getEndereco()}, {admin.getNum()}";

            lblPontos.Text = "Administrador";
        }

        private void Configuracoes_Load(object sender, EventArgs e)
        {
            int idUsuario = 0;
            bool isFuncionario = false;
            if (funcionario != null)
            {
                idUsuario = int.Parse(funcionario.getFuncionarioId());
                isFuncionario = true;
                CarregarFotoDoBanco(idUsuario, true);
            }
            else if (admin != null)
            {
                idUsuario = int.Parse(admin.getAdminId());
                isFuncionario = false;
                CarregarFotoDoBanco(idUsuario, false);
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
                return;
            }
            CarregarEquipesDoUsuario(idUsuario, isFuncionario);
        }

        private void btnTrocarFotoPerfil_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Selecione a nova foto do perfil",
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string nomeArquivoOriginal = Path.GetFileName(ofd.FileName);
                    string randomName = $"{new Random().Next(1000, 1000000)}-{nomeArquivoOriginal}";
                    randomName = Regex.Replace(randomName, @"\s+", "-");

                    string pastaUpload = baseImgFolder; // C:\xampp\htdocs\dev4tech\img
                    string caminhoCompleto = Path.Combine(pastaUpload, randomName);

                    // Copia o arquivo para a pasta img/
                    File.Copy(ofd.FileName, caminhoCompleto, true);

                    // Atualiza imagem na tela (ler em memória para evitar bloqueio)
                    using (var ms = new MemoryStream(File.ReadAllBytes(caminhoCompleto)))
                    {
                        IconFuncionario.Image = Image.FromStream(ms);
                        IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                    // Salva apenas o caminho relativo no banco (ex: img/nome.jpg)
                    string caminhoRelativo = "img/" + randomName;
                    AtualizarFotoNoBancoComoCaminho(caminhoRelativo);

                    MessageBox.Show("Foto de perfil atualizada com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao processar imagem: {ex.Message}");
                }
            }
        }

        private void AtualizarFotoNoBancoComoCaminho(string caminhoRelativo)
        {
            string connectionString = "server=localhost;database=Dev4Tech;uid=root;pwd=";

            if (funcionario != null)
            {
                int idFuncionario = int.Parse(funcionario.getFuncionarioId());
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Funcionarios SET foto_perfil = @caminho WHERE FuncionarioId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@caminho", caminhoRelativo);
                        cmd.Parameters.AddWithValue("@id", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else if (admin != null)
            {
                int idAdmin = int.Parse(admin.getAdminId());
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Administradores SET foto_perfil = @caminho WHERE AdminId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@caminho", caminhoRelativo);
                        cmd.Parameters.AddWithValue("@id", idAdmin);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void AtualizarFotoNoBancoComoBlob(byte[] imageData)
        {
            if (funcionario != null)
            {
                int idFuncionario = int.Parse(funcionario.getFuncionarioId());
                using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
                {
                    conn.Open();
                    string query = "UPDATE Funcionarios SET foto_perfil = @foto WHERE FuncionarioId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@foto", imageData);
                        cmd.Parameters.AddWithValue("@id", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else if (admin != null)
            {
                int idAdmin = int.Parse(admin.getAdminId());
                using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
                {
                    conn.Open();
                    string query = "UPDATE Administradores SET foto_perfil = @foto WHERE AdminId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@foto", imageData);
                        cmd.Parameters.AddWithValue("@id", idAdmin);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CarregarFotoDoBanco(int id, bool isFuncionario)
        {
            try
            {
                string connectionString = "server=localhost;database=Dev4Tech;uid=root;pwd=";

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = isFuncionario
                        ? "SELECT foto_perfil FROM Funcionarios WHERE FuncionarioId = @id"
                        : "SELECT foto_perfil FROM Administradores WHERE AdminId = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        var result = cmd.ExecuteScalar();

                        // Configura imagem padrão
                        IconFuncionario.Image = Properties.Resources.icon_perfil;
                        IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;

                        if (result != null && result != DBNull.Value)
                        {
                            // Caso venha como byte[] (campo LONGBLOB)
                            if (result is byte[] rawBytes)
                            {
                                // Tenta decodificar como string (caminho)
                                string possivelCaminho = TryDecodeUtf8(rawBytes);

                                if (!string.IsNullOrEmpty(possivelCaminho) && LooksLikePath(possivelCaminho))
                                {
                                    // Resolve e tenta carregar do disco
                                    string full = ResolveStoredPathToFullPath(possivelCaminho);
                                    if (!string.IsNullOrEmpty(full) && File.Exists(full))
                                    {
                                        try
                                        {
                                            using (var ms = new MemoryStream(File.ReadAllBytes(full)))
                                            {
                                                IconFuncionario.Image = Image.FromStream(ms);
                                                IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                                                return;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Erro ao carregar imagem do arquivo: {ex.Message}");
                                        }
                                    }
                                }

                                // Se não for caminho, tenta carregar como imagem binária
                                try
                                {
                                    using (var ms = new MemoryStream(rawBytes))
                                    {
                                        IconFuncionario.Image = Image.FromStream(ms);
                                        IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível interpretar blob como imagem: {ex.Message}");
                                    // Se possivelCaminho existir, tentamos novamente usar como caminho (fallback)
                                    if (!string.IsNullOrEmpty(possivelCaminho))
                                    {
                                        string full2 = ResolveStoredPathToFullPath(possivelCaminho);
                                        if (!string.IsNullOrEmpty(full2) && File.Exists(full2))
                                        {
                                            try
                                            {
                                                using (var ms = new MemoryStream(File.ReadAllBytes(full2)))
                                                {
                                                    IconFuncionario.Image = Image.FromStream(ms);
                                                    IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                                                    return;
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }
                            // Caso venha propriamente como string
                            else if (result is string caminhoRelativo)
                            {
                                string caminhoCompleto = ResolveStoredPathToFullPath(caminhoRelativo);
                                if (!string.IsNullOrEmpty(caminhoCompleto) && File.Exists(caminhoCompleto))
                                {
                                    try
                                    {
                                        using (var ms = new MemoryStream(File.ReadAllBytes(caminhoCompleto)))
                                        {
                                            IconFuncionario.Image = Image.FromStream(ms);
                                            IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Erro ao carregar imagem do arquivo: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Arquivo não encontrado: {caminhoCompleto}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar foto: {ex.Message}");
                IconFuncionario.Image = Properties.Resources.icon_perfil;
                IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btnEditDadosConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string novoTelefone = txtTelefone.Text.Trim();

                // Verificar se o telefone não está vazio
                if (string.IsNullOrWhiteSpace(novoTelefone))
                {
                    MessageBox.Show("Por favor, insira um número de telefone.");
                    return;
                }

                using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
                {
                    conn.Open();

                    if (admin != null)
                    {
                        // Atualizar administrador
                        string idStrAdm = admin.getAdminId();
                        if (string.IsNullOrWhiteSpace(idStrAdm) || !int.TryParse(idStrAdm, out int idAdmin))
                        {
                            MessageBox.Show("ID do administrador inválido.");
                            return;
                        }

                        string queryAdm = "UPDATE Administradores SET telefone = @telefone WHERE AdminId = @id";
                        using (var cmd = new MySqlCommand(queryAdm, conn))
                        {
                            cmd.Parameters.AddWithValue("@telefone", novoTelefone);
                            cmd.Parameters.AddWithValue("@id", idAdmin);
                            int rows = cmd.ExecuteNonQuery();
                            if (rows > 0)
                            {
                                admin.setTelefone(novoTelefone);
                                MessageBox.Show("Telefone atualizado com sucesso!");
                            }
                            else
                            {
                                MessageBox.Show("Nenhum registro foi atualizado.");
                            }
                        }
                    }
                    else if (funcionario != null)
                    {
                        // Atualizar funcionário
                        string idStrFunc = funcionario.getFuncionarioId();
                        if (string.IsNullOrWhiteSpace(idStrFunc) || !int.TryParse(idStrFunc, out int idFunc))
                        {
                            MessageBox.Show("ID do funcionário inválido.");
                            return;
                        }

                        string queryFunc = "UPDATE Funcionarios SET telefone = @telefone WHERE FuncionarioId = @id";
                        using (var cmd = new MySqlCommand(queryFunc, conn))
                        {
                            cmd.Parameters.AddWithValue("@telefone", novoTelefone);
                            cmd.Parameters.AddWithValue("@id", idFunc);
                            int rows = cmd.ExecuteNonQuery();
                            if (rows > 0)
                            {
                                funcionario.setTelefone(novoTelefone);
                                MessageBox.Show("Telefone atualizado com sucesso!");
                            }
                            else
                            {
                                MessageBox.Show("Nenhum registro foi atualizado.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nenhum usuário logado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar telefone: {ex.Message}");
            }
        }

        // Resto das rotinas de UI seguem idênticas às suas (omitidas aqui por brevidade, mas você as mantém)
        private void label8_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e)
        {
            var funcionarioSessao = Sessao.FuncionarioLogado;
            var adminSessao = Sessao.AdminLogado;

            if (funcionarioSessao != null)
            {
                Home hm = new Home();
                hm.Show();
                this.Hide();
            }
            else if (adminSessao != null)
            {
                HomeAdm hmAdm = new HomeAdm();
                hmAdm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
            }
        }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e)
        {
            var funcionarioSessao = Sessao.FuncionarioLogado;
            var adminSessao = Sessao.AdminLogado;

            if (funcionarioSessao != null)
            {
                Configuracoes config = new Configuracoes(funcionarioSessao);
                config.Show();
                this.Hide();
            }
            else if (adminSessao != null)
            {
                Configuracoes config = new Configuracoes(adminSessao);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
            }
        }
        private void btnEquipes_Click(object sender, EventArgs e)
        {
            PesquisaEquipes p_equipe = new PesquisaEquipes();
            p_equipe.Show();
            this.Hide();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            var funcionarioSessao = Sessao.FuncionarioLogado;
            var adminSessao = Sessao.AdminLogado;

            if (funcionarioSessao != null)
            {
                Home h = new Home();
                h.Show();
                this.Hide();
            }
            else if (adminSessao != null)
            {
                HomeAdm t_equipeAdmin = new HomeAdm();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;

            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }
        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }
        private void picPerfilMembro_Click(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtDataNascFunc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtTelefone_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }
        private void panelDados_Paint(object sender, PaintEventArgs e) { }

        private void CarregarFotoUsuario()
        {
            try
            {
                var usuarioFoto = new UsuarioFoto();
                Image foto = usuarioFoto.ObterFotoUsuario();

                if (picPerfil != null) // Verifica se o controle existe no form
                {
                    if (foto != null)
                    {
                        picPerfil.Image = foto;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        // Usar imagem padrão se não encontrar foto
                        picPerfil.Image = Properties.Resources.icon_perfil;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar foto do usuário: {ex.Message}");
                if (picPerfil != null)
                {
                    picPerfil.Image = Properties.Resources.icon_perfil;
                    picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }
    }
}

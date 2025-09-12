using MySql.Data.MySqlClient;
using System;
using System.Drawing; // Adicionado para FontStyle.Bold e manipulação de imagens
using System.Drawing.Imaging; // Adicionado para usar o método FirstOrDefault
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Dev4Tech
{
    public partial class Configuracoes : Form
    {
        private empresaCadFuncionario funcionario;
        private empresaCadAdmin admin;

        // Construtor para funcionário
        public Configuracoes(empresaCadFuncionario func)
        {
            InitializeComponent();
            funcionario = func;
            admin = null; // Garante que admin é nulo
            PreencherCamposFuncionario();

            this.Load += Perfil_Load; // Adiciona evento Load para carregar foto
            IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage; // Define modo stretch no PictureBox
        }

        // Construtor para administrador
        public Configuracoes(empresaCadAdmin adm)
        {
            InitializeComponent();
            admin = adm;
            funcionario = null; // Garante que funcionario é nulo
            PreencherCamposAdmin();

            this.Load += Perfil_Load; // Adiciona evento Load para carregar foto
            IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage; // Define modo stretch no PictureBox
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
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = equipe.FotoEquipe ?? Properties.Resources.icon_equip
                };
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
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = membro.Foto ?? Properties.Resources.icon_perfil,
                        Cursor = Cursors.Hand,
                        Margin = new Padding(2)
                    };
                    tt.SetToolTip(picMembro, membro.Nome);
                    pnlMembros.Controls.Add(picMembro);
                }
                pnlEquipe.Controls.Add(pnlMembros);

                flowLayoutPanelEquipes.Controls.Add(pnlEquipe);
            }
        }

        // Classes auxiliares para dados
        private class EquipeInfo
        {
            public int IdEquipe { get; set; }
            public string NomeEquipe { get; set; }
            public Image FotoEquipe { get; set; }
            public string Categoria { get; set; }
            public DateTime? UltimaAtividade { get; set; }
        }

        private class MembroInfo
        {
            public string Nome { get; set; }
            public Image Foto { get; set; }
        }

        private List<EquipeInfo> ObterEquipesComUltimaAtividade(int idUsuario, bool isFuncionario)
        {
            var listaEquipes = new List<EquipeInfo>();
            string query;

            if (isFuncionario)
            {
                query = @"
                    SELECT eq.id_equipe, eq.nome_equipe, eq.foto_equipe, c.nome_categoria, ua.ultima_atividade
                    FROM Equipes_Membros em
                    JOIN Equipes eq ON em.id_equipe = eq.id_equipe
                    JOIN Categorias c ON eq.id_categoria = c.id_categoria
                    LEFT JOIN UltimaAtividadeEquipe ua ON ua.id_equipe = eq.id_equipe
                    WHERE em.FuncionarioId = @idUsuario";
            }
            else
            {
                query = @"
                    SELECT eq.id_equipe, eq.nome_equipe, eq.foto_equipe, c.nome_categoria, ua.ultima_atividade
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
                            Image fotoEquipe = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("foto_equipe")))
                            {
                                byte[] bytesImg = (byte[])reader["foto_equipe"];
                                using (var ms = new System.IO.MemoryStream(bytesImg))
                                    fotoEquipe = Image.FromStream(ms);
                            }
                            listaEquipes.Add(new EquipeInfo
                            {
                                IdEquipe = reader.GetInt32("id_equipe"),
                                NomeEquipe = reader.GetString("nome_equipe"),
                                FotoEquipe = fotoEquipe,
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
                            Image fotoMembro = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("foto_perfil")))
                            {
                                byte[] bytesImg = (byte[])reader["foto_perfil"];
                                using (var ms = new System.IO.MemoryStream(bytesImg))
                                    fotoMembro = Image.FromStream(ms);
                            }
                            membros.Add(new MembroInfo
                            {
                                Nome = reader.GetString("Nome"),
                                Foto = fotoMembro
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
            lblCargo.Text = funcionario.getNome();
            txtNome.Text = funcionario.getNome();
            lblCargo.Text = funcionario.getCargo();
            txtCPF.Text = funcionario.getCPF();
            txtDataNascFunc.Text = funcionario.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = funcionario.getTelefone();
            txtEmail.Text = funcionario.getEmail();
            textBox1.Text = $"{funcionario.getEndereco()}, {funcionario.getNumero()}";

            // Obter e mostrar a pontuação atual do funcionário
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

            // Para administrador, pode exibir um texto diferente ou 0 pontos
            lblPontos.Text = "Administrador"; // Ou "0" ou "N/A"
        }

        // Os demais métodos e eventos permanecem os mesmos, sem alterações
        private void label8_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e)
        {
            var funcionarioSessao = Sessao.FuncionarioLogado;
            var adminSessao = Sessao.AdminLogado;

            if (funcionarioSessao != null)
            {
                Home hm = new Home();
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

        // Corrigido para verificar FuncionarioLogado e AdminLogado
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
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
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
            // Limpa a sessão antes de voltar para a tela inicial
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

        private void Configuracoes_Load(object sender, EventArgs e)
        {
            int idUsuario = 0;
            bool isFuncionario = false;
            if (funcionario != null)
            {
                idUsuario = int.Parse(funcionario.getFuncionarioId());
                isFuncionario = true;
            }
            else if (admin != null)
            {
                idUsuario = int.Parse(admin.getAdminId());
                isFuncionario = false;
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
                return;
            }
            CarregarEquipesDoUsuario(idUsuario, isFuncionario);
        }
        // Mantido se associado no Designer
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }

        private void AtualizarFotoNoBanco(byte[] fotoBytes)
        {
            // Adaptar para salvar na tabela correta conforme usuário atual (func/admin)
            if (funcionario != null)
            {
                int idFuncionario = int.Parse(funcionario.getFuncionarioId());
                using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
                {
                    conn.Open();
                    string query = "UPDATE Funcionarios SET foto_perfil = @foto WHERE FuncionarioId = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@foto", fotoBytes);
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
                        cmd.Parameters.AddWithValue("@foto", fotoBytes);
                        cmd.Parameters.AddWithValue("@id", idAdmin);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void btnTrocarFotoPerfil_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecione a nova foto do perfil";
            ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string caminhoImagem = ofd.FileName;
                IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                IconFuncionario.Image = new Bitmap(caminhoImagem);

                // Redimensiona e comprime antes de salvar
                byte[] fotoBytes = RedimensionarEComprimirImagem(caminhoImagem, 256, 256, 70L);

                AtualizarFotoNoBanco(fotoBytes);

                MessageBox.Show("Foto de perfil atualizada!");
            }
        }

        private void Perfil_Load(object sender, EventArgs e)
        {
            if (funcionario != null)
            {
                int idFuncionario = int.Parse(funcionario.getFuncionarioId());
                CarregarFotoDoBanco(idFuncionario, true);
            }
            else if (admin != null)
            {
                int idAdmin = int.Parse(admin.getAdminId());
                CarregarFotoDoBanco(idAdmin, false);
            }
            else
            {
                IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                IconFuncionario.Image = Properties.Resources.icon_perfil;
            }
        }

        private void CarregarFotoDoBanco(int id, bool isFuncionario)
        {
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
            {
                conn.Open();
                string query = isFuncionario
                    ? "SELECT foto_perfil FROM Funcionarios WHERE FuncionarioId = @id"
                    : "SELECT foto_perfil FROM Administradores WHERE AdminId = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        IconFuncionario.SizeMode = PictureBoxSizeMode.StretchImage;
                        if (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0))
                            {
                                byte[] fotoBytes = (byte[])rdr["foto_perfil"];
                                using (var ms = new System.IO.MemoryStream(fotoBytes))
                                {
                                    IconFuncionario.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                IconFuncionario.Image = Properties.Resources.icon_perfil; // imagem padrão
                            }
                        }
                        else
                        {
                            IconFuncionario.Image = Properties.Resources.icon_perfil;
                        }
                    }
                }
            }
        }

        private byte[] RedimensionarEComprimirImagem(string caminhoImagem, int largura, int altura, long qualidade = 70L)
        {
            using (var imagemOriginal = Image.FromFile(caminhoImagem))
            {
                using (var imagemRedimensionada = new Bitmap(largura, altura))
                {
                    using (var g = Graphics.FromImage(imagemRedimensionada))
                    {
                        g.DrawImage(imagemOriginal, 0, 0, largura, altura);
                    }
                    var codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var parametros = new EncoderParameters(1);
                    parametros.Param[0] = new EncoderParameter(Encoder.Quality, qualidade);

                    using (var ms = new System.IO.MemoryStream())
                    {
                        imagemRedimensionada.Save(ms, codec, parametros);
                        return ms.ToArray();
                    }
                }
            }
        }

    }
}

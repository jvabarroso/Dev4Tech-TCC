using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Chat_geral_equipes : Form
    {
        private int idEquipe;
        private string nomeEquipe;
        private string categoriaEquipe;
        private Chat_Mensagens messageChat = new Chat_Mensagens();
        private int mensagensCount = 0;
        private int margemTopo = 10;
        private int espacamentoVertical = 10;
        private int larguraMaxMensagem = 350;
        private int alturaMensagem = 60;

        public Chat_geral_equipes()
        {
            InitializeComponent();
            if (Sessao.IdEquipeSelecionada == 0)
            {
                MessageBox.Show("Nenhuma equipe selecionada. Redirecionando para pesquisa de equipes.");
                PesquisaEquipes pesquisa = new PesquisaEquipes();
                pesquisa.Show();
                this.Close();
                return;
            }

            this.idEquipe = Sessao.IdEquipeSelecionada;
            this.nomeEquipe = Sessao.NomeEquipeSelecionada;
            this.categoriaEquipe = Sessao.CategoriaEquipeSelecionada;

            lblNomeEquipe.Text = nomeEquipe;
            lblCategoriaEquipe.Text = categoriaEquipe;
            CarregarMensagens();
            CarregarFotoEquipe();
            CarregarFotoUsuario();
        }

        public Chat_geral_equipes(int idEquipe, string nomeEquipe, string categoriaEquipe)
        {
            InitializeComponent();
            this.idEquipe = idEquipe;
            this.nomeEquipe = nomeEquipe;
            this.categoriaEquipe = categoriaEquipe;
            lblNomeEquipe.Text = nomeEquipe;
            lblCategoriaEquipe.Text = categoriaEquipe;
            CarregarMensagens();
        }

        private void CarregarMensagens()
        {
            LimparMensagens();

            if (idEquipe == 0)
            {
                MessageBox.Show("ID da equipe não definido.");
                return;
            }

            DataTable dt = messageChat.ConsultarPorEquipe(idEquipe);
            mensagensCount = 0;
            string idUsuarioLogado = null;
            bool usuarioEhAdmin = false;

            if (Sessao.FuncionarioLogado != null)
            {
                idUsuarioLogado = Sessao.FuncionarioLogado.getFuncionarioId();
                usuarioEhAdmin = false;
            }
            else if (Sessao.AdminLogado != null)
            {
                idUsuarioLogado = Sessao.AdminLogado.getAdminId();
                usuarioEhAdmin = true;
            }

            foreach (DataRow row in dt.Rows)
            {
                string idFuncionario = row["FuncionarioId"] == DBNull.Value ? null : row["FuncionarioId"].ToString();
                string idAdmin = row["AdminId"] == DBNull.Value ? null : row["AdminId"].ToString();

                bool minhaMensagem = false;
                if (!string.IsNullOrEmpty(idUsuarioLogado))
                {
                    minhaMensagem = usuarioEhAdmin ? idAdmin == idUsuarioLogado : idFuncionario == idUsuarioLogado;
                }

                string texto = row["texto"].ToString();
                DateTime dataEnvio = Convert.ToDateTime(row["data_envio"]);
                bool mensagemAdministrador = !string.IsNullOrEmpty(idAdmin);
                string nomeUsuario = mensagemAdministrador
                    ? row["nome_admin"]?.ToString() ?? "Administrador"
                    : row["nome_funcionario"]?.ToString() ?? "Funcionário";

                byte[] fotoBytes = mensagemAdministrador
                    ? (row["foto_admin"] != DBNull.Value ? (byte[])row["foto_admin"] : null)
                    : (row["foto_funcionario"] != DBNull.Value ? (byte[])row["foto_funcionario"] : null);

                Image foto;
                if (fotoBytes != null && fotoBytes.Length > 0)
                {
                    try
                    {
                        using (var ms = new MemoryStream(fotoBytes))
                        {
                            ms.Position = 0;
                            foto = Image.FromStream(ms);
                        }
                    }
                    catch (ArgumentException)
                    {
                        foto = Properties.Resources.icon_perfil;
                    }
                }
                else
                {
                    foto = Properties.Resources.icon_perfil;
                }

                int idMensagem = Convert.ToInt32(row["id_mensagem"]);
                int idUsuarioLogadoInt = 0;
                int.TryParse(idUsuarioLogado, out idUsuarioLogadoInt);
                string statusMensagem = row["status"]?.ToString() ?? "enviada";
                int? remetenteFuncionarioId = row["FuncionarioId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["FuncionarioId"]);
                int? remetenteAdminId = row["AdminId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["AdminId"]);

                AdicionarMensagem(texto, dataEnvio, minhaMensagem, mensagemAdministrador, foto, nomeUsuario,
                    idMensagem, idUsuarioLogadoInt, idEquipe, statusMensagem,
                    remetenteFuncionarioId, remetenteAdminId);
            }
        }



        // Versão completa com parâmetros para marcação da visualização
        private void AdicionarMensagem(string texto, DateTime dataEnvio, bool minhaMensagem, bool mensagemAdministrador,
    Image fotoPerfil, string nomeUsuario, int idMensagem, int idUsuarioLogado, int idEquipe, string statusMensagem,
    int? remetenteFuncionarioId, int? remetenteAdminId)
        {
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;
            Color fundoMensagem;
            Color bordaMensagem;
            if (minhaMensagem)
            {
                fundoMensagem = Color.FromArgb(220, 255, 220);
                bordaMensagem = Color.FromArgb(120, 200, 120);
            }
            else if (mensagemAdministrador)
            {
                fundoMensagem = Color.FromArgb(220, 235, 255);
                bordaMensagem = Color.FromArgb(120, 170, 220);
            }
            else
            {
                fundoMensagem = Color.White;
                bordaMensagem = Color.LightGray;
            }
            Panel mensagemPanel = new Panel
            {
                BackColor = fundoMensagem,
                BorderStyle = BorderStyle.None,
                Width = larguraMaxMensagem - 30,
                Height = alturaMensagem + 20,
                Top = y,
                Left = minhaMensagem ? panelMensagens.Width - larguraMaxMensagem + 25 : 45,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Color statusColor;
            if (statusMensagem == "enviada")
                statusColor = Color.Gray;
            else if (statusMensagem == "entregue")
                statusColor = Color.Green;
            else if (statusMensagem == "lida")
                statusColor = Color.Blue;
            else
                statusColor = Color.Gray;
            Panel statusIndicator = new Panel
            {
                Width = 20,
                Height = 20,
                Top = y + (alturaMensagem / 2) - 10,
                Left = minhaMensagem ? panelMensagens.Width - larguraMaxMensagem : 15,
                BackColor = statusColor
            };
            statusIndicator.Paint += (s, e) =>
            {
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddEllipse(0, 0, statusIndicator.Width - 1, statusIndicator.Height - 1);
                statusIndicator.Region = new Region(gp);
            };
            mensagemPanel.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, mensagemPanel.Width - 1, mensagemPanel.Height - 1);
                using (var pen = new Pen(bordaMensagem, 2))
                using (var brush = new SolidBrush(mensagemPanel.BackColor))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(brush, rect);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };
            PictureBox avatar = new PictureBox
            {
                Image = fotoPerfil,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 36,
                Height = 36,
                Top = mensagemPanel.Height - 58,
                Left = minhaMensagem ? mensagemPanel.Width - 60 : 8,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent
            };
            avatar.Paint += (s, e) =>
            {
                var gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddEllipse(0, 0, avatar.Width - 1, avatar.Height - 1);
                avatar.Region = new Region(gp);
            };
            Label lblNome = new Label
            {
                Text = nomeUsuario,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DimGray,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopCenter,
                Left = avatar.Left - 2,
                Top = avatar.Top + avatar.Height + 2
            };
            int larguraMensagem = mensagemPanel.Width - 70;
            int mensagemLeft = minhaMensagem ? 12 : 54;
            int mensagemWidth = minhaMensagem ? larguraMensagem - 40 : larguraMensagem;
            Label lblMensagem = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = false,
                Width = mensagemWidth,
                Height = alturaMensagem - 28,
                Top = 8,
                Left = mensagemLeft,
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.FromArgb(40, 40, 40),
                Padding = new Padding(6, 4, 6, 4),
                BackColor = Color.Transparent
            };
            Label lblHora = new Label
            {
                Text = dataEnvio.ToString("HH:mm"),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                AutoSize = true,
                ForeColor = Color.Gray,
                BackColor = Color.Transparent
            };
            lblHora.Top = mensagemPanel.Height - lblHora.PreferredHeight - 6;
            lblHora.Left = minhaMensagem
                ? mensagemPanel.Width - lblHora.PreferredWidth - 60
                : 60;
            mensagemPanel.Controls.Add(lblMensagem);
            mensagemPanel.Controls.Add(avatar);
            mensagemPanel.Controls.Add(lblNome);
            mensagemPanel.Controls.Add(lblHora);
            panelMensagens.Controls.Add(statusIndicator);
            panelMensagens.Controls.Add(mensagemPanel);

            // Marcação da visualização da mensagem (se for de outro usuário)
            bool isRemetente = false;
            if (Sessao.FuncionarioLogado != null)
            {
                isRemetente = (!mensagemAdministrador && remetenteFuncionarioId.HasValue && remetenteFuncionarioId.Value == idUsuarioLogado);
            }
            else if (Sessao.AdminLogado != null)
            {
                isRemetente = (mensagemAdministrador && remetenteAdminId.HasValue && remetenteAdminId.Value == idUsuarioLogado);
            }
            if (!isRemetente && idMensagem > 0 && idUsuarioLogado > 0 && idEquipe > 0)
            {
                string tipoUsuario = Sessao.FuncionarioLogado != null ? "funcionario" : "admin";
                messageChat.MarcarMensagemVisualizada(idMensagem, idUsuarioLogado, tipoUsuario, idEquipe);
            }

            mensagensCount++;
            panelMensagens.VerticalScroll.Value = Math.Max(0, panelMensagens.VerticalScroll.Maximum);
            panelMensagens.PerformLayout();
        }



        private void LimparMensagens()
        {
            panelMensagens.Controls.Clear();
            mensagensCount = 0;
        }

        private void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDigitarMensagem.Text))
            {
                messageChat.setTexto(txtDigitarMensagem.Text);
                messageChat.setDataEnvio(DateTime.Now);
                messageChat.setIdEquipe(idEquipe);

                if (Sessao.FuncionarioLogado != null)
                {
                    messageChat.setIdFuncionario(Convert.ToInt32(Sessao.FuncionarioLogado.getFuncionarioId()));
                    messageChat.setIdAdmin(null);
                }
                else if (Sessao.AdminLogado != null)
                {
                    messageChat.setIdAdmin(Convert.ToInt32(Sessao.AdminLogado.getAdminId()));
                    messageChat.setIdFuncionario(null);
                }
                else
                {
                    MessageBox.Show("Nenhum usuário logado para enviar mensagem");
                    return;
                }

                messageChat.inserir();
                messageChat.AtualizarUltimaAtividade(idEquipe);
                CarregarMensagens();
                txtDigitarMensagem.Clear();
            }
        }

        private void CarregarFotoEquipe()
        {
            try
            {
                Chat_Mensagens chatMensagens = new Chat_Mensagens();
                Image fotoEquipe = chatMensagens.ObterFotoEquipe(this.idEquipe);

                if (fotoEquipe != null)
                {
                    iconFotoEquipe.Image = fotoEquipe;
                    iconFotoEquipe.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    // Usar imagem padrão se não encontrar foto
                    iconFotoEquipe.Image = Properties.Resources.icon_EquipLogo;
                    iconFotoEquipe.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar foto da equipe: {ex.Message}");
                iconFotoEquipe.Image = Properties.Resources.icon_EquipLogo;
                iconFotoEquipe.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void lblRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }

        private void Chat_geral_equipes_Load(object sender, EventArgs e)
        {
            // Pode implementar ou deixar vazio
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                PesquisaEquipes t_equipe = new PesquisaEquipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
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

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Tarefas_Pendentes t_equipe = new Tarefas_Pendentes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AvaliaçãoTarefaAdmin t_equipeAdmin = new AvaliaçãoTarefaAdmin();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblGeral_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes chatEquipe = new Chat_geral_equipes();
            chatEquipe.Show();
            this.Hide();
        }

        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void btnLimparChat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Home h = new Home();
                h.Show();
                this.Hide();
            }
            else if (admin != null)
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

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Configuracoes config = new Configuracoes(funcionario);
                config.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Configuracoes config = new Configuracoes(admin);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
            }
        }

        private void picPerfil_Click(object sender, EventArgs e)
        {
            // Configurações futuras
        }

        private void txtDigitarMensagem_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (Sessao.IdEquipeSelecionada != 0)
            {
                int idEquipe = Sessao.IdEquipeSelecionada;
                string nomeEquipe = "Nome da equipe"; // Ajuste para obter o nome real da equipe
                string categoriaEquipe = "Categoria da equipe"; // Ajuste para obter a categoria real da equipe

                if (funcionario != null)
                {
                    Planejamento t_equipe = new Planejamento();
                    t_equipe.Show();
                    this.Hide();
                }
                else if (admin != null)
                {
                    MessageBox.Show("Tela voltada para tarefas dos funcionários.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Chat_geral_equipes t_equipeAdmin = new Chat_geral_equipes(idEquipe, nomeEquipe, categoriaEquipe);
                    t_equipeAdmin.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Nenhuma equipe selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PesquisaEquipes pesquisa = new PesquisaEquipes();
                pesquisa.Show();
                this.Hide();
            }
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Tarefas_Pendentes t_equipe = new Tarefas_Pendentes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AdicionarTarefa t_equipeAdmin = new AdicionarTarefa();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblGeral_Click_1(object sender, EventArgs e)
        {
            Chat_geral_equipes t_chat = new Chat_geral_equipes();
            t_chat.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Planejamento p_plano = new Planejamento();
            p_plano.Show();
            this.Hide();
        }
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
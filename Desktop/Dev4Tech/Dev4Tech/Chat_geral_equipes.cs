using System;
using System.Data;
using System.Drawing;
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
            idEquipe = 0;
            nomeEquipe = "Equipe não definida";
            categoriaEquipe = "Categoria não definida";
            lblNomeEquipe.Text = nomeEquipe;
            lblCategoriaEquipe.Text = categoriaEquipe;
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
            DataTable dt = messageChat.ConsultarPorEquipe(idEquipe);
            mensagensCount = 0;

            string idFuncionarioLogado = null;
            if (Sessao.FuncionarioLogado != null)
            {
                idFuncionarioLogado = Sessao.FuncionarioLogado.getFuncionarioId();
            }

            foreach (DataRow row in dt.Rows)
            {
                string idRemetente = row.Table.Columns.Contains("FuncionarioId") && row["FuncionarioId"] != DBNull.Value
                    ? row["FuncionarioId"].ToString()
                    : null;

                string texto = row["texto"].ToString();
                DateTime dataEnvio = Convert.ToDateTime(row["data_envio"]);

                bool minhaMensagem = (idFuncionarioLogado != null && idRemetente == idFuncionarioLogado);

                AdicionarMensagem(texto, dataEnvio, minhaMensagem);
            }
        }

        private void AdicionarMensagem(string texto, DateTime dataEnvio, bool minhaMensagem)
        {
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;

            Panel mensagemPanel = new Panel
            {
                BackColor = minhaMensagem ? Color.LightGreen : Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Width = larguraMaxMensagem,
                Height = alturaMensagem,
                Top = y,
                Left = minhaMensagem ? panelMensagens.Width - larguraMaxMensagem - 20 : 20,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            PictureBox avatar = new PictureBox
            {
                Image = minhaMensagem ? Properties.Resources.icon_perfil : Properties.Resources.icon_EquipLogo,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 32,
                Height = 32,
                Top = (alturaMensagem - 32) / 2,
                Left = minhaMensagem ? mensagemPanel.Width - 42 : 10,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblMensagem = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = false,
                Width = larguraMaxMensagem - 60,
                Height = alturaMensagem - 20,
                Top = 10,
                Left = minhaMensagem ? 10 : 50,
                TextAlign = ContentAlignment.MiddleLeft,
                MaximumSize = new Size(larguraMaxMensagem - 60, 0),
                AutoEllipsis = true
            };

            Label lblHora = new Label
            {
                Text = dataEnvio.ToString("HH:mm"),
                Font = new Font("Segoe UI", 7, FontStyle.Italic),
                AutoSize = true,
                Top = mensagemPanel.Height - 18,
                Left = minhaMensagem ? 10 : 50,
                ForeColor = Color.Gray
            };

            mensagemPanel.Controls.Add(lblMensagem);
            mensagemPanel.Controls.Add(avatar);
            mensagemPanel.Controls.Add(lblHora);

            panelMensagens.Controls.Add(mensagemPanel);
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
                }

                messageChat.inserir();
                messageChat.AtualizarUltimaAtividade(idEquipe);
                CarregarMensagens();
                txtDigitarMensagem.Clear();
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
            txtDigitarMensagem.Text = "";
        }

        private void txtDigitarMensagem_TextChanged(object sender, EventArgs e)
        {
            // Sem alterações
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
    }
}
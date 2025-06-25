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
        private int margemTopo = 30;
        private int margemEsquerda = 350;
        private int espacamentoVertical = 20;
        private int alturaMensagem = 50;

        // Construtor padrão (sem parâmetros) para chamadas existentes e Designer
        public Chat_geral_equipes()
        {
            InitializeComponent();
            idEquipe = 0;
            nomeEquipe = "Equipe não definida";
            categoriaEquipe = "Categoria não definida";

            lblNomeEquipe.Text = nomeEquipe;
            lblCategoriaEquipe.Text = categoriaEquipe;
        }

        // Construtor com parâmetros para abrir chat de equipe específica
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

            foreach (DataRow row in dt.Rows)
            {
                AdicionarMensagem(row["texto"].ToString());
            }
        }

        private void AdicionarMensagem(string texto)
        {
            int x = margemEsquerda;
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;

            Panel mensagemPanel = new Panel
            {
                Width = 350,
                Height = alturaMensagem,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Left = x,
                Top = y
            };

            PictureBox avatar = new PictureBox
            {
                Image = Properties.Resources.icon_EquipLogo,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 32,
                Height = 32,
                Left = 10,
                Top = 6,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblMensagem = new Label
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = false,
                Width = 270,
                Height = 32,
                Left = 50,
                Top = 6,
                TextAlign = ContentAlignment.MiddleLeft
            };

            mensagemPanel.Controls.Add(avatar);
            mensagemPanel.Controls.Add(lblMensagem);

            panelMensagens.Controls.Add(mensagemPanel);

            mensagensCount++;
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
                messageChat.inserir();

                messageChat.AtualizarUltimaAtividade(idEquipe);

                CarregarMensagens();
                txtDigitarMensagem.Clear();
            }
        }

        // Métodos de evento exigidos pelo Designer

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

        // Outros eventos e métodos que você já tinha

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            PesquisaEquipes P_equipe = new PesquisaEquipes();
            P_equipe.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes trf_pendente = new Tarefas_Pendentes();
            trf_pendente.Show();
            this.Hide();
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
            Home h = new Home();
            h.Show();
            this.Hide();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;

            if (funcionario != null)
            {
                Configuracoes config = new Configuracoes(funcionario);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum funcionário logado.");
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
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
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

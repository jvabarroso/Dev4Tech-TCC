using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Chat_geral_equipes : Form
    {
        public Chat_geral_equipes()
        {
            InitializeComponent();
        }

        Chat_Mensagens MessageChat = new Chat_Mensagens();

       // Variáveis globais para controle de posição
private int mensagensCount = 0;
private int margemTopo = 30;
private int margemEsquerda = 350;
private int espacamentoVertical = 20;
private int alturaMensagem = 50;

        private void AdicionarMensagem(string texto)
        {
            int x = margemEsquerda; // fixa na coluna esquerda
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount; // empilha verticalmente

            // Cria painel da mensagem
            Panel mensagemPanel = new Panel();
            mensagemPanel.Width = 350;
            mensagemPanel.Height = alturaMensagem;
            mensagemPanel.BackColor = Color.White;
            mensagemPanel.BorderStyle = BorderStyle.FixedSingle;
            mensagemPanel.Left = x;
            mensagemPanel.Top = y;

            // Avatar
            PictureBox avatar = new PictureBox();
            avatar.Image = Properties.Resources.icon_EquipLogo;
            avatar.SizeMode = PictureBoxSizeMode.StretchImage;
            avatar.Width = 32;
            avatar.Height = 32;
            avatar.Left = 10;
            avatar.Top = 6;
            avatar.BorderStyle = BorderStyle.FixedSingle;

            // Label da mensagem
            Label lblMensagem = new Label();
            lblMensagem.Text = texto;
            lblMensagem.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblMensagem.AutoSize = false;
            lblMensagem.Width = 270;
            lblMensagem.Height = 32;
            lblMensagem.Left = 50;
            lblMensagem.Top = 6;
            lblMensagem.TextAlign = ContentAlignment.MiddleLeft;

            mensagemPanel.Controls.Add(avatar);
            mensagemPanel.Controls.Add(lblMensagem);

            panelMensagens.Controls.Add(mensagemPanel);

            mensagensCount++;
        }


        private void btnEquipes_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas t_equipe = new Equipes_Estatisticas();
            t_equipe.Show();
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

        private void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDigitarMensagem.Text))
            {
                AdicionarMensagem(txtDigitarMensagem.Text);
           
            }
            MessageChat.setTexto(txtDigitarMensagem.Text);
            MessageChat.setDataEnvio(DateTime.Now);

          

            MessageChat.inserir();
            txtDigitarMensagem.Clear();

        }
        private void LimparMensagens()
        {
            panelMensagens.Controls.Clear();
            mensagensCount = 0;
        }



        private void btnLimparChat_Click(object sender, EventArgs e)
        {
            // TODO: Implementar limpeza do chat
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnCadastro_Click(object sender, EventArgs e) { }
        private void btnHome_Click(object sender, EventArgs e) { }
        private void lblRanking_Click(object sender, EventArgs e) { }
        private void txtDigitarMensagem_Click(object sender, EventArgs e) { txtDigitarMensagem.Text = ""; }
        private void txtDigitarMensagem_TextChanged(object sender, EventArgs e) { }

        private void Chat_geral_equipes_Load(object sender, EventArgs e)
        {
            
        }
    }
}

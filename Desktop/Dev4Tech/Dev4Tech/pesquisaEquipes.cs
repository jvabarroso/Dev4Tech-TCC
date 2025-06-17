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
    public partial class PesquisaEquipes : Form
    {
        public PesquisaEquipes()
        {
            InitializeComponent();

            filtroEquipes.Items.Add("Todos");
            filtroEquipes.Items.Add("Desenvolvedor de software");
            filtroEquipes.Items.Add("Design");
            filtroEquipes.Items.Add("Marketing");
            filtroEquipes.SelectedIndex = 0; //Seleciona "Todos" por padrão
        }

        private int mensagensCount = 0;
        private int margemTopo = 30;
        private int margemEsquerda = 350;
        private int espacamentoVertical = 20;
        private int alturaMensagem = 50;

        private void AdicionarEquipes(string texto)
        {
            int x = margemEsquerda; // fixa na coluna esquerda
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount; // empilha verticalmente

            // Cria painel da mensagem
            Panel EquipesPanel = new Panel();
            EquipesPanel.Width = 350;
            EquipesPanel.Height = alturaMensagem;
            EquipesPanel.BackColor = Color.White;
            EquipesPanel.BorderStyle = BorderStyle.FixedSingle;
            EquipesPanel.Left = x;
            EquipesPanel.Top = y;

            // Avatar
            PictureBox avatar = new PictureBox();
            avatar.Image = Properties.Resources.icon_equip;
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

            EquipesPanel.Controls.Add(avatar);
            EquipesPanel.Controls.Add(lblMensagem);

            panelEquipes.Controls.Add(EquipesPanel);

            mensagensCount++;
        }

        private void txtPesquisaEquipe_Click(object sender, EventArgs e)
        {
            txtPesquisaEquipe.Text = "";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home t_Home = new Home();
            t_Home.Show();
            this.Hide();
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas t_Equipes = new Equipes_Estatisticas();
            t_Equipes.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes t_Ranking = new Ranking_Equipes();
            t_Ranking.Show();
            this.Hide();
        }

        private void txtPesquisarEquipe_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPesquisaEquipe.Text))
            {
                txtPesquisaEquipe.Text = "Pesquisar Equipe";
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {

        }

        private void btnTarefas_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_Tarefas = new Tarefas_Pendentes();
            t_Tarefas.Show();
            this.Hide();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes t_Chat = new Chat_geral_equipes();
            t_Chat.Show();
            this.Hide();
        }

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_Integrantes = new Integrantes_Equipe();
            t_Integrantes.Show();
            this.Hide();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            // TODO: Implementar pesquisa de equipes
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnAdicionarEquipe_Click(object sender, EventArgs e)
        {
            AdicionarEquipes adicionarEquipe = new AdicionarEquipes();
            adicionarEquipe.Show();
            this.Hide();
        }

        private void btnEquipe_Click(object sender, EventArgs e) { }
        private void btnCalendar_Click(object sender, EventArgs e) { }
        private void lblRanking_Click(object sender, EventArgs e) { }
        private void lblEquipe_Click(object sender, EventArgs e) { }
    }
}

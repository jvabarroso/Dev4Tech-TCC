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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
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

        private void label1_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void btnEntrarEquipes_Click(object sender, EventArgs e) { }
        private void btnEntrarRanking_Click(object sender, EventArgs e) { }
        private void btnConfiguracoes_Click(object sender, EventArgs e) { }
        private void btnHome_Click(object sender, EventArgs e) { }
        private void Home_Load(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void btnEntrarTarefas_Click(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e) { }
    }
}

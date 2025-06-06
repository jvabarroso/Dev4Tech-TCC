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
    public partial class Integrantes_Equipe : Form
    {
        public Integrantes_Equipe()
        {
            InitializeComponent();
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
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

        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void btnAdicionarMembro_Click(object sender, EventArgs e)
        {
            // TODO: Implementar adição de membros
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnRemoverMembro_Click(object sender, EventArgs e)
        {
            // TODO: Implementar remoção de membros
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void Integrantes_Equipe_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void lblPlanejamento_Click(object sender, EventArgs e)
        {

        }

        private void txtProcurarMebros_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMostrarMembros_Click(object sender, EventArgs e)
        {

        }

        private void lblRanking_Click(object sender, EventArgs e) { }
        private void lblTarefas_Click(object sender, EventArgs e) { }
        private void lblGeral_Click(object sender, EventArgs e) { }
    }
}

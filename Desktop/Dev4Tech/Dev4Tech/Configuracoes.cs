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
    public partial class Configuracoes : Form
    {
        public Configuracoes()
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home t_Home = new Home();
            t_Home.Show();
            this.Hide();
        }

        private void btnTarefas_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes trf_pendente = new Tarefas_Pendentes();
            trf_pendente.Show();
            this.Hide();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes chatEquipe = new Chat_geral_equipes();
            chatEquipe.Show();
            this.Hide();
        }

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // TODO: Implementar salvamento das configurações
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}

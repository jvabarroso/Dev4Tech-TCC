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

        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void Integrantes_Equipe_Load(object sender, EventArgs e)
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

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Hide();
        }

        private void btnEquipes_Click_1(object sender, EventArgs e)
        {
            Equipes_Estatisticas equip_e = new Equipes_Estatisticas();
            equip_e.Show();
            this.Hide();
        }

        private void btnRanking_Click_1(object sender, EventArgs e)
        {
            Ranking_Equipes rank = new Dev4Tech.Ranking_Equipes();
            rank.Show();
            this.Hide();
        }

        private void btnConfigurações_Click(object sender, EventArgs e)
        {
            //Configuracoes config = new Configuracoes();
            //config.Show();
            //this.Hide();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
    }
}

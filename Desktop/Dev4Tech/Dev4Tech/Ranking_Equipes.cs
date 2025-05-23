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
    public partial class Ranking_Equipes : Form
    {
        public Ranking_Equipes()
        {
            InitializeComponent();
        }

        private void lblGeral_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes chat_equipe = new Chat_geral_equipes();
            chat_equipe.Show();
            this.Hide();
        }

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes trf_pendente = new Tarefas_Pendentes();
            trf_pendente.Show();
            this.Hide(); 
        }

        private void lblRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_inicial = new Form1();
            t_inicial.Show();
            this.Hide(); 
        }

        private void Ranking_Equipes_Load(object sender, EventArgs e)
        {

        }
    }
}

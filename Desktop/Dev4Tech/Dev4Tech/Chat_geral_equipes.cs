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
            // TODO: Implementar envio de mensagens
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnLimparChat_Click(object sender, EventArgs e)
        {
            // TODO: Implementar limpeza do chat
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnCadastro_Click(object sender, EventArgs e) { }
        private void btnHome_Click(object sender, EventArgs e) { }
        private void lblRanking_Click(object sender, EventArgs e) { }
        private void txtDigitarMensagem_Click(object sender, EventArgs e) { }
        private void txtDigitarMensagem_TextChanged(object sender, EventArgs e) { }
    }
}

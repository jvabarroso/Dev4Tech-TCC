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
    public partial class Tarefas_Pendentes : Form
    {
        public Tarefas_Pendentes()
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

        private void btnConcluirTarefa_Click(object sender, EventArgs e)
        {
            // TODO: Implementar conclusão de tarefa
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnReportarProblema_Click(object sender, EventArgs e)
        {
            Relato_Problema relato = new Relato_Problema();
            relato.Show();
            this.Hide();
        }

        private void btnEnviarComentario_Click(object sender, EventArgs e)
        {
            // TODO: Implementar envio de comentários
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void lblRanking_Click(object sender, EventArgs e) { }
        private void btnPendentes_Click(object sender, EventArgs e) { }
        private void btnEmAtraso_Click(object sender, EventArgs e) { }
        private void btnCompletadas_Click(object sender, EventArgs e) { }
        private void Tarefa1_Enter(object sender, EventArgs e) { }
        private void txtPesquisaTarefa_TextChanged(object sender, EventArgs e) { }
        private void btnEquipe_Click(object sender, EventArgs e) { }
        private void btnHome_Click(object sender, EventArgs e) { }
        private void lblPlanejamento_Click(object sender, EventArgs e) { }
    }
}

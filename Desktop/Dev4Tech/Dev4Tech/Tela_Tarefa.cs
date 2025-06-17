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
    public partial class Tela_Tarefa : Form
    {
        public Tela_Tarefa()
        {
            InitializeComponent();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
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

        private void btnReportarProblema_Click(object sender, EventArgs e)
        {
            Relato_Problema relato = new Relato_Problema();
            relato.Show();
            this.Hide();
        }

        private void btnEnviarComentario_Click(object sender, EventArgs e)
        {
            // TODO: Implementar envio de comentário
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnPlanejamento_Click(object sender, EventArgs e)
        {
            Planejamento planejamento = new Planejamento();
            planejamento.Show();
            this.Hide();
        }

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe integrantes = new Integrantes_Equipe();
            integrantes.Show();
            this.Hide();
        }

        private void btnRankingEquipes_Click(object sender, EventArgs e)
        {
            Ranking_Equipes ranking = new Ranking_Equipes();
            ranking.Show();
            this.Hide();
        }

        private void btnTarefasPendentes_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes tarefas = new Tarefas_Pendentes();
            tarefas.Show();
            this.Hide();
        }

        private void btnChatGeral_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes chat = new Chat_geral_equipes();
            chat.Show();
            this.Hide();
        }

        private void btnEquipesEstatisticas_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas t_equipe = new Equipes_Estatisticas();
            t_equipe.Show();
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

        private void lblRanking_Click(object sender, EventArgs e) { }
        private void btnEnviar_Click(object sender, EventArgs e) { }
        private void btnRelatarProblema_Click(object sender, EventArgs e) { }
        private void txtDescrição_TextChanged(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e) { }
        private void lblPlanejamento_Click(object sender, EventArgs e) { }
    }
}

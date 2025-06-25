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

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home t_Home = new Home();
            t_Home.Show();
            this.Hide();
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            PesquisaEquipes P_equipe = new PesquisaEquipes();
            P_equipe.Show();
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

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_Integrantes = new Integrantes_Equipe();
            t_Integrantes.Show();
            this.Hide();
        }

        private void lblPlanejamento_Click(object sender, EventArgs e)
        {
            Planejamento p_plano = new Planejamento();
            p_plano.Show();
            this.Hide();
        }

        private void lblGeral_Click(object sender, EventArgs e) {
            Chat_geral_equipes chatEquipe = new Chat_geral_equipes();
            chatEquipe.Show();
            this.Hide();
        }
        private void lblRanking_Click(object sender, EventArgs e) {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }
        private void lblTarefas_Click(object sender, EventArgs e) {
            Tarefas_Pendentes trf_pendente = new Tarefas_Pendentes();
            trf_pendente.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;

            if (funcionario != null)
            {
                Configuracoes config = new Configuracoes(funcionario);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum funcionário logado.");
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }

        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void btnEstatisticas_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas e_nota = new Equipes_Estatisticas();
            e_nota.Show();
            this.Hide();
        }
    }
}

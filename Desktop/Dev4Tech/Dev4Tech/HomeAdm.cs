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
    public partial class HomeAdm : Form
    {
        public HomeAdm()
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

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            Configuracoes config = new Configuracoes();
            config.Show();
            this.Hide();
        }

        private void btnCriarTarefa_Click(object sender, EventArgs e)
        {
            CriarTarefas criarTarefa = new CriarTarefas();
            criarTarefa.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeAdm t_Home = new HomeAdm();
            t_Home.Show();
            this.Hide();
        }

        private void btnPlanejamento_Click(object sender, EventArgs e)
        {
            Planejamento planejamento = new Planejamento();
            planejamento.Show();
            this.Hide();
        }

        private void btnGerenciarEquipes_Click(object sender, EventArgs e)
        {
            // TODO: Implementar gerenciamento de equipes
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnGerenciarUsuarios_Click(object sender, EventArgs e)
        {
            // TODO: Implementar gerenciamento de usuários
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

        private void btnConfigurações_Click(object sender, EventArgs e) { }
    }
}

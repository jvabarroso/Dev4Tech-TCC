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


        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeAdm t_Home = new HomeAdm();
            t_Home.Show();
            this.Hide();
        }


        private void btnConfigurações_Click(object sender, EventArgs e)
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

        private void btnEntrarEquipes_Click(object sender, EventArgs e)
        {
            AdicionarEquipes addE = new AdicionarEquipes();
            addE.Show();
            this.Hide();
        }

        private void btnEntrarTarefas_Click(object sender, EventArgs e)
        {
            AdicionarTarefa addT = new AdicionarTarefa();
            addT.Show();
            this.Hide();
        }

        private void btnEntrarCadastroFuncionario_Click(object sender, EventArgs e)
        {
            cadastro_funcionário cadFunc = new cadastro_funcionário();
            cadFunc.Show();
            this.Hide();
        }

        private void btnEntrarRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_e = new Ranking_Equipes();
            rank_e.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            AvaliaçãoTarefaAdmin t_completadas = new AvaliaçãoTarefaAdmin();
            t_completadas.Show();
            this.Hide();
        }
    }
}

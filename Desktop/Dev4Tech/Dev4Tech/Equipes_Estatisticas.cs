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
    public partial class Equipes_Estatisticas : Form
    {
        public Equipes_Estatisticas()
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

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_Integrantes = new Integrantes_Equipe();
            t_Integrantes.Show();
            this.Hide();
        }

        private void btnAdicionarEquipe_Click(object sender, EventArgs e)
        {
            AdicionarEquipes t_Adicionar = new AdicionarEquipes();
            t_Adicionar.Show();
            this.Hide();
        }

        private void btnPesquisarEquipe_Click(object sender, EventArgs e)
        {
            PesquisaEquipes t_Pesquisa = new PesquisaEquipes();
            t_Pesquisa.Show();
            this.Hide();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Equipes_Estatisticas_Load(object sender, EventArgs e)
        {
            //Limpa o gráfico 
            foreach (var Series in chart1.Series) { Series.Points.Clear(); }


            //Limpa o gráfico 
            foreach (var Series in chart1.Series) { Series.Points.Clear(); }

            chart1.Series["Contribuições"].Points.AddXY("Tarefas não entregues", 15);
            chart1.Series["Contribuições"].Points.AddXY("Tarefas Atrasadas", 3);
            chart1.Series["Contribuições"].Points.AddXY("Alerta de Problemas", 38);
            chart1.Series["Contribuições"].Points.AddXY("Tarefas Postadas", 46);

            chart2.Series["Desempenho"].Points.AddXY("Pontos Perdidos", 20);
            chart2.Series["Desempenho"].Points.AddXY("Pontos Ganhos", 80);

            chart3.Series["Entrega"].Points.AddXY("Meta", 5);
            chart3.Series["Entrega"].Points.AddXY("Pontos Perdidos", 15);
            chart3.Series["Entrega"].Points.AddXY("Pontos Obtidos", 80);
        }
    }
}

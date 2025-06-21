using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public partial class Configuracoes : Form
    {

        private empresaCadFuncionario funcionario;

        public Configuracoes(string email, string senha)
        {
            InitializeComponent();
            empresaCadFuncionario empresa = new empresaCadFuncionario();
            funcionario = empresa.ObterFuncionarioPorEmailSenha(email, senha);

            if (funcionario != null)
            {
                PreencherCampos();
            }
            else
            {
                MessageBox.Show("Funcionário não encontrado.");
                this.Close();
            }
        }

        private void CarregarFuncionario(string email, string senha)
        {
            empresaCadFuncionario empresa = new empresaCadFuncionario();
            funcionario = empresa.ObterFuncionarioPorEmailSenha(email, senha);

            if (funcionario != null)
            {
                PreencherCampos();
            }
            else
            {
                MessageBox.Show("Funcionário não encontrado ou dados incorretos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void PreencherCampos()
        {
            txtNome.Text = funcionario.getNome();
            lblCargo.Text = funcionario.getCargo();
            txtCPF.Text = funcionario.getCPF();
            txtDataNascFunc.Text = funcionario.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = funcionario.getTelefone();
            txtEmail.Text = funcionario.getEmail();
            textBox1.Text = $"{funcionario.getEndereço()}, {funcionario.getNumero()}";
        }

        private void label8_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e) { }
        private void btnEquipes_Click(object sender, EventArgs e) { Equipes_Estatisticas t_equipe = new Equipes_Estatisticas(); t_equipe.Show(); this.Hide(); }
        private void btnHome_Click(object sender, EventArgs e) { Home t_Home = new Home(); t_Home.Show(); this.Hide(); }
        private void btnLogout_Click(object sender, EventArgs e) { Form1 t_incial = new Form1(); t_incial.Show(); this.Hide(); }
        private void btnRanking_Click(object sender, EventArgs e) { Ranking_Equipes rank_equipe = new Ranking_Equipes(); rank_equipe.Show(); this.Hide(); }
        private void picPerfilMembro_Click(object sender, EventArgs e) { Integrantes_Equipe t_integrantes = new Integrantes_Equipe(); t_integrantes.Show(); this.Hide(); }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }

        private void txtDataNascFunc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtTelefone_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelEquipes_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

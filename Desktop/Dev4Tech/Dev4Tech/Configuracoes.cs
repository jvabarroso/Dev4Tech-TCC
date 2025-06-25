using System;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Configuracoes : Form
    {
        private empresaCadFuncionario funcionario;

        public Configuracoes(empresaCadFuncionario func)
        {
            InitializeComponent();
            funcionario = func;
            PreencherCampos();
        }

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

        private Configuracoes()
        {
            InitializeComponent();
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
            textBox1.Text = $"{funcionario.getEndereco()}, {funcionario.getNumero()}";

            // Obter e mostrar a pontuação atual do funcionário
            pontuacaoFuncionario ptFunc = new pontuacaoFuncionario();
            int idFunc = int.Parse(funcionario.getFuncionarioId());
            int pontos = ptFunc.ObterPontos(idFunc);
            lblPontos.Text = $"{pontos}";
        }

        private void label8_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) {
            Home h = new Home();
            h.Show();
            this.Hide();
        }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e) {

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
        private void btnEquipes_Click(object sender, EventArgs e)
        {
            PesquisaEquipes p_equipe = new PesquisaEquipes();
            p_equipe.Show();
            this.Hide();
        }
        private void btnHome_Click(object sender, EventArgs e) { Home t_Home = new Home(); t_Home.Show(); this.Hide(); }
        private void btnLogout_Click(object sender, EventArgs e) { Form1 t_incial = new Form1(); t_incial.Show(); this.Hide(); }
        private void btnRanking_Click(object sender, EventArgs e) { Ranking_Equipes rank_equipe = new Ranking_Equipes(); rank_equipe.Show(); this.Hide(); }
        private void picPerfilMembro_Click(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtDataNascFunc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtTelefone_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }

        private void panelDados_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}

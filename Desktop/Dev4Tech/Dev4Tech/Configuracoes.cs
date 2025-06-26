using System;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Configuracoes : Form
    {
        private empresaCadFuncionario funcionario;
        private empresaCadAdmin admin;

        // Construtor para funcionário
        public Configuracoes(empresaCadFuncionario func)
        {
            InitializeComponent();
            funcionario = func;
            admin = null;
            PreencherCamposFuncionario();
        }

        // Construtor para administrador
        public Configuracoes(empresaCadAdmin adm)
        {
            InitializeComponent();
            admin = adm;
            funcionario = null;
            PreencherCamposAdmin();
        }

        private void PreencherCamposFuncionario()
        {
            lblNomeFunc.Text = funcionario.getNome();
            lblCargo.Text = funcionario.getCargo();
            txtNome.Text = funcionario.getNome();
            lblCargo.Text = funcionario.getCargo();
            txtCPF.Text = funcionario.getCPF();
            txtDataNascFunc.Text = funcionario.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = funcionario.getTelefone();
            txtEmail.Text = funcionario.getEmail();
            textBox1.Text = $"{funcionario.getEndereco()}, {funcionario.getNumero()}";

            pontuacaoUsuarios ptFunc = new pontuacaoUsuarios();
            int idFunc = int.Parse(funcionario.getFuncionarioId());
            int pontos = ptFunc.ObterPontos(idFunc);
            lblPontos.Text = $"{pontos}";
        }

        private void PreencherCamposAdmin()
        {
            lblNomeFunc.Text = admin.getNome();
            lblCargo.Text = admin.getCargo();
            lblNomeFunc.Text = admin.getNome();
            lblCargo.Text = admin.getCargo();
            txtNome.Text = admin.getNome();
            lblCargo.Text = admin.getCargo();
            txtCPF.Text = admin.getCPF();
            txtDataNascFunc.Text = admin.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = admin.getTelefone();
            txtEmail.Text = admin.getEmail();
            textBox1.Text = $"{admin.getEndereco()}, {admin.getNum()}";

            // Para administrador, se quiser mostrar pontos, pode implementar similarmente
            lblPontos.Text = "Administrador";
        }

        // Os demais métodos e eventos permanecem os mesmos, sem alterações
        private void label8_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                Home t_equipe = new Home();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                HomeAdm t_equipeAdmin = new HomeAdm();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e) { }
        private void btnEquipes_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                PesquisaEquipes t_equipe = new PesquisaEquipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                Home t_equipe = new Home();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                HomeAdm t_equipeAdmin = new HomeAdm();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnLogout_Click(object sender, EventArgs e) { Form1 t_incial = new Form1(); t_incial.Show(); this.Hide(); }
        private void btnRanking_Click(object sender, EventArgs e) {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
         }
        private void picPerfilMembro_Click(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtDataNascFunc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtTelefone_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }

        private void Configuracoes_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                Tarefas_Pendentes t_equipe = new Tarefas_Pendentes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                AdicionarTarefa t_equipeAdmin = new AdicionarTarefa();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
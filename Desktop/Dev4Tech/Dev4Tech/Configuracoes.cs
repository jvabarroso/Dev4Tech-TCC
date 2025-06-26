using System;
using System.Windows.Forms;
using System.Drawing; // Adicionar se ainda não tiver para FontStyle.Bold

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
            admin = null; // Garante que admin é nulo
            PreencherCamposFuncionario();
        }

        // Construtor para administrador
        public Configuracoes(empresaCadAdmin adm)
        {
            InitializeComponent();
            admin = adm;
            funcionario = null; // Garante que funcionario é nulo
            PreencherCamposAdmin();
        }


        private void PreencherCamposFuncionario()
        {
            lblNomeFunc.Text = funcionario.getNome();
            lblCargo.Text = funcionario.getNome();
            txtNome.Text = funcionario.getNome(); // Onde o nome do funcionário será exibido
            lblCargo.Text = funcionario.getCargo();
            txtCPF.Text = funcionario.getCPF();
            txtDataNascFunc.Text = funcionario.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = funcionario.getTelefone();
            txtEmail.Text = funcionario.getEmail();
            textBox1.Text = $"{funcionario.getEndereco()}, {funcionario.getNumero()}";

            // Obter e mostrar a pontuação atual do funcionário
            pontuacaoUsuarios ptFunc = new pontuacaoUsuarios(); // Certifique-se que o namespace está correto para pontuacaoUsuarios
            int idFunc = int.Parse(funcionario.getFuncionarioId());
            int pontos = ptFunc.ObterPontos(idFunc);
            lblPontos.Text = $"{pontos}";
        }

        private void PreencherCamposAdmin()
        {
           
            lblNomeFunc.Text = admin.getNome();
            lblCargo.Text = admin.getNome();
            txtNome.Text = admin.getNome(); // Onde o nome do administrador será exibido
            lblCargo.Text = admin.getCargo();
            txtCPF.Text = admin.getCPF();
            txtDataNascFunc.Text = admin.getDataNascimento().ToString("dd/MM/yyyy");
            txtTelefone.Text = admin.getTelefone();
            txtEmail.Text = admin.getEmail();
            textBox1.Text = $"{admin.getEndereco()}, {admin.getNum()}";

            // Para administrador, pode exibir um texto diferente ou 0 pontos
            lblPontos.Text = "Administrador"; // Ou "0" ou "N/A"
        }

        // Os demais métodos e eventos permanecem os mesmos, sem alterações
        private void label8_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e)
        {
            // Exemplo de como navegar para a Home
            Home h = new Home();
            h.Show();
            this.Hide();
        }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }

        // Corrigido para verificar FuncionarioLogado e AdminLogado
        private void btnConfigurações_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Configuracoes config = new Configuracoes(funcionario);
                config.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Configuracoes config = new Configuracoes(admin);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
            }
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            PesquisaEquipes p_equipe = new PesquisaEquipes();
            p_equipe.Show();
            this.Hide();
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Limpa a sessão antes de voltar para a tela inicial
                Sessao.FuncionarioLogado = null;
                Sessao.AdminLogado = null;

                Form1 t_incial = new Form1();
                t_incial.Show();
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
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpa a sessão antes de voltar para a tela inicial
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;

            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }
        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }
        private void picPerfilMembro_Click(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtDataNascFunc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtTelefone_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void txtCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }

        private void Configuracoes_Load(object sender, EventArgs e) { } // Mantido se associado no Designer
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }
    }
}
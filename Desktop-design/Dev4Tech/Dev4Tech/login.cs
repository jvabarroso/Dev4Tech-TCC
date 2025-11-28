using System;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void lblCadastrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Sessao.AdminLogado == null)
            {
                MessageBox.Show("Apenas administradores podem cadastrar funcionários. Faça login como administrador primeiro.");
                return;
            }

            cadastro_funcionário cadastroFunc = new cadastro_funcionário(
                Sessao.AdminLogado.getAdminId(),
                Sessao.AdminLogado.getIdEmpresa());
            cadastroFunc.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha o Email e a senha para efetuar o Login");
                return;
            }

            LoginVerify lv = new LoginVerify();

            // Primeiro tenta como funcionário
            bool loginValidoFuncionario = lv.ValidarLoginFuncionario(email, senha);

            if (loginValidoFuncionario)
            {
                empresaCadFuncionario empresaFunc = new empresaCadFuncionario();
                var funcionario = empresaFunc.ObterFuncionarioPorEmail(email);

                if (funcionario != null)
                {
                    Sessao.FuncionarioLogado = funcionario;
                    Sessao.AdminLogado = null;

                    Home hm = new Home();
                    hm.Show();
                    this.Hide();
                    return;
                }
            }
            else
            {
                // Tenta login como administrador
                bool loginValidoAdmin = lv.ValidarLoginAdministrador(email, senha);

                if (loginValidoAdmin)
                {
                    empresaCadAdmin adminDAO = new empresaCadAdmin();
                    var adminLogado = adminDAO.ObterAdminPorEmail(email);

                    if (adminLogado != null)
                    {
                        Sessao.AdminLogado = adminLogado;
                        Sessao.FuncionarioLogado = null;

                        HomeAdm hmAdm = new HomeAdm();
                        hmAdm.Show();
                        this.Hide();
                        return;
                    }
                }
            }

            // Se chegou aqui, login falhou
            MessageBox.Show("Email ou senha incorretos.");
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Entre com seu endereço de Email")
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Entre com seu endereço de Email";
                txtEmail.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e) { }

        private void txtSenha_TextChanged(object sender, EventArgs e) { }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "Digite sua senha")
            {
                txtSenha.Text = "";
                txtSenha.UseSystemPasswordChar = true;
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.Text = "Digite sua senha";
                txtSenha.UseSystemPasswordChar = false;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private bool senhaVisivel = false;
        private void btnMostrarSenha_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;
            txtSenha.UseSystemPasswordChar = !senhaVisivel;
        }

        private void txtEmail_TextChanged_1(object sender, EventArgs e) { }

        private void lblCadastrar_Click(object sender, EventArgs e)
        {
            Cadastro_empresa cad_empresa = new Cadastro_empresa();
            cad_empresa.Show();
            this.Hide();
        }
    }
}
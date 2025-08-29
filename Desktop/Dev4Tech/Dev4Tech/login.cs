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
            // Supondo que "adminLogado" é o objeto que você tem após login
            Sessao.AdminLogado.getAdminId();
            Sessao.AdminLogado.getIdEmpresa();

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
            bool loginValidoFuncionario = lv.ValidarLogin(email, senha);

            if (loginValidoFuncionario)
            {
                empresaCadFuncionario empresa = new empresaCadFuncionario();
                var funcionario = empresa.ObterFuncionarioPorEmailSenha(email, senha);

                if (funcionario != null)
                {
                    Sessao.FuncionarioLogado = funcionario;
                    Sessao.AdminLogado = null;

                    Home hm = new Home();
                    hm.Show();
                    this.Hide();
                    return;
                }
                else
                {
                    MessageBox.Show("Erro ao carregar dados do funcionário.");
                    return;
                }
            }
            else
            {
                // Tenta login como administrador
                empresaCadAdmin admin = new empresaCadAdmin();
                var adminLogado = admin.ObterAdminPorEmailSenha(email, senha);

                if (adminLogado != null)
                {
                    Sessao.AdminLogado = adminLogado;
                    Sessao.FuncionarioLogado = null;

                    // Abre a tela de configurações passando o admin logado
                    HomeAdm hmAdm = new HomeAdm();
                    hmAdm.Show();
                    this.Hide();
                    return;
                }
                else
                {
                    MessageBox.Show("Email ou senha incorretos.");
                    return;
                }
            }
        }

        //Texto dinâmico na txtEmail
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if(txtEmail.Text == "Entre com seu endereço de Email")
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
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
        }

        //Texto dinâmico na txtSenha
        private void txtSenha_TextChanged(object sender, EventArgs e) { }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "Digite sua senha")
            {
                txtSenha.Text = "";
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.Text = "Digite sua senha";
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
    }
}


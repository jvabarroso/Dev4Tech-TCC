using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
            cadastro_funcionário cad_funcionario = new cadastro_funcionário();
            cad_funcionario.Show();
            this.Hide();
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
            bool loginValido = lv.ValidarLogin(email, senha);

            if (loginValido)
            {
                empresaCadFuncionario empresa = new empresaCadFuncionario();
                var funcionario = empresa.ObterFuncionarioPorEmailSenha(email, senha);

                if (funcionario != null)
                {
                    Configuracoes config = new Configuracoes(email, senha);
                    config.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Erro ao carregar dados do funcionário.");
                }
            }
            else
            {
                MessageBox.Show("Email ou senha incorretos.");
            }
        }


        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSenha_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

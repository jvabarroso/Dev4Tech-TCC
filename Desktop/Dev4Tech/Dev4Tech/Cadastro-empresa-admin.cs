using System;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Cadastro_empresa_admin : Form
    {
        empresaCadAdmin emAdmin = new empresaCadAdmin();
        private readonly string idEmpresa;

        // Construtor que recebe o idEmpresa para vincular no cadastro
        public Cadastro_empresa_admin(string idEmpresa)
        {
            InitializeComponent();
            this.idEmpresa = idEmpresa;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCadAdmSenha.Text != txtCadAdmConfirmSenha.Text)
                {
                    MessageBox.Show("Erro! As duas senhas estão diferentes");
                    return;
                }

                emAdmin.setNome(txtCadAdmNome.Text);
                emAdmin.setCargo(cbBoxCargoAdm.Text);
                emAdmin.setCPF(txtCadAdmCPF.Text);

                DateTime dataNascimento;
                if (!DateTime.TryParse(txtCadAdmDataNasc.Text, out dataNascimento))
                {
                    MessageBox.Show("Data de nascimento inválida. Por favor, insira uma data válida.");
                    return;
                }
                emAdmin.setDataNascimento(dataNascimento);

                emAdmin.setTelefone(txtCadAdmTelefone.Text);
                emAdmin.setEmail(txtCadAdmEmail.Text);
                emAdmin.setSenha(txtCadAdmSenha.Text);
                emAdmin.setData_cadAdmin(DateTime.Now);
                emAdmin.setEndereco(txtEndereco.Text);
                emAdmin.setNum(txtNumEndereco.Text);

                // ASSOCIA o idEmpresa recebido ao cadastro (essencial para a FK)
                emAdmin.setIdEmpresa(idEmpresa);

                emAdmin.inserir();

                MessageBox.Show("Cadastro de administrador realizado com sucesso!");
                Login t_login = new Login();
                t_login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void txtCadAdmNome_Click(object sender, EventArgs e)
        {
            txtCadAdmNome.Text = "";
        }

        private void txtCadAdmCPF_Click(object sender, EventArgs e)
        {
            txtCadAdmCPF.Text = "";
        }

        private void txtCadAdmEmail_Click(object sender, EventArgs e)
        {
            txtCadAdmEmail.Text = "";
        }

        private void txtCadAdmSenha_Click(object sender, EventArgs e)
        {
            txtCadAdmSenha.Text = "";
        }

        private void txtCadAdmConfirmSenha_Click(object sender, EventArgs e)
        {
            txtCadAdmConfirmSenha.Text = "";
        }

        private void txtCadAdmDataNasc_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }

        private void lblLoginAdm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }

        private void txtCadAdmNome_Enter(object sender, EventArgs e)
        {
            if (txtCadAdmNome.Text == "Nome Completo")
            {
                txtCadAdmNome.Text = "";
            }
        }

        private void txtCadAdmNome_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadAdmNome.Text))
            {
                txtCadAdmNome.Text = "Nome Completo";
            }
        }

        private void txtCadAdmEmail_Enter(object sender, EventArgs e)
        {
            if (txtCadAdmEmail.Text == "Email")
            {
                txtCadAdmEmail.Text = "";
            }
        }

        private void txtCadAdmEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadAdmEmail.Text))
            {
                txtCadAdmEmail.Text = "Email";
            }
        }

        private void txtCadAdmSenha_Enter(object sender, EventArgs e)
        {
            if (txtCadAdmSenha.Text == "Senha")
            {
                txtCadAdmSenha.Text = "";
                txtCadAdmSenha.UseSystemPasswordChar = true; // Oculta os caracteres
            }
        }

        private void txtCadAdmSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadAdmSenha.Text))
            {
                txtCadAdmSenha.UseSystemPasswordChar = false; // Mostra os caracteres
                txtCadAdmSenha.Text = "Senha";
            }
        }

        private void txtCadAdmConfrmSenha_Enter(object sender, EventArgs e)
        {
            if (txtCadAdmConfirmSenha.Text == "Confirmar Senha")
            {
                txtCadAdmConfirmSenha.Text = "";
                txtCadAdmConfirmSenha.UseSystemPasswordChar = true; // Oculta os caracteres
            }
        }

        private void txtCadAdmConfirmSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadAdmConfirmSenha.Text))
            {
                txtCadAdmConfirmSenha.UseSystemPasswordChar = false; // Mostra os caracteres
                txtCadAdmConfirmSenha.Text = "Confirmar Senha";
            }
        }

        private void txtEndereco_Enter(object sender, EventArgs e)
        {
            if (txtEndereco.Text == "Endereço")
            {
                txtEndereco.Text = "";
            }
        }

        private void txtEndereco_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEndereco.Text))
            {
                txtEndereco.Text = "Endereço";
            }
        }

        private void txtNumEndereco_Enter(object sender, EventArgs e)
        {
            if (txtNumEndereco.Text == "Nº")
            {
                txtNumEndereco.Text = "";
            }
        }

        private void txtNumEndereco_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumEndereco.Text))
            {
                txtNumEndereco.Text = "Nº";
            }
        }

        private bool senhaVisivel = false;
        private void btnMostrarSenha_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;
            txtCadAdmSenha.UseSystemPasswordChar = !senhaVisivel;
            txtCadAdmConfirmSenha.UseSystemPasswordChar = !senhaVisivel;
            btnMostrarSenha.Text = senhaVisivel ? "Ocultar Senha" : "Mostrar Senha";
        }
    }
}
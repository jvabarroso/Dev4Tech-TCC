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
    public partial class Cadastro_empresa_admin : Form
    {
        empresaCadAdmin emAdmin = new empresaCadAdmin();

        public Cadastro_empresa_admin()
        {
            InitializeComponent();
        }

        private void lblLoginAdm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCadAdmSenha.Text != txtCadAdmConfirmSenha.Text)
                {
                    MessageBox.Show("Erro! As duas senhas estão diferentes");
                }
                else
                {
                    emAdmin.setNome(txtCadAdmNome.Text);
                    emAdmin.setCargo(cbBoxCargoAdm.Text);
                    emAdmin.setCPF(txtCadAdmCPF.Text);
                    emAdmin.setDataNascimento(DateTime.Today);
                    emAdmin.setTelefone(txtCadAdmTelefone.Text);
                    emAdmin.setEmail(txtCadAdmEmail.Text);
                    emAdmin.setSenha(txtCadAdmSenha.Text);
                    emAdmin.setData_cadAdmin(DateTime.Now);
                    emAdmin.inserir();
                }
                
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
    }
}

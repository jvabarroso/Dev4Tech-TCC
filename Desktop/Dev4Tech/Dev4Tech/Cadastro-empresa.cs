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
    public partial class Cadastro_empresa : Form
    {
        public Cadastro_empresa()
        {
            InitializeComponent();
        }

        empresa em = new empresa();

        private void lblLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Empresa cadastrada com sucesso");
            MessageBox.Show("Agora, faça seu Login!");
            Cadastro_empresa_admin cad_admin = new Cadastro_empresa_admin();
            cad_admin.Show();
            this.Hide();
            //em.setNomeEmpresa(txtNomeEmpresa.Text);
            //em.inserir();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide(); 
        }

        private void txtNomeEmpresa_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNomeEmpresa_Click(object sender, EventArgs e)
        {
            txtNomeEmpresa.Text = "";
        }

        private void txtCNPJEmpresa_Click(object sender, EventArgs e)
        {
            txtCNPJEmpresa.Text = "";
        }

        private void txtSetorEmpresa_Click(object sender, EventArgs e)
        {
            txtSetorEmpresa.Text = "";
        }

        private void txtLogradouroEmpresa_Click(object sender, EventArgs e)
        {
            txtLogradouroEmpresa.Text = "";
        }

        private void txtNumResidencia_Click(object sender, EventArgs e)
        {
            txtNumResidencia.Text = "";
        }

        private void txtBairroEmpresa_Click(object sender, EventArgs e)
        {
            txtBairroEmpresa.Text = "";
        }

        private void txtComplementoEmpresa_Click(object sender, EventArgs e)
        {
            txtComplementoEmpresa.Text = "";
        }
    }
}

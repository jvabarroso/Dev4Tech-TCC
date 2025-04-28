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
            login t_login = new login();
            t_login.Show();
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Empresa cadastrada com sucesso");
           // Home t_Home = new Home();
           // t_Home.Show();
           // this.Hide();
            em.setNomeEmpresa(txtNomeEmpresa.Text);
            em.inserir();
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
    }
}

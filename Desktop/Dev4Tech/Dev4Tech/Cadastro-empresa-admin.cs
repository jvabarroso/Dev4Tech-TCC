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
        public Cadastro_empresa_admin()
        {
            InitializeComponent();
        }

        private void lblLoginAdm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cadastro_empresa_admin cad_EmpresaAdm = new Cadastro_empresa_admin();
            cad_EmpresaAdm.Show();
            this.Hide();

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cadastro de administrador realizado com sucesso!");
            Home t_Home = new Home();
            t_Home.Show();
            this.Hide();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }
    }
}

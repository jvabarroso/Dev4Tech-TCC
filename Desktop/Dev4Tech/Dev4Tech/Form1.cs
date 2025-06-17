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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }

        private void btnCadastroEmpresa_Click(object sender, EventArgs e)
        {
            Cadastro_empresa cad_empresa = new Cadastro_empresa();
            cad_empresa.Show();
            this.Hide();
        }

        private void btnCadastroFuncionario_Click(object sender, EventArgs e)
        {
            cadastro_funcionário cad_funcionario = new cadastro_funcionário();
            cad_funcionario.Show();
            this.Hide();
        }

        private void btnCadastroEmpresaAdmin_Click(object sender, EventArgs e)
        {
            Cadastro_empresa_admin cad_empresa_admin = new Cadastro_empresa_admin();
            cad_empresa_admin.Show();
            this.Hide();
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            cadastro_funcionário cadFunc = new cadastro_funcionário();
            cadFunc.Show();
            this.Hide();
        }
    }
}

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
    public partial class cadastro_funcionário : Form
    {
        empresaCadFuncionario emCadFunc = new empresaCadFuncionario();

        public cadastro_funcionário()
        {
            InitializeComponent();
        }

        private void lblLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                emCadFunc.setNome(txtCadFuncNome.Text);
                emCadFunc.setCargo(cbBoxCargoFunc.Text);
                emCadFunc.setCPF(txtCadFuncCPF.Text);
                emCadFunc.setEmail(txtCadFuncEmail.Text);
                emCadFunc.setTelefone(txtCadFuncTelefone.Text);
                emCadFunc.setSenha(txtCadFuncSenha.Text);
                emCadFunc.setDataNascimento(DateTime.Today);
                emCadFunc.setData_cadFunc(DateTime.Now);
                emCadFunc.setEndereco(txtEndereço.Text);
                emCadFunc.setNumero(txtEndereçoNum.Text);
                emCadFunc.inserir();
                
                MessageBox.Show("Sua conta foi cadastrada com sucesso");
                Home t_Home = new Home();
                t_Home.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void txtCadFuncNome_Click(object sender, EventArgs e)
        {
            txtCadFuncNome.Text = "";
        }

        private void txtCadFuncEmail_Click(object sender, EventArgs e)
        {
            txtCadFuncEmail.Text = "";
        }

        private void txtCadFuncSenha_Click(object sender, EventArgs e)
        {
            txtCadFuncSenha.Text = "";
        }

        private void txtCadFuncConfirmSenha_Click(object sender, EventArgs e)
        {
            txtCadFuncConfirmSenha.Text = "";
        }

        private void cbBoxCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cadastro_funcionário_Load(object sender, EventArgs e)
        {

        }

        private void txtCadFuncNome_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtCadFuncEmail_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtCadFuncSenha_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void txtCadFuncConfirmSenha_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtEndereço_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtEndereçoNum_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}

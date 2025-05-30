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
        empresa em = new empresa();

        public Cadastro_empresa()
        {
            InitializeComponent();
        }

        // ============== MÉTODOS DE PLACEHOLDER ADICIONADOS ==============

        private void ConfigurarPlaceholder(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;

            txt.Enter += (sender, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
            txt.TextChanged += (sender, e) =>
            {
                if (txt.ForeColor == Color.Gray && txt.Text != placeholder)
                    txt.ForeColor = Color.Black;
            };
        }
        // ============== FIM DOS MÉTODOS DE PLACEHOLDER ==============

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
                if (txtNomeEmpresa.Text == "Nome da Empresa" || string.IsNullOrWhiteSpace(txtNomeEmpresa.Text))
                {
                    MessageBox.Show("Preencha o nome da empresa!");
                    return;
                }


                
                em.setNomeEmpresa(txtNomeEmpresa.Text == "Nome da Empresa" ? "" : txtNomeEmpresa.Text);
                em.setCNPJ(txtCNPJEmpresa.Text);
                em.setSetorEmpresarial(txtSetorEmpresa.Text);
                em.setLogradouro(txtLogradouroEmpresa.Text);
                em.setNumResidencia(txtNumResidencia.Text);
                em.setBairro(txtBairroEmpresa.Text);
                em.setComplemento(txtComplementoEmpresa.Text);

                em.inserir();
                MessageBox.Show("Cadastro realizado com sucesso!");

                Cadastro_empresa_admin cad_adm = new Cadastro_empresa_admin();
                cad_adm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        // ============== MÉTODOS ORIGINAIS (MANTIDOS SEM ALTERAÇÃO) ==============
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void txtNomeEmpresa_TextChanged(object sender, EventArgs e) { }

        private void txtNomeEmpresa_Click(object sender, EventArgs e)
        {
            txtNomeEmpresa.Text = "";
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

        private void txtNumResidencia_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

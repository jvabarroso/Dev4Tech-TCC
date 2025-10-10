using System;
using System.Windows.Forms;
using Dev4Tech.Utils; // importa a classe InputMask

namespace Dev4Tech
{
    public partial class Cadastro_empresa : Form
    {
        empresa em = new empresa();

        public Cadastro_empresa()
        {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                em.setNomeEmpresa(txtNomeEmpresa.Text);
                em.setCNPJ(txtCNPJEmpresa.Text);
                em.setSetorEmpresarial(txtSetorEmpresa.Text);
                em.setLogradouro(txtLogradouroEmpresa.Text);
                em.setNumResidencia(txtNumResidencia.Text);
                em.setBairro(txtBairroEmpresa.Text);
                em.setComplemento(txtComplementoEmpresa.Text);
                em.setData_cadEm(DateTime.Now);
                em.setEmail(txtEmailEmpresa.Text);
                em.setTelefone(txtTelefoneEmpresa.Text);

                // Usar o método que insere e retorna o ID gerado
                int idEmpresaGerada = em.inserirEObterId();

                if (idEmpresaGerada > 0)
                {
                    MessageBox.Show("Empresa cadastrada com sucesso!");
                    // Passar o ID da empresa para o cadastro do administrador
                    Cadastro_empresa_admin cadastroAdmin = new Cadastro_empresa_admin(idEmpresaGerada.ToString());
                    cadastroAdmin.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Erro ao cadastrar empresa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void lblLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }

        private void Cadastro_empresa_Load(object sender, EventArgs e)
        {
            // Aplicar máscaras reutilizáveis da classe InputMask
            InputMask.MaskCNPJ(txtCNPJEmpresa);
            InputMask.MaskTelefone(txtTelefoneEmpresa);
        }

        // --- PLACEHOLDERS DINÂMICOS ---
        private void txtNomeEmpresa_Enter(object sender, EventArgs e)
        {
            if (txtNomeEmpresa.Text == "Digite o nome da sua instituição")
                txtNomeEmpresa.Text = "";
        }

        private void txtNomeEmpresa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeEmpresa.Text))
            {
                txtNomeEmpresa.Text = "Digite o nome da sua instituição";
                txtNomeEmpresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtSetorEmpresa_Enter(object sender, EventArgs e)
        {
            if (txtSetorEmpresa.Text == "Digite o setor da sua instituição")
                txtSetorEmpresa.Text = "";
        }

        private void txtSetorEmpresa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSetorEmpresa.Text))
            {
                txtSetorEmpresa.Text = "Digite o setor da sua instituição";
                txtSetorEmpresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtLogradouroEmpresa_Enter(object sender, EventArgs e)
        {
            if (txtLogradouroEmpresa.Text == "Digite o logradouro da sua instituição")
                txtLogradouroEmpresa.Text = "";
        }

        private void txtLogradouroEmpresa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogradouroEmpresa.Text))
            {
                txtLogradouroEmpresa.Text = "Digite o logradouro da sua instituição";
                txtLogradouroEmpresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtNumResidencia_Enter(object sender, EventArgs e)
        {
            if (txtNumResidencia.Text == "Nº")
                txtNumResidencia.Text = "";
        }

        private void txtNumResidencia_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumResidencia.Text))
            {
                txtNumResidencia.Text = "Nº";
                txtNumResidencia.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtBairroEmpresa_Enter(object sender, EventArgs e)
        {
            if (txtBairroEmpresa.Text == "Digite o bairro da sua instituição")
                txtBairroEmpresa.Text = "";
        }

        private void txtBairroEmpresa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBairroEmpresa.Text))
            {
                txtBairroEmpresa.Text = "Digite o bairro da sua instituição";
                txtBairroEmpresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtComplementoEmpresa_Enter(object sender, EventArgs e)
        {
            if (txtComplementoEmpresa.Text == "Digite o complemento da sua instituição")
                txtComplementoEmpresa.Text = "";
        }

        private void txtComplementoEmpresa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtComplementoEmpresa.Text))
            {
                txtComplementoEmpresa.Text = "Digite o complemento da sua instituição";
                txtComplementoEmpresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtEmailEmpresa_Enter(object sender, EventArgs e)
        {
            if (txtEmailEmpresa.Text == "Digite o email da sua instituição")
                txtEmailEmpresa.Text = "";
        }

        private void txtEmailEmpresa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmailEmpresa.Text))
            {
                txtEmailEmpresa.Text = "Digite o email da sua instituição";
                txtEmailEmpresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }
    }
}

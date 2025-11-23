using System;
using System.Windows.Forms;
using Dev4Tech.Utils;

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
                // Validação básica de senha
                if (string.IsNullOrWhiteSpace(txtSenhaEmpresa.Text))
                {
                    MessageBox.Show("Por favor, digite uma senha.");
                    return;
                }

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

                // ⭐⭐ APLICA HASH NA SENHA DA EMPRESA ⭐⭐
                string senhaHash = SenhasHash.HashPassword(txtSenhaEmpresa.Text);
                em.setSenhaEmail(senhaHash);

                int idEmpresaGerada = em.inserirEObterId();

                if (idEmpresaGerada > 0)
                {
                    MessageBox.Show("Empresa cadastrada com sucesso!");
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

        private void Cadastro_empresa_Load(object sender, EventArgs e)
        {
            InputMask.MaskCNPJ(txtCNPJEmpresa);
            InputMask.MaskTelefone(txtTelefoneEmpresa);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            Login t_login = new Login();
            t_login.Show();
            this.Hide();
        }
    }
}
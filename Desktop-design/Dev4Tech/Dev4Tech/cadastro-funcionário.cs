using System;
using System.Windows.Forms;
using Dev4Tech.Utils;

namespace Dev4Tech
{
    public partial class cadastro_funcionário : Form
    {
        empresaCadFuncionario emCadFunc = new empresaCadFuncionario();

        private readonly string idAdminLogado;
        private readonly string idEmpresaAdmin;

        public cadastro_funcionário(string adminId, string empresaId)
        {
            InitializeComponent();

            this.idAdminLogado = adminId;
            this.idEmpresaAdmin = empresaId;

            InputMask.MaskCPF(txtCadFuncCPF);
            InputMask.MaskTelefone(txtCadFuncTelefone);
            InputMask.MaskData(txtCadFuncDataNasc);
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
                if (string.IsNullOrWhiteSpace(idAdminLogado) || string.IsNullOrWhiteSpace(idEmpresaAdmin))
                {
                    MessageBox.Show("Erro: IDs de administrador ou empresa não definidos. Por favor, confirme se está logado corretamente.");
                    return;
                }

                emCadFunc.setNome(txtCadFuncNome.Text);
                emCadFunc.setCargo(cbBoxCargoFunc.Text);
                emCadFunc.setCPF(txtCadFuncCPF.Text);
                emCadFunc.setEmail(txtCadFuncEmail.Text);
                emCadFunc.setTelefone(txtCadFuncTelefone.Text);
                emCadFunc.setSenha(txtCadFuncSenha.Text);

                DateTime dataNascimento;
                if (!DateTime.TryParse(txtCadFuncDataNasc.Text, out dataNascimento))
                {
                    MessageBox.Show("Data de nascimento inválida. Por favor, insira uma data válida.");
                    return;
                }
                emCadFunc.setDataNascimento(dataNascimento);

                emCadFunc.setData_cadFunc(DateTime.Now);
                emCadFunc.setEndereco(txtEndereço.Text);
                emCadFunc.setNumero(txtEndereçoNum.Text);

                emCadFunc.setIdEmpresa(idEmpresaAdmin);
                emCadFunc.setAdminId(idAdminLogado);

                emCadFunc.inserir();

                MessageBox.Show("Funcionário cadastrado com sucesso!");

                // Opcional: limpar senha confirm para evitar confusão
                txtCadFuncSenha.Text = "";
                txtCadFuncConfirmSenha.Text = "";

                HomeAdm homeAdm = new HomeAdm();
                homeAdm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar funcionário: " + ex.Message);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            HomeAdm homeAdm = new HomeAdm();
            homeAdm.Show();
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
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void cadastro_funcionário_Load(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtCadFuncNome_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtCadFuncEmail_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtCadFuncSenha_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtCadFuncConfirmSenha_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtEndereço_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtEndereçoNum_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar lógica adicional
        }

        private void txtCadFuncNome_Enter(object sender, EventArgs e)
        {
            if (txtCadFuncNome.Text == "Digite o Nome do Funcionário")
            {
                txtCadFuncNome.Text = "";
            }
        }

        private void txtCadFuncNome_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadFuncNome.Text))
            {
                txtCadFuncNome.Text = "Digite o Nome do Funcionário";
            }
        }

        private void txtCadFuncEmail_Enter(object sender, EventArgs e)
        {
            if (txtCadFuncEmail.Text == "Digite o Email do Funcionário")
            {
                txtCadFuncEmail.Text = "";
            }
        }

        private void txtCadFuncEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadFuncEmail.Text))
            {
                txtCadFuncEmail.Text = "Digite o Email do Funcionário";
            }
        }

        private void txtCadFuncSenha_Enter(object sender, EventArgs e)
        {
            if (txtCadFuncSenha.Text == "Digite a Senha")
            {
                txtCadFuncSenha.Text = "";
                txtCadFuncSenha.UseSystemPasswordChar = true;
            }
        }

        private void txtCadFuncSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadFuncSenha.Text))
            {
                txtCadFuncSenha.UseSystemPasswordChar = false;
                txtCadFuncSenha.Text = "Digite a Senha";
            }
        }

        private void txtCadFuncConfirmSenha_Enter(object sender, EventArgs e)
        {
            if (txtCadFuncConfirmSenha.Text == "Confirme a Senha")
            {
                txtCadFuncConfirmSenha.Text = "";
                txtCadFuncConfirmSenha.UseSystemPasswordChar = true;
            }
        }

        private void txtCadFuncConfirmSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadFuncConfirmSenha.Text))
            {
                txtCadFuncConfirmSenha.UseSystemPasswordChar = false;
                txtCadFuncConfirmSenha.Text = "Confirme a Senha";
            }
        }

        private void txtEndereçoNum_Enter(object sender, EventArgs e)
        {
            if (txtEndereçoNum.Text == "Nº")
            {
                txtEndereçoNum.Text = "";
            }
        }

        private void txtEndereçoNum_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEndereçoNum.Text))
            {
                txtEndereçoNum.Text = "Nº";
            }
        }

        private bool senhaVisivel = false;
        private void btnMostrarSenha_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;
            txtCadFuncSenha.UseSystemPasswordChar = !senhaVisivel;
            btnMostrarSenha.Text = senhaVisivel ? "" : "";
        }

        private void btnMostrarSenha2_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;
            txtCadFuncConfirmSenha.UseSystemPasswordChar = !senhaVisivel;
            btnMostrarSenha.Text = senhaVisivel ? "" : "";
        }
    }
}



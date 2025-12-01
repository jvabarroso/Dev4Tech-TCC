using Dev4Tech.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class cadastro_funcionário : Form
    {
        empresaCadFuncionario emCadFunc = new empresaCadFuncionario();

        private readonly string idAdminLogado;
        private readonly string idEmpresaAdmin;

        private void cadastro_funcionário_Load(object sender, EventArgs e)
        {
            CarregarCargos();
        }

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

                if (txtCadFuncSenha.Text != txtCadFuncConfirmSenha.Text)
                {
                    MessageBox.Show("As senhas não coincidem!");
                    return;
                }

                if (txtCadFuncSenha.Text.Length < 6)
                {
                    MessageBox.Show("A senha deve ter pelo menos 6 caracteres.");
                    return;
                }

                emCadFunc.setNome(txtCadFuncNome.Text);
                emCadFunc.setCargo(cbBoxCargoFunc.Text);
                emCadFunc.setCPF(txtCadFuncCPF.Text);
                emCadFunc.setEmail(txtCadFuncEmail.Text);
                emCadFunc.setTelefone(txtCadFuncTelefone.Text);

                // ⭐⭐ APLICA HASH NA SENHA DO FUNCIONÁRIO ⭐⭐
                string senhaHash = SenhasHash.HashPassword(txtCadFuncSenha.Text);
                emCadFunc.setSenha(senhaHash);

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
                emCadFunc.setCargo(cbBoxCargoFunc.Text);
                emCadFunc.setIdEmpresa(idEmpresaAdmin);
                emCadFunc.setAdminId(idAdminLogado);

                emCadFunc.inserir();

                MessageBox.Show("Funcionário cadastrado com sucesso!");

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

        private void CarregarCargos()
        {
            string connectionString = "server=localhost;database=Dev4Tech;uid=root;pwd=";
            string query = "SELECT DISTINCT Cargo FROM Funcionarios WHERE id_empresa = @idEmpresa ORDER BY Cargo";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idEmpresa", idEmpresaAdmin);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            cbBoxCargoFunc.Items.Clear();
                            while (reader.Read())
                            {
                                cbBoxCargoFunc.Items.Add(reader["Cargo"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar cargos: " + ex.Message);
                }
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            HomeAdm homeAdm = new HomeAdm();
            homeAdm.Show();
            this.Hide();
        }

        private bool senhaVisivel = false;
        private void btnMostrarSenha_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;
            txtCadFuncSenha.UseSystemPasswordChar = !senhaVisivel;
        }

        private void btnMostrarSenha2_Click(object sender, EventArgs e)
        {
            senhaVisivel = !senhaVisivel;
            txtCadFuncConfirmSenha.UseSystemPasswordChar = !senhaVisivel;
        }

        private void cbBoxCargoFunc_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
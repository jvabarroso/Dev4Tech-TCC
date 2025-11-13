// AdicionarEquipes.cs
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class AdicionarEquipes : Form
    {
        AddEquipes equipe = new AddEquipes();
        private List<string> membrosSelecionados = new List<string>();
        private List<string> funcionariosSelecionados = new List<string>();

        public AdicionarEquipes()
        {
            InitializeComponent();
            cmbCategoriaEquipe.DropDownStyle = ComboBoxStyle.DropDown;
            panelDadosFunc.AutoScroll = true; // Habilita o scroll automático
            this.Load += AdicionarEquipes_Load;
            cbmEmailMembro.SelectedIndexChanged += cbmEmailMembro_SelectedIndexChanged;
            cmbCategoriaEquipe.SelectedIndexChanged += cmbCategoriaEquipe_SelectedIndexChanged;
            txtNomeEquipe.TextChanged += txtNomeEquipe_TextChanged;
            
        }

        private void AdicionarEquipes_Load(object sender, EventArgs e)
        {
            CarregarCategorias();
            CarregarEmailsFuncionarios();
            cmbCategoriaEquipe.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void CarregarCategorias()
        {
            try
            {
                DataTable dtCategorias = equipe.ConsultarCategorias();
                cmbCategoriaEquipe.Items.Clear();
                foreach (DataRow row in dtCategorias.Rows)
                {
                    cmbCategoriaEquipe.Items.Add(row["nome_categoria"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
            }
        }

        private void CarregarEmailsFuncionarios()
        {
            try
            {
                DataTable dtEmails = equipe.ConsultarEmailsFuncionarios();
                cbmEmailMembro.Items.Clear();
                foreach (DataRow row in dtEmails.Rows)
                {
                    cbmEmailMembro.Items.Add(row["email"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar emails: " + ex.Message);
            }
        }

        private void txtNomeEquipe_TextChanged(object sender, EventArgs e)
        {
            // Opcional
        }

        private void cmbCategoriaEquipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Opcional
        }

        private void cbmEmailMembro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string emailSelecionado = cbmEmailMembro.Text.Trim();
            if (!string.IsNullOrEmpty(emailSelecionado) && !funcionariosSelecionados.Contains(emailSelecionado))
            {
                funcionariosSelecionados.Add(emailSelecionado);
                AtualizarListaFuncionarios();
            }
        }

        private void AtualizarListaFuncionarios()
        {
            panelDadosFunc.Controls.Clear();
            int posY = 10;
            string basePath = @"C:\xampp\htdocs\dev4tech\img\";

            foreach (string email in funcionariosSelecionados)
            {
                empresaCadFuncionario func = BuscarFuncionarioPorEmail(email);
                if (func == null)
                    continue;

                pontuacaoUsuarios ptFunc = new pontuacaoUsuarios();
                int idFunc = int.Parse(func.getFuncionarioId());
                int pontos = ptFunc.ObterPontos(idFunc);

                Panel funcPanel = new Panel
                {
                    Width = panelDadosFunc.Width - 20,
                    Height = 100,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Left = 10,
                    Top = posY
                };

                PictureBox picFuncionario = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 80,
                    Height = 80,
                    Left = 10,
                    Top = 10,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // CARREGAMENTO CORRETO DA FOTO DO ARQUIVO
                try
                {
                    byte[] fotoBytes = func.getFotoPerfilBytes();
                    string nomeArquivo = null;

                    if (fotoBytes != null && fotoBytes.Length > 0)
                    {
                        // Converte os bytes para string usando UTF-8
                        nomeArquivo = System.Text.Encoding.UTF8.GetString(fotoBytes);

                        // Limpa o nome do arquivo de caracteres inválidos
                        nomeArquivo = new string(nomeArquivo.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());

                        string caminhoCompleto = Path.Combine(basePath, nomeArquivo);
                        if (File.Exists(caminhoCompleto))
                        {
                            using (var imgTemp = Image.FromFile(caminhoCompleto))
                            {
                                picFuncionario.Image = new Bitmap(imgTemp);
                            }
                        }
                        else
                        {
                            picFuncionario.Image = Properties.Resources.icon_perfil;
                            Console.WriteLine($"Arquivo não encontrado: {caminhoCompleto}");
                        }
                    }
                    else
                    {
                        picFuncionario.Image = Properties.Resources.icon_perfil;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar foto: {ex.Message}");
                    picFuncionario.Image = Properties.Resources.icon_perfil;
                }

                funcPanel.Controls.Add(picFuncionario);

                Label lblNome = new Label
                {
                    Text = func.getNome(),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Left = picFuncionario.Right + 15,
                    Top = 15,
                    AutoSize = true
                };
                funcPanel.Controls.Add(lblNome);

                Label lblCargo = new Label
                {
                    Text = func.getCargo(),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Left = picFuncionario.Right + 15,
                    Top = lblNome.Bottom + 5,
                    AutoSize = true
                };
                funcPanel.Controls.Add(lblCargo);

                Label lblPontos = new Label
                {
                    Text = $"Pontos: {pontos}",
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Left = picFuncionario.Right + 15,
                    Top = lblCargo.Bottom + 5,
                    AutoSize = true
                };
                funcPanel.Controls.Add(lblPontos);

                panelDadosFunc.Controls.Add(funcPanel);
                posY += funcPanel.Height + 10;
            }
        }
        private empresaCadFuncionario BuscarFuncionarioPorEmail(string email)
        {
            empresaCadFuncionario func = new empresaCadFuncionario();
            string query = "SELECT * FROM Funcionarios WHERE Email = @Email LIMIT 1";
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd=;"))
            {
                conn.Open();
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        func.setFuncionarioId(reader["FuncionarioId"].ToString());
                        func.setNome(reader["Nome"].ToString());
                        func.setCargo(reader["Cargo"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("foto_perfil")))
                        {
                            byte[] fotoBytes = (byte[])reader["foto_perfil"];
                            func.setFotoPerfilBytes(fotoBytes);
                        }
                    }
                    else
                    {
                        func = null;
                    }
                }
            }
            return func;
        }

        private void btnAddMembro_Click(object sender, EventArgs e)
        {
            string emailSelecionado = cbmEmailMembro.Text.Trim();
            if (!string.IsNullOrEmpty(emailSelecionado) && !membrosSelecionados.Contains(emailSelecionado))
            {
                membrosSelecionados.Add(emailSelecionado);
                MessageBox.Show($"Membro {emailSelecionado} adicionado.");
            }
            else
            {
                MessageBox.Show("Selecione um email válido que ainda não foi adicionado.");
            }
        }

        private void btnCriarEquipe_Click(object sender, EventArgs e)
        {
            try
            {
                string nomeEquipe = txtNomeEquipe.Text.Trim();
                string categoria = cmbCategoriaEquipe.Text.Trim();

                if (string.IsNullOrEmpty(nomeEquipe))
                {
                    MessageBox.Show("Informe o nome da equipe.");
                    return;
                }
                if (string.IsNullOrEmpty(categoria))
                {
                    MessageBox.Show("Informe a categoria da equipe.");
                    return;
                }
                if (membrosSelecionados.Count == 0)
                {
                    MessageBox.Show("Adicione pelo menos um membro (email) para a equipe.");
                    return;
                }

                equipe.setNomeEquipe(nomeEquipe);
                equipe.setCategoria(categoria);

                string adminId = "0";
                if (Sessao.AdminLogado != null)
                {
                    adminId = Sessao.AdminLogado.getAdminId() ?? "0";
                }
                equipe.setAdminId(adminId);

                equipe.setFotoEquipe(fotoEquipe);

                string idEmpresa = "0";
                if (Sessao.AdminLogado != null)
                {
                    idEmpresa = Sessao.AdminLogado.getIdEmpresa() ?? "0";
                }
                equipe.setIdEmpresa(idEmpresa);

                int idEquipe = equipe.InserirEquipeRetornandoId();

                foreach (string email in membrosSelecionados)
                {
                    equipe.InserirMembroEquipe(idEquipe, email);
                }

                MessageBox.Show("Equipe cadastrada com sucesso!");

                // Atualiza categorias para refletir a nova categoria inserida (se for nova)
                CarregarCategoriasEquipe();

                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar equipe: " + ex.Message);
            }
        }

        private void CarregarCategoriasEquipe()
        {
            string connectionString = "server=localhost;database=Dev4Tech;uid=root;pwd=";
            string query = "SELECT DISTINCT nome_categoria FROM Categorias ORDER BY nome_categoria;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            cmbCategoriaEquipe.Items.Clear();
                            while (reader.Read())
                            {
                                cmbCategoriaEquipe.Items.Add(reader["nome_categoria"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            CarregarCategoriasEquipe();
        }

        private void LimparFormulario()
        {
            txtNomeEquipe.Clear();
            cmbCategoriaEquipe.Text = "";
            cbmEmailMembro.Text = "";
            membrosSelecionados.Clear();
            funcionariosSelecionados.Clear();
            panelDadosFunc.Controls.Clear();

            // Limpar a foto também
            picBoxFtEquipe.Image = null;
            fotoEquipe = null;
        }

        // Eventos adicionais para botões e controles conforme você já tem no código
        private void btnConfig_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            if (funcionario != null)
            {
                Configuracoes config = new Configuracoes(funcionario);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum funcionário logado.");
            }
        }
        private void btnRanking_Click_1(object sender, EventArgs e)
        {
            Ranking_Equipes rank = new Ranking_Equipes();
            rank.Show();
            this.Hide();
        }
        private void btnCalendar_Click(object sender, EventArgs e)
        {
            AvaliaçãoTarefaAdmin t_pendentes = new AvaliaçãoTarefaAdmin();
            t_pendentes.Show();
            this.Hide();
        }
        private void btnEquipes_Click_1(object sender, EventArgs e)
        {
            PesquisaEquipes p_equipes = new PesquisaEquipes();
            p_equipes.Show();
            this.Hide();
        }
        private void btnHome_Click_1(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Home h = new Home();
                h.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                HomeAdm t_equipeAdmin = new HomeAdm();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }
        private void panelDadosFunc_Paint(object sender, PaintEventArgs e)
        {
        }
        private void AdicionarEquipes_Load_1(object sender, EventArgs e)
        {
        }

        private string fotoEquipe;

        private void btnFtEquipe_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.webp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string pastaDestino = @"C:\xampp\htdocs\dev4tech\img\";
                        if (!Directory.Exists(pastaDestino))
                        {
                            Directory.CreateDirectory(pastaDestino);
                        }

                        // Gerar nome único para o arquivo
                        string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(ofd.FileName);
                        string caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

                        // Copiar arquivo para a pasta de imagens
                        File.Copy(ofd.FileName, caminhoCompleto, true);

                        // Exibir imagem no PictureBox
                        picBoxFtEquipe.Image = Image.FromFile(caminhoCompleto);
                        picBoxFtEquipe.SizeMode = PictureBoxSizeMode.StretchImage;

                        // Salvar apenas o nome do arquivo (igual ao mobile)
                        fotoEquipe = nomeArquivo;

                        MessageBox.Show("Imagem carregada com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        // Método auxiliar para obter o encoder JPEG
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }

        private void btnSalvarImg_Click(object sender, EventArgs e)
        {

        }

        private void cmbCategoriaEquipe_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtNomeEquipe_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbCategoriaEquipe_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Integrantes_Equipe : Form
    {
        private int equipeSelecionadaId = -1;
        private string nomeEquipeSelecionada;
        private string categoriaEquipeSelecionada;
        private string baseFolder = @"C:\xampp\htdocs\dev4tech";
        private string basePathImagemEquipe = @"C:\xampp\htdocs\dev4tech\img";
        private const string txtPesquisarPlaceholder = "🔎 Pesquisar Membros";

        public Integrantes_Equipe()
        {
            InitializeComponent();
            CarregarEquipes();
            CarregarFotoUsuario();
            ConfigurarPlaceholder();
        }

        // Métodos auxiliares para processar fotos
        private string TryDecodeUtf8(byte[] bytes)
        {
            try
            {
                string s = Encoding.UTF8.GetString(bytes).Trim('\0').Trim();
                return s;
            }
            catch
            {
                return null;
            }
        }

        private bool LooksLikePath(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.ToLowerInvariant();
            if (s.Contains("img/") || s.Contains("img\\") || s.Contains(".jpg") ||
                s.Contains(".jpeg") || s.Contains(".png") || s.Contains(".bmp"))
                return true;
            return false;
        }

        private string ResolveStoredPathToFullPath(string stored)
        {
            if (string.IsNullOrWhiteSpace(stored)) return null;

            try
            {
                stored = stored.Trim().Trim('"').Trim('\'');
                string normalized = stored.Replace('/', Path.DirectorySeparatorChar)
                                         .Replace('\\', Path.DirectorySeparatorChar);

                if (Path.IsPathRooted(normalized))
                {
                    return normalized;
                }

                string prefix = "img" + Path.DirectorySeparatorChar;
                if (normalized.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    string withoutLeading = normalized.Substring(prefix.Length);
                    return Path.Combine(baseFolder, "img", withoutLeading);
                }

                if (normalized.Equals("img", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Path.Combine(baseFolder, "img");
                }

                if (!normalized.Contains(Path.DirectorySeparatorChar))
                {
                    return Path.Combine(basePathImagemEquipe, normalized);
                }

                return Path.Combine(baseFolder, normalized.TrimStart(Path.DirectorySeparatorChar));
            }
            catch
            {
                return null;
            }
        }

        private void ConfigurarPlaceholder()
        {
            txtPesquisarMembros.ForeColor = Color.Gray;
            txtPesquisarMembros.Text = txtPesquisarPlaceholder;

            txtPesquisarMembros.Enter += (s, e) =>
            {
                if (txtPesquisarMembros.Text == txtPesquisarPlaceholder)
                {
                    txtPesquisarMembros.Text = "";
                    txtPesquisarMembros.ForeColor = SystemColors.WindowText;
                }
            };

            txtPesquisarMembros.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPesquisarMembros.Text))
                {
                    txtPesquisarMembros.Text = txtPesquisarPlaceholder;
                    txtPesquisarMembros.ForeColor = Color.Gray;
                }
            };
        }

        private void CarregarEquipes()
        {
            panelEquipes.Controls.Clear();
            PesquisaIntegrantes dao = new PesquisaIntegrantes();
            DataTable equipes = dao.BuscarEquipesComCategoriaEMembros();
            int top = 10;

            foreach (DataRow row in equipes.Rows)
            {
                int idEquipe = Convert.ToInt32(row["id_equipe"]);
                string nomeEquipe = row["nome_equipe"].ToString();
                string categoria = row["nome_categoria"].ToString();

                Panel equipePanel = new Panel
                {
                    Width = 650,
                    Height = 120,
                    BackColor = Color.White,
                    Top = top,
                    Left = 10,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = idEquipe
                };

                PictureBox picEquipe = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 40,
                    Height = 40,
                    Left = 10,
                    Top = 15
                };

                object fotoEquipeData = row["foto_equipe"];
                Image fotoEquipe = ObterFotoEquipeDosDados(fotoEquipeData);
                picEquipe.Image = fotoEquipe ?? Properties.Resources.icon_EquipLogo;

                equipePanel.Controls.Add(picEquipe);

                Label lblNome = new Label
                {
                    Text = nomeEquipe,
                    Font = new Font("Poppins Medium", 9, FontStyle.Bold),
                    Left = 60,
                    Top = 10,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblNome);

                Label lblCategoria = new Label
                {
                    Text = categoria,
                    Font = new Font("Poppins", 9, FontStyle.Regular),
                    Left = 60,
                    Top = 35,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblCategoria);

                // Fotos dos membros (até 3)
                DataTable membros = dao.BuscarMembrosDaEquipe(idEquipe);
                int leftFoto = 10;
                int count = 0;

                foreach (DataRow m in membros.Rows)
                {
                    if (count >= 3) break;

                    PictureBox picMembro = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 32,
                        Height = 32,
                        Left = leftFoto,
                        Top = 70,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    object fotoMembroData = m["foto_perfil"];
                    Image fotoMembro = ObterFotoMembroDosDados(fotoMembroData);
                    picMembro.Image = fotoMembro ?? Properties.Resources.icon_perfil;

                    equipePanel.Controls.Add(picMembro);
                    leftFoto += 35;
                    count++;
                }

                equipePanel.Click += (s, e) =>
                {
                    equipeSelecionadaId = idEquipe;
                    nomeEquipeSelecionada = nomeEquipe;
                    categoriaEquipeSelecionada = categoria;

                    lblNomeEquipe.Text = nomeEquipeSelecionada;
                    lblCategoriaEquipe.Text = categoriaEquipeSelecionada;

                    CarregarMembrosDaEquipe();
                };

                panelEquipes.Controls.Add(equipePanel);
                top += 140;
            }
        }

        private void CarregarMembrosDaEquipe(string filtroNome = "")
        {
            panelMembros.Controls.Clear();
            if (equipeSelecionadaId == -1) return;

            PesquisaIntegrantes dao = new PesquisaIntegrantes();
            bool usarFiltroNome = !string.IsNullOrEmpty(filtroNome) &&
                                filtroNome != txtPesquisarPlaceholder &&
                                txtPesquisarMembros.ForeColor != Color.Gray;

            DataTable membros = dao.BuscarMembrosDaEquipe(equipeSelecionadaId,
                usarFiltroNome ? filtroNome : "");

            int top = 10;

            foreach (DataRow row in membros.Rows)
            {
                Panel membroPanel = new Panel
                {
                    Width = 600,
                    Height = 60,
                    BackColor = Color.WhiteSmoke,
                    Top = top,
                    Left = 10,
                    BorderStyle = BorderStyle.FixedSingle
                };

                PictureBox pic = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 48,
                    Height = 48,
                    Left = 10,
                    Top = 6,
                    BorderStyle = BorderStyle.FixedSingle
                };

                object fotoMembroData = row["foto_perfil"];
                Image fotoMembro = ObterFotoMembroDosDados(fotoMembroData);
                pic.Image = fotoMembro ?? Properties.Resources.icon_perfil;

                membroPanel.Controls.Add(pic);

                Label lblNome = new Label
                {
                    Text = row["Nome"].ToString(),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Left = 70,
                    Top = 8,
                    AutoSize = true
                };
                membroPanel.Controls.Add(lblNome);

                Label lblEmail = new Label
                {
                    Text = "Email: " + row["Email"].ToString(),
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 70,
                    Top = 28,
                    AutoSize = true
                };
                membroPanel.Controls.Add(lblEmail);

                Label lblTelefone = new Label
                {
                    Text = "Telefone: " + row["Telefone"].ToString(),
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 250,
                    Top = 28,
                    AutoSize = true
                };
                membroPanel.Controls.Add(lblTelefone);

                panelMembros.Controls.Add(membroPanel);
                top += 70;
            }
        }

        private Image ObterFotoEquipeDosDados(object fotoData)
        {
            Image fotoEquipe = null;

            if (fotoData != null && fotoData != DBNull.Value)
            {
                if (fotoData is byte[] imageData)
                {
                    // É um blob - tentar carregar como imagem diretamente
                    try
                    {
                        using (var ms = new MemoryStream(imageData))
                        {
                            fotoEquipe = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao carregar imagem do blob: {ex.Message}");
                        // Tentar como string se falhar como imagem
                        try
                        {
                            string possivelCaminho = TryDecodeUtf8(imageData);
                            if (!string.IsNullOrEmpty(possivelCaminho) && LooksLikePath(possivelCaminho))
                            {
                                string fullPath = ResolveStoredPathToFullPath(possivelCaminho);
                                if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                                {
                                    fotoEquipe = Image.FromFile(fullPath);
                                }
                            }
                        }
                        catch
                        {
                            // Se tudo falhar, retorna null e usará imagem padrão
                        }
                    }
                }
                else if (fotoData is string caminhoRelativo)
                {
                    // É um caminho
                    string fullPath = ResolveStoredPathToFullPath(caminhoRelativo);
                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        try
                        {
                            fotoEquipe = Image.FromFile(fullPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                        }
                    }
                }
            }
            return fotoEquipe;
        }

        private Image ObterFotoMembroDosDados(object fotoData)
        {
            Image fotoMembro = null;

            if (fotoData != null && fotoData != DBNull.Value)
            {
                if (fotoData is byte[] imageData)
                {
                    // É um blob - tentar carregar como imagem diretamente
                    try
                    {
                        using (var ms = new MemoryStream(imageData))
                        {
                            fotoMembro = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao carregar imagem do blob: {ex.Message}");
                        // Tentar como string se falhar como imagem
                        try
                        {
                            string possivelCaminho = TryDecodeUtf8(imageData);
                            if (!string.IsNullOrEmpty(possivelCaminho) && LooksLikePath(possivelCaminho))
                            {
                                string fullPath = ResolveStoredPathToFullPath(possivelCaminho);
                                if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                                {
                                    fotoMembro = Image.FromFile(fullPath);
                                }
                            }
                        }
                        catch
                        {
                            // Se tudo falhar, retorna null e usará imagem padrão
                        }
                    }
                }
                else if (fotoData is string caminhoRelativo)
                {
                    // É um caminho
                    string fullPath = ResolveStoredPathToFullPath(caminhoRelativo);
                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        try
                        {
                            fotoMembro = Image.FromFile(fullPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                        }
                    }
                }
            }
            return fotoMembro;
        }

        private void CarregarFotoUsuario()
        {
            try
            {
                var usuarioFoto = new UsuarioFoto();
                Image foto = usuarioFoto.ObterFotoUsuario();

                if (picPerfil != null)
                {
                    if (foto != null)
                    {
                        picPerfil.Image = foto;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        picPerfil.Image = Properties.Resources.icon_perfil;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar foto do usuário: {ex.Message}");
                if (picPerfil != null)
                {
                    picPerfil.Image = Properties.Resources.icon_perfil;
                    picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        // Métodos de navegação (mantidos conforme original)
        private void lblPlanejamento_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (Sessao.IdEquipeSelecionada != 0)
            {
                int idEquipe = Sessao.IdEquipeSelecionada;

                if (funcionario != null)
                {
                    Planejamento t_equipe = new Planejamento();
                    t_equipe.Show();
                    this.Hide();
                }
                else if (admin != null)
                {
                    MessageBox.Show("Tela voltada para tarefas dos funcionários.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
                    t_equipeAdmin.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Nenhuma equipe selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PesquisaEquipes rank = new PesquisaEquipes();
                rank.Show();
                this.Hide();
            }
        }

        private void lblRanking_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Tarefas_Pendentes t_equipe = new Tarefas_Pendentes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AvaliaçãoTarefaAdmin t_equipeAdmin = new AvaliaçãoTarefaAdmin();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblGeral_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (Sessao.IdEquipeSelecionada != 0)
            {
                string nomeEquipe = Sessao.NomeEquipeSelecionada;
                string categoriaEquipe = Sessao.CategoriaEquipeSelecionada;

                if (funcionario != null)
                {
                    Chat_geral_equipes t_equipe = new Chat_geral_equipes(Sessao.IdEquipeSelecionada, nomeEquipe, categoriaEquipe);
                    t_equipe.Show();
                    this.Hide();
                }
                else if (admin != null)
                {
                    Chat_geral_equipes t_equipeAdmin = new Chat_geral_equipes(Sessao.IdEquipeSelecionada, nomeEquipe, categoriaEquipe);
                    t_equipeAdmin.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Nenhuma equipe selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PesquisaEquipes pesquisa = new PesquisaEquipes();
                pesquisa.Show();
                this.Hide();
            }
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

        private void btnEquipes_Click_1(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                PesquisaEquipes t_equipe = new PesquisaEquipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRanking_Click_1(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConfigurações_Click(object sender, EventArgs e)
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

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Tarefas_Pendentes t_equipe = new Tarefas_Pendentes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AvaliaçãoTarefaAdmin t_equipeAdmin = new AvaliaçãoTarefaAdmin();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtProcurarMebros_TextChanged(object sender, EventArgs e)
        {
            if (txtPesquisarMembros.Text != txtPesquisarPlaceholder &&
                txtPesquisarMembros.ForeColor != Color.Gray)
            {
                CarregarMembrosDaEquipe(txtPesquisarMembros.Text.Trim());
            }
        }

        private void btnMostrarMembros_Click(object sender, EventArgs e) { }

        private void Integrantes_Equipe_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class PesquisaEquipes : Form
    {
        private string basePathImagemEquipe = @"C:\xampp\htdocs\dev4tech\img";
        private string baseFolder = @"C:\xampp\htdocs\dev4tech";
        private int mensagensCount = 0;
        private int margemTopo = 30;
        private int margemEsquerda = 350;
        private int espacamentoVertical = 20;
        private int alturaMensagem = 140;
        private const string TextoPlaceholder = "Pesquisar equipe";

        public PesquisaEquipes()
        {
            InitializeComponent();
            Sessao.LimparEquipeSelecionada();
            panelEquipes.AutoScroll = true;
            ConfigurarPlaceholder();
            CarregarCategorias();
            CarregarEquipes(); // carrega tudo inicialmente
            CarregarFotoUsuario();
        }

        private void ConfigurarPlaceholder()
        {
            txtPesquisaEquipe.ForeColor = Color.Gray;
            txtPesquisaEquipe.Text = TextoPlaceholder;

            // Quando entra no campo
            txtPesquisaEquipe.Enter += (s, e) =>
            {
                if (txtPesquisaEquipe.Text == TextoPlaceholder)
                {
                    txtPesquisaEquipe.Text = "";
                    txtPesquisaEquipe.ForeColor = Color.Black;
                }
            };

            txtPesquisaEquipe.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPesquisaEquipe.Text))
                {
                    txtPesquisaEquipe.Text = TextoPlaceholder;
                    txtPesquisaEquipe.ForeColor = Color.Gray;
                }
            };

            txtPesquisaEquipe.TextChanged += (s, e) =>
            {
                if (txtPesquisaEquipe.ForeColor == Color.Gray) return;
                AtualizarEquipes();
            };
        }

        private void CarregarCategorias()
        {
            try
            {
                FiltroEquipes filtro = new FiltroEquipes();
                DataTable dtCategorias = filtro.ObterCategorias();

                filtroEquipes.Items.Clear();
                filtroEquipes.Items.Add("Todos");
                foreach (DataRow row in dtCategorias.Rows)
                {
                    filtroEquipes.Items.Add(row["nome_categoria"].ToString());
                }
                filtroEquipes.SelectedIndex = 0;

                // Evento para atualizar ao trocar categoria
                filtroEquipes.SelectedIndexChanged += (s, e) => AtualizarEquipes();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar categorias: {ex.Message}");
            }
        }

        private void AtualizarEquipes()
        {
            string categoria = filtroEquipes.SelectedItem?.ToString();

            // Se o texto é o placeholder ou está em cinza, considera como sem filtro de nome
            string textoPesquisa = txtPesquisaEquipe.ForeColor == Color.Gray ? null : txtPesquisaEquipe.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoPesquisa)) textoPesquisa = null;

            CarregarEquipes(categoria, textoPesquisa);
        }

        private void CarregarEquipes(string filtroCategoria = null, string filtroNome = null)
        {
            panelEquipes.Controls.Clear();
            mensagensCount = 0;
            FiltroEquipes filtro = new FiltroEquipes();

            DataTable equipesDt = filtro.ObterEquipesDoUsuario(filtroCategoria, filtroNome);

            foreach (DataRow equipe in equipesDt.Rows)
            {
                int idEquipe = equipe.Field<int>("id_equipe");
                int diasDesdeUltimaAtividade = -1;
                if (!equipe.IsNull("ultima_atividade"))
                    diasDesdeUltimaAtividade = (DateTime.Now - equipe.Field<DateTime>("ultima_atividade")).Days;

                // Get members for this specific team
                DataTable membrosDt = filtro.ObterMembrosDaEquipe(idEquipe);
                var membros = membrosDt.AsEnumerable().Select(r =>
                {
                    object fotoObj = r["foto_perfil"];
                    string caminhoFoto = null;
                    byte[] blobFoto = null;

                    if (fotoObj != null && fotoObj != DBNull.Value)
                    {
                        if (fotoObj is byte[] bytes)
                        {
                            // Tenta decodificar como string (caminho)
                            string possivelCaminho = TryDecodeUtf8(bytes);
                            if (!string.IsNullOrEmpty(possivelCaminho) && LooksLikePath(possivelCaminho))
                            {
                                caminhoFoto = possivelCaminho;
                            }
                            else
                            {
                                blobFoto = bytes;
                            }
                        }
                        else if (fotoObj is string caminho)
                        {
                            caminhoFoto = caminho;
                        }
                    }

                    return new MembroEquipe
                    {
                        IdFuncionario = r.Field<int>("FuncionarioId"),
                        Nome = r.Field<string>("nome_funcionario"),
                        CaminhoFotoPerfil = caminhoFoto,
                        FotoBlob = blobFoto
                    };
                }).ToList();

                AdicionarPainelEquipe(
                    equipe.Field<string>("nome_equipe"),
                    equipe.Field<string>("nome_categoria"),
                    membros,
                    idEquipe,
                    diasDesdeUltimaAtividade,
                    equipe["foto_equipe"]
                );
            }

            // Se não encontrou nada, exibe mensagem
            if (panelEquipes.Controls.Count == 0)
            {
                Label lblSem = new Label
                {
                    Text = "Nenhuma equipe encontrada.",
                    AutoSize = true,
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    Left = 10,
                    Top = 10
                };
                panelEquipes.Controls.Add(lblSem);
            }
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

        public class MembroEquipe
        {
            public int IdFuncionario { get; set; }
            public string Nome { get; set; }
            public string CaminhoFotoPerfil { get; set; }
            public byte[] FotoBlob { get; set; }
        }

        private void AdicionarPainelEquipe(string nomeEquipe, string categoria,
            System.Collections.Generic.List<MembroEquipe> membros, int idEquipe,
            int diasDesdeUltimaAtividade, object fotoEquipeData)
        {
            int x = margemEsquerda;
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;
            Panel equipePanel = new Panel
            {
                Width = 627,
                Height = alturaMensagem,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Left = x,
                Top = y,
                Cursor = Cursors.Hand
            };

            // CAPTURAR VALORES ANTES DO EVENTO
            int equipeId = idEquipe;
            string equipeNome = nomeEquipe;
            string equipeCategoria = categoria;

            equipePanel.Click += (s, e) =>
            {
                Sessao.DefinirEquipeSelecionada(equipeId, equipeNome, equipeCategoria);
                Chat_geral_equipes chatForm = new Chat_geral_equipes();
                chatForm.Show();
                this.Hide();
            };

            // CARREGAR FOTO DA EQUIPE
            PictureBox picEquipe = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 40,
                Height = 40,
                Left = 10,
                Top = 10,
                BorderStyle = BorderStyle.FixedSingle
            };

            Image fotoEquipe = ObterFotoEquipeDosDados(fotoEquipeData);
            picEquipe.Image = fotoEquipe ?? Properties.Resources.icon_EquipLogo;
            equipePanel.Controls.Add(picEquipe);

            Label lblNomeEquipe = new Label
            {
                Text = nomeEquipe,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 60,
                Top = 5,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblNomeEquipe);

            Label lblCategoria = new Label
            {
                Text = categoria,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Left = 60,
                Top = 30,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblCategoria);

            string textoAtividade = diasDesdeUltimaAtividade == -1
                ? "Nunca ativo"
                : $"Ativo há {diasDesdeUltimaAtividade} dia(s)";
            Label lblAtividade = new Label
            {
                Text = textoAtividade,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = diasDesdeUltimaAtividade > 7 ? Color.Red : Color.Green,
                Left = 60,
                Top = 50,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblAtividade);

            Label lblColaboradores = new Label
            {
                Text = "Colaboradores",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Left = 60,
                Top = 70,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblColaboradores);

            int fotoLeft = 60;
            int fotoTop = 90;

            foreach (var membro in membros)
            {
                PictureBox picMembro = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 32,
                    Height = 32,
                    Left = fotoLeft,
                    Top = fotoTop,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                    Tag = membro.IdFuncionario
                };

                // CARREGAR FOTO DO MEMBRO - CORRIGIDO
                if (!string.IsNullOrEmpty(membro.CaminhoFotoPerfil))
                {
                    string fullPath = ResolveStoredPathToFullPath(membro.CaminhoFotoPerfil);
                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        try
                        {
                            using (var ms = new MemoryStream(File.ReadAllBytes(fullPath)))
                            {
                                picMembro.Image = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            picMembro.Image = Properties.Resources.icon_perfil;
                        }
                    }
                    else
                    {
                        picMembro.Image = Properties.Resources.icon_perfil;
                    }
                }
                else if (membro.FotoBlob != null && membro.FotoBlob.Length > 0)
                {
                    try
                    {
                        using (var ms = new MemoryStream(membro.FotoBlob))
                        {
                            ms.Position = 0;
                            picMembro.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        picMembro.Image = Properties.Resources.icon_perfil;
                    }
                }
                else
                {
                    picMembro.Image = Properties.Resources.icon_perfil;
                }

                ToolTip tt = new ToolTip();
                picMembro.MouseHover += (s, e) =>
                {
                    tt.SetToolTip(picMembro, membro.Nome);
                };
                equipePanel.Controls.Add(picMembro);
                fotoLeft += 38;
            }
            panelEquipes.Controls.Add(equipePanel);
            mensagensCount++;
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
                    try
                    {
                        string fullPath = ResolveStoredPathToFullPath(caminhoRelativo);
                        if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                        {
                            fotoEquipe = Image.FromFile(fullPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                    }
                }
            }
            return fotoEquipe;
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

        // Event handlers
        private void txtPesquisaEquipe_Click(object sender, EventArgs e)
        {
            if (txtPesquisaEquipe.Text == TextoPlaceholder)
            {
                txtPesquisaEquipe.Text = "";
                txtPesquisaEquipe.ForeColor = Color.Black;
            }
        }

        private void txtPesquisarEquipe_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPesquisaEquipe.Text))
            {
                txtPesquisaEquipe.Text = TextoPlaceholder;
                txtPesquisaEquipe.ForeColor = Color.Gray;
            }
        }

        private void txtPesquisaEquipe_TextChanged(object sender, EventArgs e)
        {
            if (txtPesquisaEquipe.ForeColor == Color.Gray) return;
            AtualizarEquipes();
        }

        private void filtroEquipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarEquipes();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            AtualizarEquipes();
        }

        // Métodos de navegação (mantidos conforme original)
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
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

        private void btnRanking_Click(object sender, EventArgs e)
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

        private void btnEquipe_Click(object sender, EventArgs e)
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

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Configuracoes config = new Configuracoes(funcionario);
                config.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Configuracoes config = new Configuracoes(admin);
                config.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.");
            }
        }

        private void PesquisaEquipes_Load(object sender, EventArgs e) { }

        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }
    }
}
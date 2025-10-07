using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class PesquisaEquipes : Form
    {
        private string basePathImagemEquipe = @"C:\xampp\htdocs\dev4tech\img";
        private string baseFolder = @"C:\xampp\htdocs\dev4tech\";
        private int mensagensCount = 0;
        private int margemTopo = 30;
        private int margemEsquerda = 350;
        private int espacamentoVertical = 20;
        private int alturaMensagem = 110;

        // Texto placeholder usado na TextBox de pesquisa
        private const string TextoPlaceholder = "Pesquisar equipe";

        public PesquisaEquipes()
        {
            InitializeComponent();
            Sessao.LimparEquipeSelecionada();
            panelEquipes.AutoScroll = true;

            // Inicializa UI (placeholder, categorias e carregamento inicial)
            ConfigurarPlaceholder();
            CarregarCategorias();
            CarregarEquipes(); // carrega tudo inicialmente
            CarregarFotoUsuario();
        }

        // -----------------------
        // CONFIGURAÇÕES / PLACEHOLDER
        // -----------------------
        private void ConfigurarPlaceholder()
        {
            // Define placeholder inicial
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

            // Quando sai do campo
            txtPesquisaEquipe.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPesquisaEquipe.Text))
                {
                    txtPesquisaEquipe.Text = TextoPlaceholder;
                    txtPesquisaEquipe.ForeColor = Color.Gray;
                }
            };

            // Atualiza enquanto digita (ignora se for placeholder)
            txtPesquisaEquipe.TextChanged += (s, e) =>
            {
                if (txtPesquisaEquipe.ForeColor == Color.Gray) return;
                AtualizarEquipes();
            };
        }

        // -----------------------
        // CARREGAR CATEGORIAS NO COMBOBOX
        // -----------------------
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

        // -----------------------
        // MÉTODOS DE ATUALIZAÇÃO / FILTRO
        // -----------------------
        private void AtualizarEquipes()
        {
            string categoria = filtroEquipes.SelectedItem?.ToString();

            // Se o texto é o placeholder ou está em cinza, considera como sem filtro de nome
            string textoPesquisa = txtPesquisaEquipe.ForeColor == Color.Gray ? null : txtPesquisaEquipe.Text.Trim();
            if (string.IsNullOrWhiteSpace(textoPesquisa)) textoPesquisa = null;

            CarregarEquipes(categoria, textoPesquisa);
        }

        // Carrega equipes no painel segundo filtros (categoria e nome)
        // Mantive a assinatura compatível com suas chamadas anteriores (filtroCategoria opcional).
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
                    if (fotoObj is byte[] bytes)
                        blobFoto = bytes;
                    else if (fotoObj is string s)
                        caminhoFoto = s;
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

        // -----------------------
        // CLASSE AUXILIAR
        // -----------------------
        public class MembroEquipe
        {
            public int IdFuncionario { get; set; }
            public string Nome { get; set; }
            public string CaminhoFotoPerfil { get; set; }
            public byte[] FotoBlob { get; set; }
        }

        // -----------------------
        // CRIAÇÃO DOS CARDS (PANELS) DE CADA EQUIPE
        // -----------------------
        private void AdicionarPainelEquipe(string nomeEquipe, string categoria, System.Collections.Generic.List<MembroEquipe> membros, int idEquipe, int diasDesdeUltimaAtividade, object fotoEquipeData)
        {
            int x = margemEsquerda;
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;
            Panel equipePanel = new Panel
            {
                Width = 350,
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

            // CARREGAR FOTO DA EQUIPE - USANDO OS DADOS DO DATATABLE
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

                // CARREGAR FOTO DO MEMBRO - MESMA LÓGICA DAS OUTRAS TELAS
                if (!string.IsNullOrEmpty(membro.CaminhoFotoPerfil))
                {
                    string caminhoFotoCorrigido = membro.CaminhoFotoPerfil.Replace("/", "\\");
                    string caminhoCompleto = Path.Combine(baseFolder, caminhoFotoCorrigido);
                    if (File.Exists(caminhoCompleto))
                    {
                        try
                        {
                            using (var imgTemp = Image.FromFile(caminhoCompleto))
                            {
                                picMembro.Image = new Bitmap(imgTemp);
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

        // -----------------------
        // OBTÉM IMAGEM (BLOB OU CAMINHO) COM TRATAMENTO
        // -----------------------
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
                            string nomeArquivo = System.Text.Encoding.UTF8.GetString(imageData);
                            // LIMPAR O NOME DO ARQUIVO DE CARACTERES INVÁLIDOS
                            nomeArquivo = new string(nomeArquivo.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
                            string caminhoImagemEquipe = Path.Combine(basePathImagemEquipe, nomeArquivo);
                            if (File.Exists(caminhoImagemEquipe))
                            {
                                fotoEquipe = Image.FromFile(caminhoImagemEquipe);
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
                        // LIMPAR O CAMINHO DE CARACTERES INVÁLIDOS
                        caminhoRelativo = new string(caminhoRelativo.Where(c => !Path.GetInvalidPathChars().Contains(c)).ToArray());
                        string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));
                        if (File.Exists(caminhoCompleto))
                        {
                            fotoEquipe = Image.FromFile(caminhoCompleto);
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

        // -----------------------
        // CARREGA FOTO DO USUÁRIO (CANTO)
        // -----------------------
        private void CarregarFotoUsuario()
        {
            try
            {
                var usuarioFoto = new UsuarioFoto();
                Image foto = usuarioFoto.ObterFotoUsuario();

                if (picPerfil != null) // Verifica se o controle existe no form
                {
                    if (foto != null)
                    {
                        picPerfil.Image = foto;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        // Usar imagem padrão se não encontrar foto
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

        // -----------------------
        // EVENT HANDLERS (mantidos/implementados)
        // -----------------------

        private void txtPesquisaEquipe_Click(object sender, EventArgs e)
        {
            // Se clicar e houver o placeholder, limpa
            if (txtPesquisaEquipe.Text == TextoPlaceholder)
            {
                txtPesquisaEquipe.Text = "";
                txtPesquisaEquipe.ForeColor = Color.Black;
            }
        }

        private void txtPesquisarEquipe_Leave(object sender, EventArgs e)
        {
            // Compatibilidade com nome de evento antigo no seu projeto
            if (string.IsNullOrWhiteSpace(txtPesquisaEquipe.Text))
            {
                txtPesquisaEquipe.Text = TextoPlaceholder;
                txtPesquisaEquipe.ForeColor = Color.Gray;
            }
        }

        private void txtPesquisaEquipe_TextChanged(object sender, EventArgs e)
        {
            // Evento ligado pelo designer — respeita placeholder
            if (txtPesquisaEquipe.ForeColor == Color.Gray) return;
            AtualizarEquipes();
        }

        private void filtroEquipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Evento ligado pelo designer — atualiza lista
            AtualizarEquipes();
        }

        private void panelEquipes_Paint(object sender, PaintEventArgs e) { /* Mantido caso precise pintar */ }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            // Se usuário preferir clicar no botão
            AtualizarEquipes();
        }

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
                AdicionarTarefa t_equipeAdmin = new AdicionarTarefa();
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

        private void PesquisaEquipes_Load(object sender, EventArgs e)
        {
        }
    }
}

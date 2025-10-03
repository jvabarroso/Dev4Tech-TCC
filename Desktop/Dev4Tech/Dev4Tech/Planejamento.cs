using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Planejamento : Form
    {
        private planejamentoSQL dbPlanejamento = new planejamentoSQL();
        private Dictionary<int, List<string>> tarefasPaginasCache = new Dictionary<int, List<string>>();
        private Dictionary<int, int> progressoLeitura = new Dictionary<int, int>(); // ID da tarefa -> página atual
        private Dictionary<int, int> totalPaginasPorTarefa = new Dictionary<int, int>(); // ID da tarefa -> total de páginas
        private int idFuncionarioLogado;

        public Planejamento()
        {
            InitializeComponent();
            this.Load += Planejamento_Load;
            CarregarFotoUsuario();
        }

        private void Planejamento_Load(object sender, EventArgs e)
        {
            try
            {
                var funcionario = Sessao.FuncionarioLogado;
                if (funcionario == null)
                {
                    MessageBox.Show("Funcionário não está logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                idFuncionarioLogado = int.Parse(funcionario.getFuncionarioId());
                var idsEquipes = dbPlanejamento.ObterIdsEquipesFuncionario(idFuncionarioLogado);

                if (idsEquipes == null || idsEquipes.Count == 0)
                {
                    MessageBox.Show("Funcionário não pertence a nenhuma equipe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Limpar controles
                flpP.Controls.Clear();
                flpF.Controls.Clear();
                flpC.Controls.Clear();
                flpPDFs.Controls.Clear();

                var tarefas = dbPlanejamento.ObterTarefasPendentesPorEquipesComArquivo(idsEquipes);

                if (tarefas == null)
                {
                    MessageBox.Show("Erro ao carregar tarefas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var tarefa in tarefas)
                {
                    DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(tarefa.IdTarefa);
                    string statusTarefa = ObterStatusTarefa(tarefa.IdTarefa);
                    List<Image> avatares = dbPlanejamento.ObterAvataresPorTarefa(tarefa.IdTarefa);

                    Panel card = CriarCardTarefa(tarefa.NomeTarefa, dataEntrega, avatares, statusTarefa);
                    card.Tag = tarefa.IdTarefa;

                    card.Click += (senderCard, eCard) =>
                    {
                        int idTarefa = (int)((Panel)senderCard).Tag;
                        CarregarPdfDaTarefa(idTarefa);
                    };

                    // Adicionar ao painel correto
                    switch (statusTarefa)
                    {
                        case "Pendente": flpP.Controls.Add(card); break;
                        case "Fazendo": flpF.Controls.Add(card); break;
                        case "Concluida": flpC.Controls.Add(card); break;
                        default: flpP.Controls.Add(card); break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tarefas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObterStatusTarefa(int idTarefa)
        {
            if (!progressoLeitura.ContainsKey(idTarefa) || progressoLeitura[idTarefa] == 0)
                return "Pendente";

            int paginaAtual = progressoLeitura[idTarefa];
            int totalPaginas = totalPaginasPorTarefa.ContainsKey(idTarefa) ? totalPaginasPorTarefa[idTarefa] : 0;

            if (totalPaginas == 0) return "Pendente";

            if (paginaAtual > 0 && paginaAtual < totalPaginas)
                return "Fazendo";
            else if (paginaAtual >= totalPaginas)
                return "Concluida";
            else
                return "Pendente";
        }

        private Panel CriarCardTarefa(string titulo, DateTime dataEntrega, List<Image> avatares, string status)
        {
            Panel card = new Panel
            {
                Width = flpP.Width - 25,
                Height = 90,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Padding = new Padding(8),
                BackColor = GetCorStatus(status),
                Cursor = Cursors.Hand
            };

            // Adicionar badge de status
            Label lblStatus = new Label
            {
                Text = status,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetCorBadgeStatus(status),
                AutoSize = true,
                Location = new Point(card.Width - 70, 5),
                Padding = new Padding(3, 1, 3, 1),
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblStatus);

            FlowLayoutPanel membrosPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Height = 24,
                Width = card.Width - 80,
                Location = new Point(0, 0),
                WrapContents = false,
                AutoScroll = false,
            };

            foreach (var avatar in avatares.Take(5))
            {
                PictureBox pic = new PictureBox
                {
                    Image = avatar,
                    Width = 24,
                    Height = 24,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = new Padding(2)
                };
                membrosPanel.Controls.Add(pic);
            }

            Label lblTitulo = new Label
            {
                Text = titulo.Length > 50 ? titulo.Substring(0, 47) + "..." : titulo,
                Location = new Point(0, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = false,
                Width = card.Width - 16,
                Height = 30,
                ForeColor = Color.Black
            };

            Label lblData = new Label
            {
                Text = "Até " + dataEntrega.ToString("dd/MM/yyyy"),
                Location = new Point(0, 65),
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            card.Controls.Add(membrosPanel);
            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblData);

            return card;
        }

        private Color GetCorStatus(string status)
        {
            switch (status)
            {
                case "Fazendo": return Color.FromArgb(255, 255, 240); // Amarelo claro
                case "Concluida": return Color.FromArgb(240, 255, 240); // Verde claro
                default: return Color.White; // Branco para Pendente
            }
        }

        private Color GetCorBadgeStatus(string status)
        {
            switch (status)
            {
                case "Pendente": return Color.Gray;
                case "Fazendo": return Color.Orange;
                case "Concluida": return Color.Green;
                default: return Color.Gray;
            }
        }

        private void CarregarPdfDaTarefa(int idTarefa)
        {
            try
            {
                // Verificar se já temos as páginas em cache
                if (!tarefasPaginasCache.ContainsKey(idTarefa))
                {
                    string nomeArquivo = dbPlanejamento.ObterNomeArquivoTarefa(idTarefa);

                    if (string.IsNullOrEmpty(nomeArquivo))
                    {
                        MessageBox.Show("Nenhum arquivo PDF encontrado para essa tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Verificar se a pasta de arquivos existe
                    if (!dbPlanejamento.VerificarPastaArquivos())
                    {
                        MessageBox.Show("Pasta de arquivos não encontrada. Contate o administrador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string caminhoPdf = dbPlanejamento.ObterCaminhoCompletoPdf(nomeArquivo);

                    if (!File.Exists(caminhoPdf))
                    {
                        MessageBox.Show($"Arquivo PDF não encontrado: {caminhoPdf}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string pastaPaginas = dbPlanejamento.CriarPastaTemporaria();
                    List<string> paginas = dbPlanejamento.DividirPdfEmPaginas(caminhoPdf, pastaPaginas);

                    if (paginas.Count == 0)
                    {
                        MessageBox.Show("Não foi possível dividir o PDF em páginas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    tarefasPaginasCache[idTarefa] = paginas;
                    totalPaginasPorTarefa[idTarefa] = paginas.Count;
                }

                // Inicializar progresso se não existir
                if (!progressoLeitura.ContainsKey(idTarefa))
                {
                    progressoLeitura[idTarefa] = 0;
                }

                ExibirPdfsNoFlowLayout(tarefasPaginasCache[idTarefa], idTarefa);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar PDFs: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class PdfPaginaInfo
        {
            public string Arquivo { get; set; }
            public int IdTarefa { get; set; }
            public int Pagina { get; set; }
            public int Total { get; set; }
        }

        private void ExibirPdfsNoFlowLayout(List<string> caminhosArquivosPdf, int idTarefa)
        {
            flpPDFs.Controls.Clear();
            if (!caminhosArquivosPdf.Any())
            {
                Label lblSemPaginas = new Label
                {
                    Text = "Nenhuma página encontrada",
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                flpPDFs.Controls.Add(lblSemPaginas);
                return;
            }
            int paginaAtual = progressoLeitura.ContainsKey(idTarefa) ? progressoLeitura[idTarefa] : 0;
            int totalPaginas = caminhosArquivosPdf.Count;
            for (int i = 0; i < caminhosArquivosPdf.Count; i++)
            {
                string caminhoPdf = caminhosArquivosPdf[i];
                int numeroPagina = i + 1;
                bool foiLida = numeroPagina <= paginaAtual;
                var info = new PdfPaginaInfo
                {
                    Arquivo = caminhoPdf,
                    IdTarefa = idTarefa,
                    Pagina = numeroPagina,
                    Total = totalPaginas
                };
                Panel painelCartao = new Panel
                {
                    Width = 150,
                    Height = 200,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Tag = info,
                    Cursor = Cursors.Hand,
                    BackColor = foiLida ? Color.LightGreen : Color.White
                };
                Button btnAbrirPdf = new Button
                {
                    Text = $"Página {numeroPagina}",
                    Dock = DockStyle.Bottom,
                    Height = 30,
                    BackColor = foiLida ? Color.Green : SystemColors.Control,
                    ForeColor = foiLida ? Color.White : SystemColors.ControlText
                };
                btnAbrirPdf.Click += (s, e) =>
                {
                    var infoTag = (PdfPaginaInfo)((Button)s).Parent.Tag;
                    AbrirPdfExternamente(infoTag.Arquivo, infoTag.IdTarefa, infoTag.Pagina, infoTag.Total);
                };
                PictureBox picThumbnail = new PictureBox
                {
                    Image = foiLida ? Properties.Resources.icon_documento : Properties.Resources.icon_documento_blue,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill
                };
                if (foiLida)
                {
                    Label lblLida = new Label
                    {
                        Text = "✓",
                        Font = new Font("Segoe UI", 16, FontStyle.Bold),
                        ForeColor = Color.Green,
                        BackColor = Color.Transparent,
                        AutoSize = true,
                        Location = new Point(5, 5)
                    };
                    painelCartao.Controls.Add(lblLida);
                }
                painelCartao.Click += (s, e) =>
                {
                    var infoTag = (PdfPaginaInfo)((Panel)s).Tag;
                    AbrirPdfExternamente(infoTag.Arquivo, infoTag.IdTarefa, infoTag.Pagina, infoTag.Total);
                };
                painelCartao.Controls.Add(picThumbnail);
                painelCartao.Controls.Add(btnAbrirPdf);
                flpPDFs.Controls.Add(painelCartao);
            }
            AdicionarBarraProgresso(idTarefa, paginaAtual, totalPaginas);
        }


        private void AdicionarBarraProgresso(int idTarefa, int paginaAtual, int totalPaginas)
        {
            Panel panelProgresso = new Panel
            {
                Width = flpPDFs.Width - 20,
                Height = 30,
                BackColor = Color.Transparent,
                Margin = new Padding(10, 0, 10, 10)
            };

            // Label de progresso
            Label lblProgresso = new Label
            {
                Text = $"Progresso: {paginaAtual}/{totalPaginas} páginas",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(0, 5)
            };
            panelProgresso.Controls.Add(lblProgresso);

            // Barra de progresso visual
            if (totalPaginas > 0)
            {
                int larguraBarra = panelProgresso.Width - 150;
                Panel panelBarraFundo = new Panel
                {
                    Width = larguraBarra,
                    Height = 15,
                    BackColor = Color.LightGray,
                    Location = new Point(panelProgresso.Width - larguraBarra, 7)
                };

                int larguraPreenchimento = (int)(larguraBarra * ((double)paginaAtual / totalPaginas));
                Panel panelBarraPreenchida = new Panel
                {
                    Width = larguraPreenchimento,
                    Height = 15,
                    BackColor = paginaAtual == totalPaginas ? Color.Green : Color.Blue,
                    Location = new Point(0, 0)
                };

                panelBarraFundo.Controls.Add(panelBarraPreenchida);
                panelProgresso.Controls.Add(panelBarraFundo);
            }

            flpPDFs.Controls.Add(panelProgresso);
        }

        private void AbrirPdfExternamente(string caminhoPdf, int idTarefa, int numeroPagina, int totalPaginas)
        {
            try
            {
                if (File.Exists(caminhoPdf))
                {
                    // Atualizar o progresso - sempre pega a página mais alta
                    if (!progressoLeitura.ContainsKey(idTarefa) || numeroPagina > progressoLeitura[idTarefa])
                    {
                        progressoLeitura[idTarefa] = numeroPagina;
                    }

                    // Abrir o PDF
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = caminhoPdf,
                        UseShellExecute = true
                    });

                    // Atualizar a exibição do PDF
                    ExibirPdfsNoFlowLayout(tarefasPaginasCache[idTarefa], idTarefa);

                    // Atualizar os painéis de tarefas
                    AtualizarPainelTarefas();
                }
                else
                {
                    MessageBox.Show("Arquivo PDF não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarPainelTarefas()
        {
            // Limpar os painéis
            flpP.Controls.Clear();
            flpF.Controls.Clear();
            flpC.Controls.Clear();

            // Recarregar as tarefas
            var funcionario = Sessao.FuncionarioLogado;
            if (funcionario == null) return;

            int idFuncionario = int.Parse(funcionario.getFuncionarioId());
            var idsEquipes = dbPlanejamento.ObterIdsEquipesFuncionario(idFuncionario);
            var tarefas = dbPlanejamento.ObterTarefasPendentesPorEquipesComArquivo(idsEquipes);

            foreach (var tarefa in tarefas)
            {
                DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(tarefa.IdTarefa);
                string statusTarefa = ObterStatusTarefa(tarefa.IdTarefa);
                List<Image> avatares = dbPlanejamento.ObterAvataresPorTarefa(tarefa.IdTarefa);

                Panel card = CriarCardTarefa(tarefa.NomeTarefa, dataEntrega, avatares, statusTarefa);
                card.Tag = tarefa.IdTarefa;

                card.Click += (senderCard, eCard) =>
                {
                    int idTarefa = (int)((Panel)senderCard).Tag;
                    CarregarPdfDaTarefa(idTarefa);
                };

                switch (statusTarefa)
                {
                    case "Pendente":
                        flpP.Controls.Add(card);
                        break;
                    case "Fazendo":
                        flpF.Controls.Add(card);
                        break;
                    case "Concluida":
                        flpC.Controls.Add(card);
                        break;
                }
            }
        }

        // Métodos de navegação (mantidos do código original)

        private void btnPendentes_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes trf_Pendentes = new Tarefas_Pendentes();
            trf_Pendentes.Show();
            this.Hide();
        }

        private void btnEmAtraso_Click(object sender, EventArgs e)
        {
            Tarefas_Atrasadas trf_Atrasadas = new Tarefas_Atrasadas();
            trf_Atrasadas.Show();
            this.Hide();
        }

        private void btnCompletadas_Click(object sender, EventArgs e)
        {
            Tarefas_Completadas trf_Completas = new Tarefas_Completadas();
            trf_Completas.Show();
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

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null || admin != null)
            {
                PesquisaEquipes t_equipe = new PesquisaEquipes();
                t_equipe.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEstatisticas_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null || admin != null)
            {
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void picPerfilMembro_Click(object sender, EventArgs e)
        {
            // Implementar se necessário
        }

        private void lblPlanejamento_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null || admin != null)
            {
                Planejamento t_planejamento = new Planejamento();
                t_planejamento.Show();
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
                int idEquipe = Sessao.IdEquipeSelecionada;
                string nomeEquipe = "Nome da equipe"; // Ajuste para obter o nome real da equipe
                string categoriaEquipe = "Categoria da equipe"; // Ajuste para obter a categoria real da equipe

                if (funcionario != null)
                {
                    Chat_geral_equipes t_equipe = new Chat_geral_equipes(idEquipe, nomeEquipe, categoriaEquipe);
                    t_equipe.Show();
                    this.Hide();
                }
                else if (admin != null)
                {
                    Chat_geral_equipes t_equipeAdmin = new Chat_geral_equipes(idEquipe, nomeEquipe, categoriaEquipe);
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

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Tarefas_Pendentes t_tarefas = new Tarefas_Pendentes();
                t_tarefas.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AvaliaçãoTarefaAdmin t_admin = new AvaliaçãoTarefaAdmin();
                t_admin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblRanking_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null || admin != null)
            {
                Ranking_Equipes t_ranking = new Ranking_Equipes();
                t_ranking.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblMembros_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
                t_integrantes.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AdicionarEquipes t_adicionar = new AdicionarEquipes();
                t_adicionar.Show();
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
                Tarefas_Pendentes t_tarefas = new Tarefas_Pendentes();
                t_tarefas.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AdicionarTarefa t_adicionar = new AdicionarTarefa();
                t_adicionar.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes p_pendente = new Tarefas_Pendentes();
            p_pendente.Show();
            this.Hide();
        }

        private void flpPDFs_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelKBS_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flpP_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flpF_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flpC_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Tarefa1_Enter(object sender, EventArgs e)
        {
        }

        private void txtPesquisaTarefa_TextChanged(object sender, EventArgs e)
        {
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {
        }

        private void panelTarefas_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Tarefas_Pendentes_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Home t_equipe = new Home();
                t_equipe.Show();
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
    }
}
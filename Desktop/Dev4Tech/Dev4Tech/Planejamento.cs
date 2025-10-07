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

        // SUBSTITUIR os dicionários antigos por estes novos:
        private Dictionary<int, HashSet<int>> paginasVisualizadasCache = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, ProgressoLeitura> progressoCache = new Dictionary<int, ProgressoLeitura>();

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

                // Limpar caches
                paginasVisualizadasCache.Clear();
                progressoCache.Clear();

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

        // MÉTODO COMPLETAMENTE REFEITO
        private string ObterStatusTarefa(int idTarefa)
        {
            try
            {
                // Primeiro verifica se há entrega concluída no sistema antigo
                string statusEntrega = dbPlanejamento.ObterStatusTarefa(idTarefa);
                if (statusEntrega == "Concluida")
                    return "Concluida";

                // Depois verifica o progresso REAL de leitura
                if (!progressoCache.ContainsKey(idTarefa))
                {
                    // Carregar do banco se não estiver em cache
                    var progresso = dbPlanejamento.ObterProgressoLeitura(idTarefa, idFuncionarioLogado);
                    progressoCache[idTarefa] = progresso;
                }

                var progressoAtual = progressoCache[idTarefa];

                if (progressoAtual == null)
                    return "Pendente";

                if (progressoAtual.Concluida)
                    return "Concluida";

                if (progressoAtual.TotalPaginasVisualizadas > 0)
                    return "Fazendo";

                return "Pendente";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter status da tarefa {idTarefa}: {ex.Message}");
                return "Pendente";
            }
        }

        // MÉTODO MODIFICADO para incluir informações de progresso no card
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

                    // Verificar se o PDF foi alterado
                    bool pdfAlterado = dbPlanejamento.VerificarPdfAlterado(idTarefa, nomeArquivo);
                    if (pdfAlterado)
                    {
                        // Limpar visualizações antigas se o PDF foi alterado
                        dbPlanejamento.LimparVisualizacoesTarefa(idTarefa, idFuncionarioLogado);
                        paginasVisualizadasCache.Remove(idTarefa);
                        progressoCache.Remove(idTarefa);
                    }

                    string pastaPaginas = dbPlanejamento.CriarPastaTemporaria();
                    List<string> paginas = dbPlanejamento.DividirPdfEmPaginas(caminhoPdf, pastaPaginas);

                    if (paginas.Count == 0)
                    {
                        MessageBox.Show("Não foi possível dividir o PDF em páginas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    tarefasPaginasCache[idTarefa] = paginas;

                    // Salvar metadados no banco
                    dbPlanejamento.SalvarPdfMetadata(idTarefa, nomeArquivo, paginas.Count);
                }

                // Carregar páginas visualizadas do banco se não estiverem em cache
                if (!paginasVisualizadasCache.ContainsKey(idTarefa))
                {
                    var paginasVisualizadas = dbPlanejamento.ObterPaginasVisualizadas(idTarefa, idFuncionarioLogado);
                    paginasVisualizadasCache[idTarefa] = new HashSet<int>(paginasVisualizadas);
                }

                // Carregar progresso do banco se não estiver em cache
                if (!progressoCache.ContainsKey(idTarefa))
                {
                    var progresso = dbPlanejamento.ObterProgressoLeitura(idTarefa, idFuncionarioLogado);
                    progressoCache[idTarefa] = progresso;
                }

                ExibirPdfsNoFlowLayout(tarefasPaginasCache[idTarefa], idTarefa);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar PDFs: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // Obter informações de progresso
            var progresso = progressoCache.ContainsKey(idTarefa) ? progressoCache[idTarefa] : null;
            var paginasVisualizadas = paginasVisualizadasCache.ContainsKey(idTarefa) ? paginasVisualizadasCache[idTarefa] : new HashSet<int>();

            for (int i = 0; i < caminhosArquivosPdf.Count; i++)
            {
                string caminhoPdf = caminhosArquivosPdf[i];
                int numeroPagina = i + 1;
                bool foiLida = paginasVisualizadas.Contains(numeroPagina);

                var info = new PdfPaginaInfo
                {
                    Arquivo = caminhoPdf,
                    IdTarefa = idTarefa,
                    Pagina = numeroPagina,
                    Total = caminhosArquivosPdf.Count
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
                    ForeColor = foiLida ? Color.White : SystemColors.ControlText,
                    Tag = info
                };

                btnAbrirPdf.Click += (s, e) =>
                {
                    var infoTag = (PdfPaginaInfo)((Button)s).Tag;
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

            // Adicionar barra de progresso detalhada
            if (progresso != null)
            {
                AdicionarBarraProgressoDetalhada(idTarefa, progresso);
            }
        }

        // NOVO MÉTODO - Barra de progresso melhorada
        private void AdicionarBarraProgressoDetalhada(int idTarefa, ProgressoLeitura progresso)
        {
            Panel panelProgresso = new Panel
            {
                Width = flpPDFs.Width - 20,
                Height = 80,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10, 0, 10, 10),
                Padding = new Padding(10)
            };

            // Label de progresso detalhado
            Label lblProgresso = new Label
            {
                Text = $"Progresso: {progresso.TotalPaginasVisualizadas}/{progresso.TotalPaginas} páginas ({progresso.PercentualConcluido:F1}%)",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            panelProgresso.Controls.Add(lblProgresso);

            // Status da tarefa
            string status = progresso.Concluida ? "Concluída" : "Em andamento";
            Label lblStatus = new Label
            {
                Text = $"Status: {status}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(10, 35),
                ForeColor = progresso.Concluida ? Color.Green : Color.Blue
            };
            panelProgresso.Controls.Add(lblStatus);

            // Barra de progresso visual
            if (progresso.TotalPaginas > 0)
            {
                int larguraBarra = panelProgresso.Width - 30;
                Panel panelBarraFundo = new Panel
                {
                    Width = larguraBarra,
                    Height = 15,
                    BackColor = Color.LightGray,
                    Location = new Point(10, 55)
                };

                int larguraPreenchimento = (int)(larguraBarra * ((double)progresso.TotalPaginasVisualizadas / progresso.TotalPaginas));
                Panel panelBarraPreenchida = new Panel
                {
                    Width = larguraPreenchimento,
                    Height = 15,
                    BackColor = progresso.Concluida ? Color.Green : Color.Blue,
                    Location = new Point(0, 0)
                };

                panelBarraFundo.Controls.Add(panelBarraPreenchida);
                panelProgresso.Controls.Add(panelBarraFundo);
            }

            flpPDFs.Controls.Add(panelProgresso);
        }

        // MÉTODO COMPLETAMENTE REFEITO
        private void AbrirPdfExternamente(string caminhoPdf, int idTarefa, int numeroPagina, int totalPaginas)
        {
            try
            {
                if (File.Exists(caminhoPdf))
                {
                    // Registrar apenas a página específica que foi aberta
                    dbPlanejamento.RegistrarVisualizacaoPagina(idTarefa, idFuncionarioLogado, numeroPagina);

                    // Atualizar cache local
                    if (!paginasVisualizadasCache.ContainsKey(idTarefa))
                    {
                        paginasVisualizadasCache[idTarefa] = new HashSet<int>();
                    }
                    paginasVisualizadasCache[idTarefa].Add(numeroPagina);

                    // Atualizar progresso no cache
                    if (progressoCache.ContainsKey(idTarefa))
                    {
                        var progresso = progressoCache[idTarefa];
                        progresso.TotalPaginasVisualizadas = paginasVisualizadasCache[idTarefa].Count;
                        progresso.PercentualConcluido = (decimal)progresso.TotalPaginasVisualizadas / progresso.TotalPaginas * 100;
                        progresso.Concluida = progresso.TotalPaginasVisualizadas >= progresso.TotalPaginas;
                        progresso.DataUltimaAtualizacao = DateTime.Now;
                    }
                    else
                    {
                        // Recarregar do banco se não estiver em cache
                        var progresso = dbPlanejamento.ObterProgressoLeitura(idTarefa, idFuncionarioLogado);
                        progressoCache[idTarefa] = progresso;
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

        // MÉTODO MODIFICADO para usar o novo sistema de progresso
        private void AtualizarPainelTarefas()
        {
            // Limpar os painéis
            flpP.Controls.Clear();
            flpF.Controls.Clear();
            flpC.Controls.Clear();

            // Limpar caches de progresso para forçar recarregamento
            progressoCache.Clear();

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

        // ==================================================
        // MÉTODOS AUXILIARES (MANTIDOS)
        // ==================================================

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

        // ==================================================
        // MÉTODOS DE NAVEGAÇÃO (MANTIDOS - SEM ALTERAÇÕES)
        // ==================================================

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

        private void flpPDFs_Paint(object sender, PaintEventArgs e) { }

        private void panelKBS_Paint(object sender, PaintEventArgs e) { }

        private void flpP_Paint(object sender, PaintEventArgs e) { }

        private void flpF_Paint(object sender, PaintEventArgs e) { }

        private void flpC_Paint(object sender, PaintEventArgs e) { }

        private void Tarefa1_Enter(object sender, EventArgs e) { }

        private void txtPesquisaTarefa_TextChanged(object sender, EventArgs e) { }

        private void groupBox5_Enter(object sender, EventArgs e) { }

        private void panelTarefas_Paint(object sender, PaintEventArgs e) { }

        private void Tarefas_Pendentes_Load(object sender, EventArgs e) { }

        private void pictureBox2_Click(object sender, EventArgs e) { }

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
    }

    // CLASSE AUXILIAR PARA INFO DE PÁGINA PDF

    public class PdfPaginaInfo
    {
        public string Arquivo { get; set; }
        public int IdTarefa { get; set; }
        public int Pagina { get; set; }
        public int Total { get; set; }
    }
}
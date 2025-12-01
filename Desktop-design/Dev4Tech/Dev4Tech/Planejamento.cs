using MigraDoc.Rendering;
using OfficeConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Planejamento : Form
    {
        private planejamentoSQL dbPlanejamento = new planejamentoSQL();
        private Dictionary<int, List<string>> tarefasPaginasCache = new Dictionary<int, List<string>>();
        private Dictionary<int, HashSet<int>> paginasVisualizadasCache = new Dictionary<int, HashSet<int>>();
        private Dictionary<int, ProgressoLeitura> progressoCache = new Dictionary<int, ProgressoLeitura>();
        private int idFuncionarioLogado;
        private HashSet<int> tarefasVisualizadas = new HashSet<int>();
        private string nomeEquipeSelecionada = "";
        private string CategoriaEquipeSelecionada = "";
        private int idEquipeSelecionada = 0;

        public Planejamento()
        {
            InitializeComponent();
            this.Load += Planejamento_Load;
            CarregarFotoUsuario();

        nomeEquipeSelecionada = Sessao.NomeEquipeSelecionada;
        CategoriaEquipeSelecionada = Sessao.CategoriaEquipeSelecionada;
            lblNomeEquipe.Text = nomeEquipeSelecionada;
            lblCategoriaEquipe.Text = CategoriaEquipeSelecionada;




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

                // Limpar controles e caches
                flpP.Controls.Clear();
                flpF.Controls.Clear();
                flpC.Controls.Clear();
                flpPDFs.Controls.Clear();
                paginasVisualizadasCache.Clear();
                progressoCache.Clear();

                // ALTERAÇÃO PRINCIPAL: usar método que traz todas as tarefas, com e sem arquivo
                var tarefas = dbPlanejamento.ObterTodasTarefasPendentesPorEquipes(idsEquipes);

                if (tarefas == null)
                {
                    MessageBox.Show("Erro ao carregar tarefas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Exibir todas tarefas
                foreach (var tarefa in tarefas)
                {
                    DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(tarefa.IdTarefa);
                    string statusTarefa = ObterStatusTarefa(tarefa.IdTarefa);
                    List<Image> avatares = dbPlanejamento.ObterAvataresPorTarefa(tarefa.IdTarefa);

                    Panel card = CriarCardTarefa(tarefa.NomeTarefa, dataEntrega, avatares, statusTarefa, tarefa.IdTarefa);
                    card.Tag = tarefa.IdTarefa;

                    // CORREÇÃO: Capturar a tarefa atual em uma variável local para evitar problema de closure
                    int idTarefaAtual = tarefa.IdTarefa;
                    string nomeArquivoAtual = tarefa.NomeArquivo;
                    string nomeTarefaAtual = tarefa.NomeTarefa;

                    // Evento de clique conforme disponibilidade do arquivo
                    card.Click += (senderCard, eCard) =>
                    {
                        // Verificar se o clique foi no botão de enviar
                        if (eCard is MouseEventArgs mouseArgs)
                        {
                            var pos = mouseArgs.Location;
                            var btnEnviar = card.Controls.OfType<Button>().FirstOrDefault();

                            if (btnEnviar != null && btnEnviar.Bounds.Contains(pos))
                            {
                                return; // Não fazer nada - o clique será tratado pelo botão
                            }
                        }

                        if (!string.IsNullOrEmpty(nomeArquivoAtual))
                        {
                            CarregarPdfDaTarefa(idTarefaAtual);
                        }
                        else
                        {
                            // Para tarefas sem arquivo, mostra instruções
                            string instrucoes = dbPlanejamento.ObterInstrucoesTarefa(idTarefaAtual);

                            MessageBox.Show(
                                string.IsNullOrWhiteSpace(instrucoes) ?
                                    "Nenhuma instrução cadastrada para esta tarefa." :
                                    instrucoes,
                                $"Instruções: {nomeTarefaAtual}",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            // Marcar como visualizada
                            if (!tarefasVisualizadas.Contains(idTarefaAtual))
                            {
                                tarefasVisualizadas.Add(idTarefaAtual);
                                AtualizarPainelTarefas();
                            }
                        }
                    };

                    // Adiciona ao painel conforme status
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
            // VERIFICA PRIMEIRO SE A TAREFA JÁ FOI ENTREGUE - isso é o que define "Concluida"
            if (dbPlanejamento.FuncionarioEntregou(idTarefa, idFuncionarioLogado))
                return "Concluida";

            // Para tarefas COM arquivo: verifica se está em progresso
            if (!progressoCache.ContainsKey(idTarefa))
            {
                var progresso = dbPlanejamento.ObterProgressoLeitura(idTarefa, idFuncionarioLogado);
                progressoCache[idTarefa] = progresso;
            }
            var progressoAtual = progressoCache[idTarefa];

            // Tarefas COM arquivo: se já começou a ler, marca como "Fazendo"
            if (progressoAtual != null && progressoAtual.TotalPaginasVisualizadas > 0)
            {
                // SE TODAS AS PÁGINAS FORAM VISUALIZADAS, MARCA COMO "Concluida"
                if (progressoAtual.Concluida)
                    return "Concluida";

                return "Fazendo";
            }

            // Tarefas SEM arquivo: se já foi visualizada, marca como "Fazendo"
            if (tarefasVisualizadas.Contains(idTarefa))
                return "Fazendo";

            // Caso contrário, permanece como "Pendente"
            return "Pendente";
        }

        private Panel CriarCardTarefa(string titulo, DateTime dataEntrega, List<Image> avatares, string status, int idTarefa)
        {
            Panel card = new Panel
            {
                Width = flpP.Width - 25,
                Height = 90,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Padding = new Padding(8),
                BackColor = GetCorStatus(status),
                Cursor = Cursors.Hand,
                Tag = idTarefa // Garantir que o ID está no Tag
            };

            // Badge de status
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

            // ADICIONAR: Botão "Enviar Tarefa" para tarefas em progresso
            if (status == "Fazendo" || (status == "Concluida" && dbPlanejamento.TarefaTemArquivo(idTarefa)))
            {
                Button btnEnviar = new Button
                {
                    Text = status == "Concluida" ? "📤 Entregar" : "📤 Enviar",
                    Size = new Size(80, 25),
                    Location = new Point(card.Width - 85, 25),
                    Font = new Font("Segoe UI", 7, FontStyle.Bold),
                    BackColor = status == "Concluida" ? Color.FromArgb(46, 204, 113) : Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Tag = idTarefa // Armazenar o ID da tarefa no botão
                };


                // Estilizar o botão
                btnEnviar.FlatAppearance.BorderSize = 0;
                btnEnviar.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 174, 96);
                btnEnviar.FlatAppearance.MouseDownBackColor = Color.FromArgb(33, 145, 80);

                // Evento de clique do botão
                btnEnviar.Click += (sender, e) =>
                {
                    int tarefaId = (int)((Button)sender).Tag;
                    AbrirTelaEnvioTarefa(tarefaId);
                };

                // Prevenir que o clique no botão dispare o evento do card
                btnEnviar.MouseDown += (sender, e) => e = e;

                card.Controls.Add(btnEnviar);
            }

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
                if (!tarefasPaginasCache.ContainsKey(idTarefa))
                {
                    string nomeArquivo = dbPlanejamento.ObterNomeArquivoTarefa(idTarefa);

                    if (string.IsNullOrEmpty(nomeArquivo))
                    {
                        ExibirMensagemBonita("📄 Esta tarefa não possui um arquivo PDF associado.", "Tarefa Sem PDF", Color.LightBlue);
                        return;
                    }

                    if (!dbPlanejamento.VerificarPastaArquivos())
                    {
                        ExibirMensagemBonita("⚠️ Pasta de arquivos não encontrada.", "Erro de Configuração", Color.LightGoldenrodYellow);
                        return;
                    }

                    string caminhoPdf = dbPlanejamento.ObterCaminhoCompletoPdf(nomeArquivo);

                    if (!File.Exists(caminhoPdf))
                    {
                        ExibirMensagemBonita("❌ O arquivo PDF não foi encontrado no servidor.", "Arquivo Não Encontrado", Color.LightCoral);
                        return;
                    }

                    bool pdfAlterado = dbPlanejamento.VerificarPdfAlterado(idTarefa, nomeArquivo);
                    if (pdfAlterado)
                    {
                        dbPlanejamento.LimparVisualizacoesTarefa(idTarefa, idFuncionarioLogado);
                        paginasVisualizadasCache.Remove(idTarefa);
                        progressoCache.Remove(idTarefa);
                    }

                    string pastaPaginas = dbPlanejamento.CriarPastaTemporaria();
                    List<string> paginas = dbPlanejamento.DividirPdfEmPaginas(caminhoPdf, pastaPaginas);

                    if (paginas.Count == 0)
                    {
                        ExibirMensagemBonita("💥 Não foi possível dividir o PDF em páginas.", "Erro ao Processar", Color.LightCoral);
                        return;
                    }

                    tarefasPaginasCache[idTarefa] = paginas;
                    dbPlanejamento.SalvarPdfMetadata(idTarefa, nomeArquivo, paginas.Count);
                }

                if (!paginasVisualizadasCache.ContainsKey(idTarefa))
                {
                    var paginasVisualizadas = dbPlanejamento.ObterPaginasVisualizadas(idTarefa, idFuncionarioLogado);
                    paginasVisualizadasCache[idTarefa] = new HashSet<int>(paginasVisualizadas);
                }

                if (!progressoCache.ContainsKey(idTarefa))
                {
                    var progresso = dbPlanejamento.ObterProgressoLeitura(idTarefa, idFuncionarioLogado);
                    progressoCache[idTarefa] = progresso;
                }

                ExibirPdfsNoFlowLayout(tarefasPaginasCache[idTarefa], idTarefa);
            }
            catch (Exception ex)
            {
                ExibirMensagemBonita($"💥 Ocorreu um erro inesperado:\n{ex.Message}", "Erro", Color.LightCoral);
            }
        }

        private void ExibirMensagemBonita(string mensagem, string titulo, Color corFundo)
        {
            flpPDFs.Controls.Clear();

            Panel panelMensagem = new Panel
            {
                Width = flpPDFs.Width - 40,
                Height = 150,
                BackColor = corFundo,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20),
                Padding = new Padding(20)
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                AutoSize = true,
                Location = new Point(0, 10),
                Width = panelMensagem.Width,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblMensagem = new Label
            {
                Text = mensagem,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DimGray,
                AutoSize = false,
                Width = panelMensagem.Width - 10,
                Height = 80,
                Location = new Point(5, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelMensagem.Controls.Add(lblTitulo);
            panelMensagem.Controls.Add(lblMensagem);
            flpPDFs.Controls.Add(panelMensagem);
        }

        private void ExibirPdfsNoFlowLayout(List<string> caminhosArquivosPdf, int idTarefa)
        {
            flpPDFs.Controls.Clear();
            flpPDFs.AutoScroll = true; // Garantir que scrolling funcione

            if (!caminhosArquivosPdf.Any())
            {
                ExibirMensagemBonita("📄 Nenhuma página PDF disponível", "PDF Vazio", Color.LightGray);
                return;
            }

            var progresso = progressoCache.ContainsKey(idTarefa) ? progressoCache[idTarefa] : null;
            var paginasVisualizadas = paginasVisualizadasCache.ContainsKey(idTarefa) ? paginasVisualizadasCache[idTarefa] : new HashSet<int>();

            // Cabeçalho - SEMPRE primeiro
            Panel panelCabecalho = new Panel
            {
                Width = flpPDFs.Width - 25,
                Height = 60,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10, 10, 10, 5), // Margem inferior reduzida
                Padding = new Padding(15)
            };

            Label lblTitulo = new Label
            {
                Text = "📖 Páginas do Documento",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, 15)
            };
            panelCabecalho.Controls.Add(lblTitulo);

            Label lblInfo = new Label
            {
                Text = progresso != null ?
                       $"{progresso.TotalPaginas} páginas disponíveis • {progresso.TotalPaginasVisualizadas} visualizadas" :
                       "Carregando informações...",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Location = new Point(10, 35)
            };
            panelCabecalho.Controls.Add(lblInfo);

            flpPDFs.Controls.Add(panelCabecalho);

            // Container para os cards das páginas - SEM ALTURA FIXA
            FlowLayoutPanel containerPaginas = new FlowLayoutPanel
            {
                Width = flpPDFs.Width - 25,
                AutoSize = true, // IMPORTANTE: Deixa o container expandir conforme o conteúdo
                WrapContents = true,
                Margin = new Padding(10, 5, 10, 5), // Margens verticais reduzidas
                Padding = new Padding(15),
                BackColor = Color.White,
                AutoScroll = false // Container não deve ter scroll próprio
            };

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

                Panel cartao = CriarCartaoPagina(info, foiLida, 180);
                containerPaginas.Controls.Add(cartao);
            }

            flpPDFs.Controls.Add(containerPaginas);

            // Barra de progresso - SEMPRE depois do container de páginas
            if (progresso != null)
            {
                AdicionarBarraProgressoDetalhada(idTarefa, progresso);
            }
        }

        private Panel CriarCartaoPagina(PdfPaginaInfo info, bool foiLida, int larguraCard)
        {
            Panel cartao = new Panel
            {
                Width = larguraCard,
                Height = 180,
                Margin = new Padding(10, 10, 10, 15),
                BackColor = foiLida ? Color.FromArgb(235, 255, 235) : Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = info
            };

            // Efeito visual melhorado
            cartao.Paint += (sender, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, cartao.Width - 1, cartao.Height - 1);
                }

                // Gradiente sutil
                using (var brush = new LinearGradientBrush(
                    cartao.ClientRectangle,
                    foiLida ? Color.FromArgb(245, 255, 245) : Color.FromArgb(250, 250, 250),
                    foiLida ? Color.FromArgb(225, 255, 225) : Color.White,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, cartao.ClientRectangle);
                }
            };

            // Ícone do documento - tamanho adequado
            PictureBox icone = new PictureBox
            {
                Image = foiLida ? Properties.Resources.icon_documento : Properties.Resources.icon_documento_blue,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 80,
                Height = 80,
                Location = new Point((cartao.Width - 80) / 2, 15),
                BackColor = Color.Transparent
            };

            // Container do conteúdo
            Panel conteudo = new Panel
            {
                Location = new Point(5, 100),
                Size = new Size(cartao.Width - 10, 70),
                BackColor = Color.Transparent
            };

            // Número da página
            Label lblPagina = new Label
            {
                Text = $"Página {info.Pagina}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = foiLida ? Color.Green : Color.FromArgb(0, 123, 255),
                Size = new Size(conteudo.Width, 25),
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Status
            Label lblStatus = new Label
            {
                Text = foiLida ? "✅ Visualizada" : "📖 Não visualizada",
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = foiLida ? Color.Green : Color.Gray,
                Size = new Size(conteudo.Width, 20),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Informação adicional
            Label lblInfo = new Label
            {
                Text = $"{info.Pagina} de {info.Total}",
                Font = new Font("Segoe UI", 7, FontStyle.Italic),
                ForeColor = Color.DarkGray,
                Size = new Size(conteudo.Width, 15),
                Location = new Point(0, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Badge de conclusão
            if (foiLida)
            {
                Panel badge = new Panel
                {
                    Size = new Size(24, 24),
                    Location = new Point(cartao.Width - 30, 10),
                    BackColor = Color.Green
                };
                badge.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(Brushes.Green, 0, 0, badge.Width, badge.Height);
                    e.Graphics.DrawString("✓", new Font("Segoe UI", 10, FontStyle.Bold),
                        Brushes.White, new PointF(6, 4));
                };
                cartao.Controls.Add(badge);
            }

            // Montar hierarquia
            conteudo.Controls.Add(lblPagina);
            conteudo.Controls.Add(lblStatus);
            conteudo.Controls.Add(lblInfo);
            cartao.Controls.Add(icone);
            cartao.Controls.Add(conteudo);

            // Eventos de clique
            EventHandler clickHandler = (s, e) => AbrirPdfExternamente(info.Arquivo, info.IdTarefa, info.Pagina, info.Total);
            cartao.Click += clickHandler;
            icone.Click += clickHandler;
            conteudo.Click += clickHandler;
            foreach (Control control in conteudo.Controls)
            {
                control.Click += clickHandler;
                control.Cursor = Cursors.Hand;
            }

            return cartao;
        }

        private void AdicionarBarraProgressoDetalhada(int idTarefa, ProgressoLeitura progresso)
        {
            Panel panelProgresso = new Panel
            {
                Width = flpPDFs.Width - 40,
                Height = 100,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(20, 10, 20, 20), // Margem inferior aumentada
                Padding = new Padding(15)
            };

            // Título
            Label lblTitulo = new Label
            {
                Text = "📊 Progresso de Leitura",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            panelProgresso.Controls.Add(lblTitulo);

            // Estatísticas
            string textoProgresso = $"{progresso.TotalPaginasVisualizadas} de {progresso.TotalPaginas} páginas ({progresso.PercentualConcluido:F1}%)";
            Label lblEstatisticas = new Label
            {
                Text = textoProgresso,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Location = new Point(0, 25)
            };
            panelProgresso.Controls.Add(lblEstatisticas);

            // Barra de progresso
            if (progresso.TotalPaginas > 0)
            {
                int larguraBarra = panelProgresso.Width - 30;

                // Fundo da barra
                Panel fundoBarra = new Panel
                {
                    Width = larguraBarra,
                    Height = 15,
                    BackColor = Color.LightGray,
                    Location = new Point(0, 50)
                };

                // Progresso
                int larguraPreenchida = (int)(larguraBarra * ((double)progresso.TotalPaginasVisualizadas / progresso.TotalPaginas));
                Panel barraPreenchida = new Panel
                {
                    Width = larguraPreenchida,
                    Height = 15,
                    BackColor = progresso.Concluida ? Color.Green : Color.Blue,
                    Location = new Point(0, 0)
                };

                fundoBarra.Controls.Add(barraPreenchida);
                panelProgresso.Controls.Add(fundoBarra);
            }

            // ADICIONAR ao flpPDFs (não esquecer esta linha!)
            flpPDFs.Controls.Add(panelProgresso);
        }

        private void AbrirPdfExternamente(string caminhoPdf, int idTarefa, int numeroPagina, int totalPaginas)
        {
            try
            {
                if (File.Exists(caminhoPdf))
                {
                    dbPlanejamento.RegistrarVisualizacaoPagina(idTarefa, idFuncionarioLogado, numeroPagina);

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

                        // ATUALIZAR A INTERFACE SE TODAS AS PÁGINAS FORAM VISUALIZADAS
                        if (progresso.Concluida)
                        {
                            AtualizarPainelTarefas();
                        }
                    }

                    // Abrir o PDF
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = caminhoPdf,
                        UseShellExecute = true
                    });

                    ExibirPdfsNoFlowLayout(tarefasPaginasCache[idTarefa], idTarefa);
                    AtualizarPainelTarefas(); // Atualizar sempre para refletir mudanças de status
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
            // LIMPAR CACHE DE PROGRESSO PARA FORÇAR ATUALIZAÇÃO
            progressoCache.Clear();

            flpP.Controls.Clear();
            flpF.Controls.Clear();
            flpC.Controls.Clear();


            var funcionario = Sessao.FuncionarioLogado;
            if (funcionario == null) return;

            int idFuncionario = int.Parse(funcionario.getFuncionarioId());
            var idsEquipes = dbPlanejamento.ObterIdsEquipesFuncionario(idFuncionario);
            var tarefas = dbPlanejamento.ObterTodasTarefasPendentesPorEquipes(idsEquipes);

            foreach (var tarefa in tarefas)
            {
                DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(tarefa.IdTarefa);
                string statusTarefa = ObterStatusTarefa(tarefa.IdTarefa);
                List<Image> avatares = dbPlanejamento.ObterAvataresPorTarefa(tarefa.IdTarefa);

                Panel card = CriarCardTarefa(tarefa.NomeTarefa, dataEntrega, avatares, statusTarefa, tarefa.IdTarefa);
                card.Tag = tarefa.IdTarefa;

                // CORREÇÃO: Capturar a tarefa atual em uma variável local para evitar problema de closure
                int idTarefaAtual = tarefa.IdTarefa;
                string nomeArquivoAtual = tarefa.NomeArquivo;
                string nomeTarefaAtual = tarefa.NomeTarefa;

                card.Click += (senderCard, eCard) =>
                {
                    // Verificar se o clique foi no botão de enviar
                    if (eCard is MouseEventArgs mouseArgs)
                    {
                        var pos = mouseArgs.Location;
                        var btnEnviar = card.Controls.OfType<Button>().FirstOrDefault();

                        if (btnEnviar != null && btnEnviar.Bounds.Contains(pos))
                        {
                            return; // Não fazer nada - o clique será tratado pelo botão
                        }
                    }

                    if (!string.IsNullOrEmpty(nomeArquivoAtual))
                    {
                        CarregarPdfDaTarefa(idTarefaAtual);
                    }
                    else
                    {
                        string instrucoes = dbPlanejamento.ObterInstrucoesTarefa(idTarefaAtual);
                        MessageBox.Show(
                            string.IsNullOrWhiteSpace(instrucoes) ? "Nenhuma instrução cadastrada para esta tarefa." : instrucoes,
                            "Instruções da Tarefa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                };

                switch (statusTarefa)
                {
                    case "Pendente": flpP.Controls.Add(card); break;
                    case "Fazendo": flpF.Controls.Add(card); break;
                    case "Concluida": flpC.Controls.Add(card); break;
                    default: flpP.Controls.Add(card); break;
                }
            }
        }

        private Color GetCorStatus(string status)
        {
            switch (status)
            {
                case "Fazendo": return Color.FromArgb(255, 255, 240);
                case "Concluida": return Color.FromArgb(240, 255, 240);
                default: return Color.White;
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

        private void AbrirTelaEnvioTarefa(int idTarefa)
        {
            try
            {
                // Obter informações da tarefa para mostrar na confirmação
                string nomeTarefa = ObterNomeTarefa(idTarefa);
                DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(idTarefa);

                // Obter o ID da equipe da tarefa
                int idEquipe = dbPlanejamento.ObterIdEquipeDaTarefa(idTarefa);

                if (idEquipe == 0)
                {
                    MessageBox.Show("Não foi possível identificar a equipe desta tarefa.", "Erro",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmacao = MessageBox.Show(
                    $"Deseja enviar a tarefa?\n\n" +
                    $"Tarefa: {nomeTarefa}\n" +
                    $"Data de entrega: {dataEntrega:dd/MM/yyyy}",
                    "Confirmar Envio",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacao == DialogResult.Yes)
                {
                    // Abrir a tela de tarefa para envio
                    Tela_Tarefa telaTarefa = new Tela_Tarefa(idEquipe);
                    telaTarefa.CarregarDetalhesTarefa(idTarefa);
                    telaTarefa.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir tela de envio: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObterNomeTarefa(int idTarefa)
        {
            try
            {
                // Você pode otimizar isso buscando de um cache se necessário
                var idsEquipes = dbPlanejamento.ObterIdsEquipesFuncionario(idFuncionarioLogado);
                var tarefas = dbPlanejamento.ObterTodasTarefasPendentesPorEquipes(idsEquipes);

                var tarefa = tarefas.FirstOrDefault(t => t.IdTarefa == idTarefa);
                return tarefa?.NomeTarefa ?? $"Tarefa #{idTarefa}";
            }
            catch
            {
                return $"Tarefa #{idTarefa}";
            }
        }

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
        }

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

        // CLASSE AUXILIAR PARA INFO DE PÁGINA PDF

        public class PdfPaginaInfo
        {
            public string Arquivo { get; set; }
            public int IdTarefa { get; set; }
            public int Pagina { get; set; }
            public int Total { get; set; }
        }
    }
}
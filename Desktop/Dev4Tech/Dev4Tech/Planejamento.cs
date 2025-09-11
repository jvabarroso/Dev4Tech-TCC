using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Dev4Tech
{
    public partial class Planejamento : Form
    {
        private planejamentoSQL dbPlanejamento = new planejamentoSQL();

        public Planejamento()
        {
            InitializeComponent();
            this.Load += Planejamento_Load;
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

                int idFuncionario = int.Parse(funcionario.getFuncionarioId());
                var idsEquipes = dbPlanejamento.ObterIdsEquipesFuncionario(idFuncionario);

                if (idsEquipes.Count == 0)
                {
                    MessageBox.Show("Funcionário não pertence a nenhuma equipe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                flpP.Controls.Clear();

                var tarefas = dbPlanejamento.ObterTarefasPendentesPorEquipesComArquivo(idsEquipes);
                string pastaArquivos = @"C:\Dev4Tech\ArquivosTarefas";

                foreach (var tarefa in tarefas)
                {
                    string caminhoPdf = Path.Combine(pastaArquivos, tarefa.NomeArquivo);
                    if (!File.Exists(caminhoPdf))
                        continue;

                    string pastaTemporaria = dbPlanejamento.CriarPastaTemporaria();
                    var paginasPdf = dbPlanejamento.DividirPdfEmPaginas(caminhoPdf, pastaTemporaria);

                    DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(tarefa.IdTarefa);
                    string statusTarefa = dbPlanejamento.ObterStatusTarefa(tarefa.IdTarefa);
                    List<Image> avatares = dbPlanejamento.ObterAvataresPorTarefa(tarefa.IdTarefa);

                    foreach (var paginaPdf in paginasPdf)
                    {
                        Panel card = CriarCardTarefa(
                            Path.GetFileNameWithoutExtension(paginaPdf),
                            dataEntrega,
                            avatares
                        );
                        card.Tag = paginaPdf;

                        card.Click += (senderCard, eCard) =>
                        {
                            string arquivo = ((Panel)senderCard).Tag.ToString();
                            AbrirPdfExternamente(arquivo);
                        };

                        if (statusTarefa == "Pendente")
                        {
                            flpP.Controls.Add(card);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tarefas pendentes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public class TarefaModelo
        {
            public int IdTarefa { get; set; }
            public string Titulo { get; set; }
            public DateTime DataEntrega { get; set; }
            public string Status { get; set; } 
            public List<Image> Avatares { get; set; }
        }

        // Cria cada card visual conforme design
        private Panel CriarCardTarefa(string titulo, DateTime dataEntrega, List<Image> avatares)
        {
            Panel card = new Panel
            {
                Width = flpP.Width - 25,
                Height = 90,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Padding = new Padding(8),
                BackColor = Color.White
            };

            FlowLayoutPanel membrosPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Height = 24,
                Width = card.Width - 16,
                Location = new Point(0, 0),
                WrapContents = false,
                AutoScroll = false,
            };

            foreach (var avatar in avatares)
            {
                PictureBox pic = new PictureBox
                {
                    Image = avatar,
                    Width = 24,
                    Height = 24,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = new Padding(2),
                    Cursor = Cursors.Hand
                };
                membrosPanel.Controls.Add(pic);
            }

            Label lblTitulo = new Label
            {
                Text = titulo,
                Location = new Point(0, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = false,
                Width = card.Width - 16,
                Height = 30,
                ForeColor = Color.Black
            };

            Label lblData = new Label
            {
                Text = "Até " + dataEntrega.ToString("dd/MM"),
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

        // Popular os FlowLayoutPanels das colunas do Kanban com os cards
        private void PopularKanban(List<TarefaModelo> tarefas)
        {
            flpP.Controls.Clear();
            flpF.Controls.Clear();
            flpC.Controls.Clear();

            foreach (var tarefa in tarefas)
            {
                Panel card = CriarCardTarefa(tarefa.Titulo, tarefa.DataEntrega, tarefa.Avatares);
                switch (tarefa.Status)
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

        // Exemplo simplificado para buscar tarefas do banco - substitua pela sua lógica real
        private List<TarefaModelo> BuscarTarefasDoFuncionario()
        {
            // Aqui, use dbPlanejamento para buscar tarefas pendentes, fazer map dos status, carregar imagens etc.
            // Exemplo estático só para demonstração:

            return new List<TarefaModelo>()
            {
                new TarefaModelo
                {
                    IdTarefa = 1,
                    Titulo = "Documentação da empresa",
                    DataEntrega = DateTime.Parse("2025-09-08"),
                    Status = "Pendente",
                    Avatares = new List<Image> { Properties.Resources.icon_perfil, Properties.Resources.icon_perfil }
                },
                new TarefaModelo
                {
                    IdTarefa = 2,
                    Titulo = "Documentação da empresa",
                    DataEntrega = DateTime.Parse("2025-09-08"),
                    Status = "Fazendo",
                    Avatares = new List<Image> { Properties.Resources.icon_perfil, Properties.Resources.icon_perfil }
                },
                new TarefaModelo
                {
                    IdTarefa = 3,
                    Titulo = "Documentação da empresa",
                    DataEntrega = DateTime.Parse("2025-09-08"),
                    Status = "Concluida",
                    Avatares = new List<Image> { Properties.Resources.icon_perfil, Properties.Resources.icon_perfil }
                }
            };
        }
        private void btnPendentes_Click(object sender, EventArgs e)
        {
            // Implementar filtro pendentes
        }

        private void btnEmAtraso_Click(object sender, EventArgs e)
        {
            // Implementar filtro atrasados
        }

        private void btnCompletadas_Click(object sender, EventArgs e)
        {
            // Implementar filtro completadas
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
            if (funcionario != null || admin != null)
            {
                Chat_geral_equipes t_chat = new Chat_geral_equipes();
                t_chat.Show();
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

        public void divisaoPDF(string caminhoArquivoEntrada, string pastaSaida)
        {
            PdfDocument documento = PdfReader.Open(caminhoArquivoEntrada, PdfDocumentOpenMode.Import);
            int totalPaginas = documento.PageCount;

            if (!Directory.Exists(pastaSaida))
                Directory.CreateDirectory(pastaSaida);

            for (int i = 0; i < totalPaginas; i++)
            {
                PdfDocument novoDocumento = new PdfDocument();
                novoDocumento.Version = documento.Version;
                novoDocumento.AddPage(documento.Pages[i]);
                string caminhoNovoArquivo = Path.Combine(pastaSaida, $"pagina_{i + 1}.pdf");
                novoDocumento.Save(caminhoNovoArquivo);
            }
        }

        private void flpPDFs_Paint(object sender, PaintEventArgs e)
        {
            // Se desejar, pode customizar o paint do flowlayoutpanel
        }

        private void CarregarPdfDaTarefa(int idTarefa)
        {
            try
            {
                string pastaArquivos = @"C:\Dev4Tech\ArquivosTarefas";

                // Obter nome do arquivo PDF salvo no banco para a tarefa
                string nomeArquivo = dbPlanejamento.ObterNomeArquivoTarefa(idTarefa);

                if (string.IsNullOrEmpty(nomeArquivo))
                {
                    MessageBox.Show("Nenhum arquivo PDF encontrado para essa tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string caminhoPdf = Path.Combine(pastaArquivos, nomeArquivo);

                string pastaPaginas = dbPlanejamento.CriarPastaTemporaria();

                divisaoPDF(caminhoPdf, pastaPaginas);

                List<string> arquivosPdf = Directory.GetFiles(pastaPaginas, "*.pdf").ToList();

                if (!arquivosPdf.Any())
                {
                    MessageBox.Show("Nenhum arquivo PDF encontrado para essa tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ExibirPdfsNoFlowLayout(arquivosPdf);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar PDFs: " + ex.Message);
            }
        }

        private void AbrirPdfExternamente(string caminhoPdf)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = caminhoPdf,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir PDF externo: " + ex.Message);
            }
        }

        private void ExibirPdfNoVisualizador(string caminhoPdf)
        {
            try
            {
                webBrowserPdf.Navigate("about:blank");
                webBrowserPdf.DocumentCompleted += (s, e) =>
                {
                    if (webBrowserPdf.Url.ToString() == "about:blank")
                        webBrowserPdf.Navigate(caminhoPdf);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exibir PDF: " + ex.Message);
            }
        }

        private void ExibirPdfsNoFlowLayout(List<string> caminhosArquivosPdf)
        {
            flpPDFs.Controls.Clear();

            foreach (string caminhoPdf in caminhosArquivosPdf)
            {
                Panel painelCartao = new Panel
                {
                    Width = 150,
                    Height = 200,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Tag = caminhoPdf
                };

                Button btnAbrirPdf = new Button
                {
                    Text = Path.GetFileName(caminhoPdf),
                    Dock = DockStyle.Bottom,
                    Height = 30
                };
                btnAbrirPdf.Click += (s, e) =>
                {
                    string arquivoSelecionado = ((Button)s).Parent.Tag.ToString();
                    ExibirPdfNoVisualizador(arquivoSelecionado);
                };

                PictureBox picThumbnail = new PictureBox
                {
                    Image = Properties.Resources.icon_documento_blue,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill
                };

                painelCartao.Controls.Add(picThumbnail);
                painelCartao.Controls.Add(btnAbrirPdf);

                flpPDFs.Controls.Add(painelCartao);
            }
        }

        // Exemplo de evento para carregar a tarefa ao selecionar na UI - você deve conectar conforme sua interface
        private void OnTarefaSelecionada(int idTarefaSelecionada)
        {
            CarregarPdfDaTarefa(idTarefaSelecionada);
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
    }
}

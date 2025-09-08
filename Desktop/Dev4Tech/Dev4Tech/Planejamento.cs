using System;
using System.Collections.Generic;
using System.Data;
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

        private void ExibirPdfNoVisualizador(string caminhoPdf)
        {
            webBrowserPdf.Navigate(caminhoPdf);
        }

        // Exemplo de evento para carregar a tarefa ao selecionar na UI - você deve conectar conforme sua interface
        private void OnTarefaSelecionada(int idTarefaSelecionada)
        {
            CarregarPdfDaTarefa(idTarefaSelecionada);
        }
    }
}

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
                flpPDFs.Controls.Clear();

                var tarefas = dbPlanejamento.ObterTarefasPendentesPorEquipesComArquivo(idsEquipes);

                foreach (var tarefa in tarefas)
                {
                    DateTime dataEntrega = dbPlanejamento.ObterDataEntregaTarefa(tarefa.IdTarefa);
                    string statusTarefa = dbPlanejamento.ObterStatusTarefa(tarefa.IdTarefa);
                    List<Image> avatares = dbPlanejamento.ObterAvataresPorTarefa(tarefa.IdTarefa);

                    Panel card = CriarCardTarefa(tarefa.NomeTarefa, dataEntrega, avatares);
                    card.Tag = tarefa.IdTarefa;

                    card.Click += (senderCard, eCard) =>
                    {
                        int idTarefa = (int)((Panel)senderCard).Tag;
                        CarregarPdfDaTarefa(idTarefa);
                    };

                    if (statusTarefa == "Pendente")
                    {
                        flpP.Controls.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tarefas pendentes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CriarCardTarefa(string titulo, DateTime dataEntrega, List<Image> avatares)
        {
            Panel card = new Panel
            {
                Width = flpP.Width - 25,
                Height = 90,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Padding = new Padding(8),
                BackColor = Color.White,
                Cursor = Cursors.Hand
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

            foreach (var avatar in avatares.Take(5)) // Limitar a 5 avatares
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

                    string caminhoPdf = dbPlanejamento.ObterCaminhoCompletoPdf(nomeArquivo);

                    if (!File.Exists(caminhoPdf))
                    {
                        MessageBox.Show($"Arquivo PDF não encontrado: {caminhoPdf}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string pastaPaginas = dbPlanejamento.CriarPastaTemporaria();
                    List<string> paginas = dbPlanejamento.DividirPdfEmPaginas(caminhoPdf, pastaPaginas);

                    tarefasPaginasCache[idTarefa] = paginas;
                }

                ExibirPdfsNoFlowLayout(tarefasPaginasCache[idTarefa]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar PDFs: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExibirPdfsNoFlowLayout(List<string> caminhosArquivosPdf)
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

            foreach (string caminhoPdf in caminhosArquivosPdf)
            {
                Panel painelCartao = new Panel
                {
                    Width = 150,
                    Height = 200,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Tag = caminhoPdf,
                    Cursor = Cursors.Hand
                };

                Button btnAbrirPdf = new Button
                {
                    Text = $"Página {Path.GetFileNameWithoutExtension(caminhoPdf).Replace("pagina_", "")}",
                    Dock = DockStyle.Bottom,
                    Height = 30
                };
                btnAbrirPdf.Click += (s, e) =>
                {
                    string arquivoSelecionado = ((Button)s).Parent.Tag.ToString();
                    AbrirPdfExternamente(arquivoSelecionado);
                };

                PictureBox picThumbnail = new PictureBox
                {
                    Image = Properties.Resources.icon_documento_blue,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill
                };

                // Também permitir clicar no próprio painel para abrir o PDF
                painelCartao.Click += (s, e) =>
                {
                    string arquivoSelecionado = ((Panel)s).Tag.ToString();
                    AbrirPdfExternamente(arquivoSelecionado);
                };

                painelCartao.Controls.Add(picThumbnail);
                painelCartao.Controls.Add(btnAbrirPdf);

                flpPDFs.Controls.Add(painelCartao);
            }
        }

        private void AbrirPdfExternamente(string caminhoPdf)
        {
            try
            {
                if (File.Exists(caminhoPdf))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = caminhoPdf,
                        UseShellExecute = true
                    });
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

        // Métodos da classe TarefaModelo (mantidos para compatibilidade)
        public class TarefaModelo
        {
            public int IdTarefa { get; set; }
            public string Titulo { get; set; }
            public DateTime DataEntrega { get; set; }
            public string Status { get; set; }
            public List<Image> Avatares { get; set; }
        }
    }
}
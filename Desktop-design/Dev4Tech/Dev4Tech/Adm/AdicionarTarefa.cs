using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using OfficeConverter;
using OfficeConverter;

namespace Dev4Tech
{
    public partial class AdicionarTarefa : Form
    {
        private string caminhoArquivoSelecionado = "";
        private List<int> equipesSelecionadas = new List<int>(); // Lista interna para armazenar equipes selecionadas

        public AdicionarTarefa()
        {
            InitializeComponent();

            // Carrega equipes no ComboBox
            CarregarEquipes();

            // Configura eventos dos botões
            btnAnexarArquivos.Click += BtnAnexarArquivos_Click;
            btnAddTarefas.Click += BtnAddTarefas_Click;
            btnAddEquipe.Click += btnAddEquipe_Click;

            // Inicializa comboBox de dificuldade
            cmbDificuldade.Items.AddRange(new string[] { "Fácil", "Média", "Difícil" });
            cmbDificuldade.SelectedIndex = 1; // Seleciona "Média" por padrão
        }

        // Busca equipes do banco e carrega no ComboBox
        private void CarregarEquipes()
        {
            AddTarefas tarefa = new AddTarefas();
            DataTable dt = tarefa.BuscarEquipes();

            cmbAddEquipe.DataSource = dt;
            cmbAddEquipe.DisplayMember = "nome_equipe";
            cmbAddEquipe.ValueMember = "id_equipe";
            cmbAddEquipe.SelectedIndex = -1;
        }

        // Evento para anexar arquivo
        private void BtnAnexarArquivos_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Todos os arquivos suportados (*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx)|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx|" +
                         "Arquivos PDF (*.pdf)|*.pdf|" +
                         "Documentos Word (*.doc, *.docx)|*.doc;*.docx|" +
                         "Planilhas Excel (*.xls, *.xlsx)|*.xls;*.xlsx|" +
                         "Apresentações PowerPoint (*.ppt, *.pptx)|*.ppt;*.pptx|" +
                         "Todos os arquivos (*.*)|*.*",
                Title = "Selecione o arquivo para anexar",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                caminhoArquivoSelecionado = ofd.FileName;
                lblArquivosSelecionado.Text = Path.GetFileName(caminhoArquivoSelecionado);
            }
        }

        // Evento para adicionar tarefa no banco para todas as equipes selecionadas
        private void BtnAddTarefas_Click(object sender, EventArgs e)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(txtInstruções.Text))
            {
                MessageBox.Show("Por favor, preencha as instruções da tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNomeTarefa.Text))
            {
                MessageBox.Show("Por favor, preencha o nome da tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (equipesSelecionadas.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos uma equipe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtpDataDeEntrega.Value.Date < DateTime.Today)
            {
                MessageBox.Show("A data de entrega deve ser hoje ou uma data futura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbDificuldade.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione a dificuldade da tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dados coletados do formulário
            string nomeTarefa = txtNomeTarefa.Text.Trim();
            string instrucoes = txtInstruções.Text.Trim();
            string dificuldade = cmbDificuldade.SelectedItem.ToString();
            DateTime dataEntrega = dtpDataDeEntrega.Value.Date;

            // Define a pasta de arquivos correta
            string pastaArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";
            if (!Directory.Exists(pastaArquivos))
                Directory.CreateDirectory(pastaArquivos);

            string nomeArquivo = "";

            // Salvar arquivo PDF na pasta e pegar nome único
            if (!string.IsNullOrEmpty(caminhoArquivoSelecionado))
            {
                try
                {
                    string extensao = Path.GetExtension(caminhoArquivoSelecionado).ToLower();
                    string nomeArquivoUnico = Guid.NewGuid().ToString();

                    // Se for PDF, copia diretamente
                    if (extensao == ".pdf")
                    {
                        nomeArquivo = nomeArquivoUnico + ".pdf";
                        string caminhoCompleto = Path.Combine(pastaArquivos, nomeArquivo);
                        File.Copy(caminhoArquivoSelecionado, caminhoCompleto, overwrite: true);
                    }
                    else
                    {
                        // Para outros formatos (Word, Excel, etc.), converte para PDF
                        nomeArquivo = nomeArquivoUnico + ".pdf";
                        string caminhoCompleto = Path.Combine(pastaArquivos, nomeArquivo);

                        // Usa OfficeConverter para converter o arquivo
                        using (var converter = new Converter())
                        {
                            converter.Convert(caminhoArquivoSelecionado, caminhoCompleto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao processar arquivo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Insere tarefa para cada equipe selecionada
            foreach (int idEquipe in equipesSelecionadas)
            {
                AddTarefas tarefa = new AddTarefas
                {
                    NomeTarefa = nomeTarefa,
                    Instrucoes = instrucoes,
                    Dificuldade = dificuldade,
                    IdEquipe = idEquipe,
                    DataEntrega = dataEntrega,
                    NomeArquivo = nomeArquivo,
                    ArquivoBlob = null // não salva blob, arquivo fica na pasta
                };
                try
                {
                    tarefa.IdEmpresa = Convert.ToInt32(Sessao.AdminLogado.getIdEmpresa());
                    tarefa.Inserir();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao adicionar tarefa para equipe ID {idEquipe}: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            MessageBox.Show("Tarefas adicionadas com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparFormulario();
        }


        // Limpa campos após inserção
        private void LimparFormulario()
        {
            txtInstruções.Clear();
            txtNomeTarefa.Clear();
            equipesSelecionadas.Clear();
            cmbAddEquipe.SelectedIndex = -1;
            cmbDificuldade.SelectedIndex = 1;
            dtpDataDeEntrega.Value = DateTime.Today;
            caminhoArquivoSelecionado = "";
            lblArquivosSelecionado.Text = "Nenhum arquivo selecionado";
        }

        // Eventos mantidos
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
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
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
            PesquisaEquipes equip_e = new PesquisaEquipes();
            equip_e.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rk = new Ranking_Equipes();
            rk.Show();
            this.Hide();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {

            var admin = Sessao.AdminLogado;
            var funcionario = Sessao.FuncionarioLogado;

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
                {
                    MessageBox.Show("Nenhum funcionário logado.");
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpa a sessão antes de voltar para a tela inicial
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;

            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void AdicionarTarefa_Load(object sender, EventArgs e)
        {

        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            AdicionarTarefa  t_completadas = new AdicionarTarefa();
            t_completadas.Show();
            this.Hide();
        }

        private void btnAnexarArquivos_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAddEquipe_Click(object sender, EventArgs e)
        {
            if (cmbAddEquipe.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma equipe para adicionar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEquipe = Convert.ToInt32(cmbAddEquipe.SelectedValue);

            if (!equipesSelecionadas.Contains(idEquipe))
            {
                equipesSelecionadas.Add(idEquipe);
                MessageBox.Show("Equipe selecionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Essa equipe já foi selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }
    }
}

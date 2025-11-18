using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeConverter;

namespace Dev4Tech
{
    public partial class AdicionarTarefa : Form
    {
        private string caminhoArquivoSelecionado = "";
        private List<int> equipesSelecionadas = new List<int>(); // Lista interna para armazenar equipes selecionadas
        private Timer timerAtualizaData;

        public AdicionarTarefa()
        {
            InitializeComponent();

            // Carrega equipes no ComboBox
            CarregarEquipes();


            timerAtualizaData = new Timer();
            timerAtualizaData.Interval = 60000; // 60.000 milissegundos (1 minuto)
            timerAtualizaData.Tick += TimerAtualizaData_Tick;
            timerAtualizaData.Start();


            // Configura eventos dos botões
            
            

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
                Filter = "Todos os arquivos suportados (*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx;*.txt;*.jpg;*.png)|" +
                         "*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx;*.txt;*.jpg;*.jpeg;*.png;*.bmp|" +
                         "Arquivos PDF (*.pdf)|*.pdf|" +
                         "Documentos Word (*.doc, *.docx)|*.doc;*.docx|" +
                         "Planilhas Excel (*.xls, *.xlsx)|*.xls;*.xlsx|" +
                         "Apresentações (*.ppt, *.pptx)|*.ppt;*.pptx|" +
                         "Arquivos de Texto (*.txt)|*.txt|" +
                         "Imagens (*.jpg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|" +
                         "Todos os arquivos (*.*)|*.*",
                Title = "Selecione o arquivo para anexar",
                Multiselect = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                caminhoArquivoSelecionado = ofd.FileName;
                lblArquivosSelecionado.Text = Path.GetFileName(caminhoArquivoSelecionado);

                // Validação de tamanho de arquivo
                FileInfo fileInfo = new FileInfo(caminhoArquivoSelecionado);
                if (fileInfo.Length > 100 * 1024 * 1024) // 100MB
                {
                    MessageBox.Show("O arquivo é muito grande. Por favor, selecione um arquivo menor que 100MB.",
                        "Arquivo Grande", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    caminhoArquivoSelecionado = "";
                    lblArquivosSelecionado.Text = "Nenhum arquivo selecionado";
                }
                else
                {
                    // Mostrar informações do arquivo
                    string extensao = Path.GetExtension(caminhoArquivoSelecionado).ToLower();
                    string[] formatosSuportados = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".jpg", ".jpeg", ".png", ".bmp" };

                    if (Array.IndexOf(formatosSuportados, extensao) == -1)
                    {
                        MessageBox.Show($"O formato {extensao} pode não ser suportado.\n\nFormatos suportados: PDF, Word, Excel, PowerPoint, Texto, Imagens",
                            "Formato Não Testado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void TimerAtualizaData_Tick(object sender, EventArgs e)
        {
            // Atualizar o DateTimePicker para a data e hora atual do sistema
            dtpDataDeEntrega.Value = DateTime.Today;
        }

        // Evento para adicionar tarefa no banco para todas as equipes selecionadas
        private async void BtnAddTarefas_Click(object sender, EventArgs e)
        {
            // Mostrar cursor de espera
            this.Cursor = Cursors.WaitCursor;
            btnAddTarefas.Enabled = false;

            try
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
                int idEmpresa = Convert.ToInt32(Sessao.AdminLogado.getIdEmpresa());

                // Define a pasta de arquivos correta
                string pastaArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";
                if (!Directory.Exists(pastaArquivos))
                    Directory.CreateDirectory(pastaArquivos);

                string nomeArquivoComHash = ""; // ✅ Variável para armazenar o nome com hash

                // Processar arquivo anexado
                if (!string.IsNullOrEmpty(caminhoArquivoSelecionado))
                {
                    try
                    {
                        // Mostrar progresso
                        lblArquivosSelecionado.Text = "🔄 Convertendo arquivo...";
                        lblArquivosSelecionado.ForeColor = Color.Blue;
                        Application.DoEvents();

                        var conversorPython = new PythonExecutor();

                        if (!conversorPython.VerificarPython())
                        {
                            var resultado = MessageBox.Show(
                                "API de conversão não está disponível.\n\nDeseja continuar sem converter o arquivo?",
                                "API Não Disponível",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (resultado == DialogResult.Yes)
                            {
                                nomeArquivoComHash = ""; // Continuar sem arquivo
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            // ✅ O PythonExecutor agora SEMPRE gera hash automaticamente
                            nomeArquivoComHash = await conversorPython.ConverterParaPdfAsync(caminhoArquivoSelecionado, pastaArquivos);

                            if (!string.IsNullOrEmpty(nomeArquivoComHash))
                            {
                                lblArquivosSelecionado.Text = "✅ Arquivo convertido!";
                                lblArquivosSelecionado.ForeColor = Color.Green;
                                await Task.Delay(500);
                            }
                            else
                            {
                                throw new Exception("Conversão retornou nome vazio");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblArquivosSelecionado.Text = "❌ Erro na conversão";
                        lblArquivosSelecionado.ForeColor = Color.Red;

                        DialogResult resultado = MessageBox.Show(
                            $"Erro ao converter arquivo: {ex.Message}\n\nDeseja continuar sem anexar o arquivo?",
                            "Erro de Conversão",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (resultado == DialogResult.No)
                        {
                            return;
                        }
                        nomeArquivoComHash = "";
                    }
                }

                // ✅ INSERIR TAREFAS EM LOTE (CORRIGINDO DUPLICAÇÃO)
                AddTarefas tarefaManager = new AddTarefas();
                bool sucesso = tarefaManager.InserirTarefasEmLote(
                    new List<int>(equipesSelecionadas),
                    nomeTarefa,
                    instrucoes,
                    dificuldade,
                    dataEntrega,
                    nomeArquivoComHash, // ✅ Agora com hash no nome do arquivo
                    idEmpresa
                );

                // Mostrar resultado final
                if (sucesso)
                {
                    string mensagemSucesso = $"{equipesSelecionadas.Count} tarefa(s) adicionada(s) com sucesso!";

                    if (!string.IsNullOrEmpty(nomeArquivoComHash))
                    {
                        mensagemSucesso += $"\n\nArquivo salvo com nome seguro: {nomeArquivoComHash}";
                    }

                    MessageBox.Show(mensagemSucesso, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimparFormulario();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restaurar cursor e botão
                this.Cursor = Cursors.Default;
                btnAddTarefas.Enabled = true;
                lblArquivosSelecionado.Text = string.IsNullOrEmpty(caminhoArquivoSelecionado) ?
                    "Nenhum arquivo selecionado" : Path.GetFileName(caminhoArquivoSelecionado);
                lblArquivosSelecionado.ForeColor = Color.Black;
            }
        }


        // Limpa campos após inserção
        private void LimparFormulario()
        {
            try
            {
                txtInstruções.Clear();
                txtNomeTarefa.Clear();
                equipesSelecionadas.Clear();
                cmbAddEquipe.SelectedIndex = -1;
                cmbDificuldade.SelectedIndex = 1;
                dtpDataDeEntrega.Value = DateTime.Today;
                caminhoArquivoSelecionado = "";
                lblArquivosSelecionado.Text = "Nenhum arquivo selecionado";
                lblArquivosSelecionado.ForeColor = Color.Black;

                if (cmbAddEquipe.Items.Count > 0)
                {
                    cmbAddEquipe.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao limpar formulário: {ex.Message}");
            }
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
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
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

        private void btnAnexarArquivos_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAddEquipe_Click(object sender, EventArgs e)
        {
            btnAddEquipe.Enabled = false; // bloqueia clique repetido enquanto executa

            try
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
            finally
            {
                btnAddEquipe.Enabled = true; // reabilita o botão
            }
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void dtpDataDeEntrega_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class AdicionarTarefa : Form
    {
        // Guarda o caminho do arquivo selecionado
        private string caminhoArquivoSelecionado = "";

        public AdicionarTarefa()
        {
            InitializeComponent();

            // Carrega as equipes no ComboBox ao iniciar a tela
            CarregarEquipes();

            // Configura eventos dos botões
            btnAnexarArquivos.Click += BtnAnexarArquivos_Click;
            btnAddTarefas.Click += BtnAddTarefas_Click;
        }

        // Carrega equipes para o ComboBox cmbAddEquipe buscando do banco
        private void CarregarEquipes()
        {
            AddTarefas tarefa = new AddTarefas();
            DataTable dt = tarefa.BuscarEquipes();

            cmbAddEquipe.DataSource = dt;
            cmbAddEquipe.DisplayMember = "nome_equipe";
            cmbAddEquipe.ValueMember = "id_equipe";
            cmbAddEquipe.SelectedIndex = -1; // Nenhuma selecionada inicialmente
        }

        // Busca as equipes reais do banco



        // Evento para anexar arquivo
        private void BtnAnexarArquivos_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Todos os arquivos (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                caminhoArquivoSelecionado = ofd.FileName;
                lblArquivosSelecionado.Text = Path.GetFileName(caminhoArquivoSelecionado);
            }
        }

        // Evento para adicionar tarefa no banco
        private void BtnAddTarefas_Click(object sender, EventArgs e)
        {
            // Validação básica
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
            if (cmbAddEquipe.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma equipe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtpDataDeEntrega.Value.Date < DateTime.Today)
            {
                MessageBox.Show("A data de entrega deve ser hoje ou uma data futura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Coleta dados
            string nomeTarefa = txtNomeTarefa.Text.Trim();
            string instrucoes = txtInstruções.Text.Trim();
            int idEquipe = Convert.ToInt32(cmbAddEquipe.SelectedValue);
            DateTime dataEntrega = dtpDataDeEntrega.Value.Date;
            byte[] arquivoBytes = null;
            string nomeArquivo = "";

            // Lê arquivo se foi selecionado
            if (!string.IsNullOrEmpty(caminhoArquivoSelecionado))
            {
                try
                {
                    arquivoBytes = File.ReadAllBytes(caminhoArquivoSelecionado);
                    nomeArquivo = Path.GetFileName(caminhoArquivoSelecionado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao ler o arquivo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Cria objeto tarefa e insere no banco
            AddTarefas tarefa = new AddTarefas
            {
                NomeTarefa = nomeTarefa,
                Instrucoes = instrucoes,
                IdEquipe = idEquipe,
                DataEntrega = dataEntrega,
                NomeArquivo = nomeArquivo,
                ArquivoBlob = arquivoBytes
            };

            try
            {
                tarefa.Inserir();
                MessageBox.Show("Tarefa adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar tarefa: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Limpa os campos do formulário após salvar
        private void LimparFormulario()
        {
            txtInstruções.Clear();
            txtNomeTarefa.Clear();
            cmbAddEquipe.SelectedIndex = -1;
            dtpDataDeEntrega.Value = DateTime.Today;
            caminhoArquivoSelecionado = "";
            lblArquivosSelecionado.Text = "Nenhum arquivo selecionado";
        }

        private void btnAddTarefas_Click_1(object sender, EventArgs e)
        {

        }

        private void txtNomeTarefa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class AdicionarTarefa : Form
    {
        private string caminhoArquivoSelecionado = "";

        public AdicionarTarefa()
        {
            InitializeComponent();

            // Carrega equipes no ComboBox
            CarregarEquipes();

            // Configura eventos dos botões
            btnAnexarArquivos.Click += BtnAnexarArquivos_Click;
            btnAddTarefas.Click += BtnAddTarefas_Click;
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

            // Dados coletados do formulário
            string nomeTarefa = txtNomeTarefa.Text.Trim();
            string instrucoes = txtInstruções.Text.Trim();
            int idEquipe = Convert.ToInt32(cmbAddEquipe.SelectedValue);
            DateTime dataEntrega = dtpDataDeEntrega.Value.Date;
            byte[] arquivoBytes = null;
            string nomeArquivo = "";

            // Lê arquivo se selecionado
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

        // Limpa campos após inserção
        private void LimparFormulario()
        {
            txtInstruções.Clear();
            txtNomeTarefa.Clear();
            cmbAddEquipe.SelectedIndex = -1;
            dtpDataDeEntrega.Value = DateTime.Today;
            caminhoArquivoSelecionado = "";
            lblArquivosSelecionado.Text = "Nenhum arquivo selecionado";
        }

        // Eventos mantidos
        private void btnHome_Click(object sender, EventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Hide();
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas equip_e = new Equipes_Estatisticas();
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
            // Configurações futuras
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
        private void btnAddTarefas_Click_1(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar o que for necessário
        }

        private void txtNomeTarefa_TextChanged(object sender, EventArgs e)
        {
            // Pode deixar vazio ou implementar o que for necessário
        }

    }
}

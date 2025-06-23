using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public partial class Tela_Tarefa : Form
    {
        private int idEquipeAtual = 0; // ID da equipe selecionada
        private int idTarefaExibida = 0; // ID da tarefa atualmente exibida
        private string caminhoArquivoEntrega = ""; // Caminho do arquivo para entrega

        // Construtor padrão para evitar erros em chamadas sem parâmetros
        public Tela_Tarefa()
        {
            InitializeComponent();
            // Opcional: inicializar variáveis padrão ou mostrar mensagem
        }

        // Construtor que recebe o ID da equipe para carregar dados específicos
        public Tela_Tarefa(int idEquipe)
        {
            InitializeComponent();
            idEquipeAtual = idEquipe;
            txtNomeEquipe.Text = BuscarNomeEquipe(idEquipeAtual);

            btnEnviar.Click += BtnEnviar_Click;
            lblArquivoEntregaTarefa.Click += LblArquivoEntregaTarefa_Click;

            // Adiciona evento para atualizar detalhes ao mudar seleção no ComboBox
            cmbTarefas.SelectedIndexChanged += cmbTarefas_SelectedIndexChanged;

            CarregarTarefasNoComboBox();
        }

        // Carrega todas as tarefas da equipe no ComboBox cmbTarefas
        private void CarregarTarefasNoComboBox()
        {
            EntregaTarefa entrTarefa = new EntregaTarefa();
            DataTable dtTarefas = entrTarefa.BuscarTodasTarefasPorEquipe(idEquipeAtual);

            if (dtTarefas != null && dtTarefas.Rows.Count > 0)
            {
                cmbTarefas.DataSource = null; // limpa antes
                cmbTarefas.DataSource = dtTarefas;
                cmbTarefas.DisplayMember = "nomeTarefa"; // nome exato da coluna
                cmbTarefas.ValueMember = "id_tarefa";
                cmbTarefas.SelectedIndex = 0; // seleciona o primeiro item para mostrar texto
            }
            else
            {
                cmbTarefas.DataSource = null;
                cmbTarefas.Items.Clear();
                cmbTarefas.Text = "Nenhuma tarefa encontrada";
                LimparDetalhesTarefa();
            }
        }

        // Evento chamado quando o usuário seleciona uma tarefa diferente no ComboBox
        private void cmbTarefas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idTarefa;
            if (cmbTarefas.SelectedValue != null && int.TryParse(cmbTarefas.SelectedValue.ToString(), out idTarefa))
            {
                CarregarDetalhesTarefa(idTarefa);
            }
            else
            {
                LimparDetalhesTarefa();
            }
        }

        // Carrega detalhes da tarefa selecionada e atualiza a interface
        private void CarregarDetalhesTarefa(int idTarefa)
        {
            EntregaTarefa entrTarefa = new EntregaTarefa();
            DataRow tarefa = entrTarefa.BuscarTarefaPorId(idTarefa);

            if (tarefa != null)
            {
                idTarefaExibida = Convert.ToInt32(tarefa["id_tarefa"]);
                lblInstrucoes.Text = tarefa["instrucoes"].ToString();

                lblArquivoTarefa.Click -= LblArquivoTarefa_Click;

                if (tarefa["nome_arquivo"] != DBNull.Value && !string.IsNullOrEmpty(tarefa["nome_arquivo"].ToString()))
                {
                    lblArquivoTarefa.Text = "Arquivo: " + tarefa["nome_arquivo"].ToString();
                    lblArquivoTarefa.ForeColor = Color.Blue;
                    lblArquivoTarefa.Cursor = Cursors.Hand;
                    lblArquivoTarefa.Click += LblArquivoTarefa_Click;
                }
                else
                {
                    lblArquivoTarefa.Text = "Nenhum arquivo anexado à tarefa.";
                    lblArquivoTarefa.ForeColor = SystemColors.ControlText;
                    lblArquivoTarefa.Cursor = Cursors.Default;
                }

                btnEnviar.Enabled = true;
                LimparCamposEntrega();
            }
            else
            {
                LimparDetalhesTarefa();
            }
        }

        // Limpa os detalhes da tarefa da tela
        private void LimparDetalhesTarefa()
        {
            idTarefaExibida = 0;
            lblInstrucoes.Text = "";
            lblArquivoTarefa.Text = "";
            btnEnviar.Enabled = false;
            LimparCamposEntrega();
        }

        // Evento para abrir o arquivo anexado à tarefa
        private void LblArquivoTarefa_Click(object sender, EventArgs e)
        {
            if (idTarefaExibida == 0) return;

            EntregaTarefa entrTarefa = new EntregaTarefa();
            DataRow tarefa = entrTarefa.BuscarTarefaPorId(idTarefaExibida);

            if (tarefa != null && tarefa["arquivo_blob"] != DBNull.Value)
            {
                try
                {
                    byte[] arquivo = (byte[])tarefa["arquivo_blob"];
                    string tempPath = Path.Combine(Path.GetTempPath(), tarefa["nome_arquivo"].ToString());
                    File.WriteAllBytes(tempPath, arquivo);
                    System.Diagnostics.Process.Start(tempPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o arquivo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Busca o nome da equipe pelo ID no banco
        private string BuscarNomeEquipe(int idEquipe)
        {
            string nome = "";
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=seu_usuario;pwd=sua_senha;"))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT nome_equipe FROM Equipes WHERE id_equipe = @id", conn);
                cmd.Parameters.AddWithValue("@id", idEquipe);
                var result = cmd.ExecuteScalar();
                nome = result != null ? result.ToString() : "";
            }
            return nome;
        }

        // Evento para anexar arquivo de entrega
        private void LblArquivoEntregaTarefa_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Todos os arquivos (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                caminhoArquivoEntrega = ofd.FileName;
                lblArquivoEntregaTarefa.Text = Path.GetFileName(caminhoArquivoEntrega);
                lblArquivoEntregaTarefa.ForeColor = Color.Blue;
            }
        }

        // Evento para enviar a entrega da tarefa
        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (idTarefaExibida == 0)
            {
                MessageBox.Show("Não há tarefa para ser entregue.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescrição.Text))
            {
                MessageBox.Show("Por favor, descreva sua entrega.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] arquivoBytes = null;
            string nomeArquivo = null;

            if (!string.IsNullOrEmpty(caminhoArquivoEntrega))
            {
                try
                {
                    arquivoBytes = File.ReadAllBytes(caminhoArquivoEntrega);
                    nomeArquivo = Path.GetFileName(caminhoArquivoEntrega);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao ler o arquivo de entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            EntregaTarefa entrTarefa = new EntregaTarefa();
            try
            {
                entrTarefa.RegistrarEntrega(idTarefaExibida, idEquipeAtual, txtDescrição.Text, nomeArquivo, arquivoBytes);
                MessageBox.Show("Entrega registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCamposEntrega();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar a entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Limpa campos após entrega
        private void LimparCamposEntrega()
        {
            txtDescrição.Clear();
            lblArquivoEntregaTarefa.Text = "Clique para anexar arquivo";
            lblArquivoEntregaTarefa.ForeColor = Color.Gray;
            caminhoArquivoEntrega = "";
        }

        // Eventos e métodos adicionais (mantidos)
        private void btnHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas t_equipe = new Equipes_Estatisticas();
            t_equipe.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes trf_pendente = new Tarefas_Pendentes();
            trf_pendente.Show();
            this.Hide();
        }

        private void lblGeral_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes chatEquipe = new Chat_geral_equipes();
            chatEquipe.Show();
            this.Hide();
        }

        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (cmbTarefas.SelectedValue != null)
            {
                int idTarefaSelecionada;
                if (int.TryParse(cmbTarefas.SelectedValue.ToString(), out idTarefaSelecionada))
                {
                    CarregarDetalhesTarefa(idTarefaSelecionada);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma tarefa na lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblRanking_Click(object sender, EventArgs e) { }
        private void btnRelatarProblema_Click(object sender, EventArgs e) { }
        private void txtDescrição_TextChanged(object sender, EventArgs e) { }
        private void btnConfigurações_Click(object sender, EventArgs e) { }
        private void lblPlanejamento_Click(object sender, EventArgs e) { }
        private void btnEnviar_Click(object sender, EventArgs e) { }
    }
}

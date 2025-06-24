using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public partial class Tela_TarefasAdmin : Form
    {
        private int idTarefaExibida = 0; // ID da tarefa atualmente exibida

        public Tela_TarefasAdmin(int idTarefa)
        {
            InitializeComponent();
            CarregarDetalhesTarefa(idTarefa);
        }

        public void CarregarDetalhesTarefa(int idTarefa)
        {
            EntregaTarefa entrTarefa = new EntregaTarefa();
            DataRow tarefa = entrTarefa.BuscarTarefaPorId(idTarefa);

            if (tarefa != null)
            {
                idTarefaExibida = Convert.ToInt32(tarefa["id_tarefa"]);

                // Nome da tarefa na label
                lblNomeTarefa.Text = tarefa["nomeTarefa"].ToString();

                // Nome da equipe na label (busca pelo id_equipe)
                int idEquipe = Convert.ToInt32(tarefa["id_equipe"]);
                lblEquipe.Text = BuscarNomeEquipe(idEquipe);

                // Categoria da equipe na label
                lblCategoriaEquipe.Text = tarefa["nome_categoria"].ToString();

                // Data de entrega formatada na label
                DateTime dataEntrega = Convert.ToDateTime(tarefa["data_entrega"]);
                lblDataEntrega.Text = dataEntrega.ToString("dd/MM/yyyy");

                lblInstrucoes.Text = tarefa["instrucoes"].ToString();

                // Mostrar dificuldade
                if (tarefa.Table.Columns.Contains("dificuldade") && tarefa["dificuldade"] != DBNull.Value)
                {
                    lblDificuldade.Text = "Dificuldade: " + tarefa["dificuldade"].ToString();
                    lblDificuldade.Visible = true;
                }
                else
                {
                    lblDificuldade.Visible = false;
                }

                // Arquivo anexado
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

                LimparCamposEntrega();
            }
            else
            {
                LimparDetalhesTarefa();
            }
        }

        private void LimparDetalhesTarefa()
        {
            idTarefaExibida = 0;
            lblInstrucoes.Text = "";
            lblArquivoTarefa.Text = "";
            lblNomeTarefa.Text = "";
            lblEquipe.Text = "";
            lblCategoriaEquipe.Text = "";
            lblDataEntrega.Text = "";
            lblDificuldade.Text = "";
            LimparCamposEntrega();
        }

        private string BuscarNomeEquipe(int idEquipe)
        {
            string nome = "";
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd=;"))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT nome_equipe FROM Equipes WHERE id_equipe = @id", conn);
                cmd.Parameters.AddWithValue("@id", idEquipe);
                var result = cmd.ExecuteScalar();
                nome = result != null ? result.ToString() : "";
            }
            return nome;
        }

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

        private void LimparCamposEntrega()
        {
        }

        // Eventos para navegação (exemplo)
        private void lblVoltar_Click(object sender, EventArgs e)
        {
            AvaliaçãoTarefaAdmin avAdmin = new AvaliaçãoTarefaAdmin();
            avAdmin.Show();
            this.Hide();
        }
    }
}
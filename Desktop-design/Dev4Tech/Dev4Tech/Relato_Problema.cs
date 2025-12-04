using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Relato_Problema : Form
    {
        private int idTarefa;
        private int idEquipe;

        // Construtor que recebe os parâmetros necessários
        public Relato_Problema(int idTarefa, int idEquipe)
        {
            InitializeComponent();
            this.idTarefa = idTarefa;
            this.idEquipe = idEquipe;

            // VERIFICAR SE A TAREFA JÁ FOI AVALIADA
            AvaliacaoTarefa avaliacao = new AvaliacaoTarefa();
            bool tarefaAvaliada = avaliacao.TarefaFoiAvaliada(idTarefa);

            if (tarefaAvaliada)
            {
                // DESABILITAR O FORMULÁRIO SE A TAREFA JÁ FOI AVALIADA
                txtDescriçãoProblema.Enabled = false;
                btnEnviar.Enabled = false;

                // MUDAR A COR E TEXTO PARA INDICAR BLOQUEIO
                txtDescriçãoProblema.Text = "Esta tarefa já foi avaliada pelo administrador e não é mais possível relatar problemas.";
                txtDescriçãoProblema.BackColor = Color.LightGray;
                txtDescriçãoProblema.ForeColor = Color.DarkRed;

                // ALTERAR O TEXTO DO BOTÃO
                btnEnviar.Text = "Bloqueado";
                btnEnviar.BackColor = Color.Gray;

                MessageBox.Show("Esta tarefa já foi avaliada e não é mais possível relatar problemas.", "Tarefa Avaliada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtDescriçãoProblema_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // VERIFICAR NOVAMENTE SE A TAREFA JÁ FOI AVALIADA (PARA EVITAR BURLAS)
            AvaliacaoTarefa avaliacao = new AvaliacaoTarefa();
            bool tarefaAvaliada = avaliacao.TarefaFoiAvaliada(idTarefa);

            if (tarefaAvaliada)
            {
                MessageBox.Show("Esta tarefa já foi avaliada pelo administrador e não é mais possível relatar problemas.", "Tarefa Avaliada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string descricao = txtDescriçãoProblema.Text.Trim();

            if (string.IsNullOrEmpty(descricao))
            {
                MessageBox.Show("Por favor, escreva a descrição do problema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEmpresa = BuscarIdEmpresaPorTarefa(idTarefa);

            EnvioProblema relato = new EnvioProblema();
            try
            {
                relato.InserirRelato(idTarefa, idEquipe, idEmpresa, descricao);
                MessageBox.Show("Problema enviado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar problema: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int BuscarIdEmpresaPorTarefa(int idTarefa)
        {
            int idEmpresa = 0;
            string query = @"SELECT a.id_empresa
                             FROM Administradores a
                             INNER JOIN Equipes e ON a.AdminId = e.AdminId
                             INNER JOIN Tarefas t ON t.id_equipe = e.id_equipe
                             WHERE t.id_tarefa = @idTarefa
                             LIMIT 1";
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd=;"))
            {
                conn.Open();
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                    }
                }
            }
            return idEmpresa;
        }

    }
}

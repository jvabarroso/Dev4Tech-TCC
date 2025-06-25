using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Tarefas_Pendentes : Form
    {
        public Tarefas_Pendentes()
        {
            InitializeComponent();
            CarregarTarefasPendentes();
        }

        private void CarregarTarefasPendentes()
        {
            int idEquipe = 1; // Ajuste conforme o contexto do seu sistema

            panelTarefas.Controls.Clear();

            EntregaTarefa entregaTarefa = new EntregaTarefa();
            DataTable dt = entregaTarefa.BuscarTarefasPendentesPorEquipeOrdenadasPorDificuldade(idEquipe);

            int margemTopo = 20;
            int margemEsquerda = 20;
            int espacamentoVertical = 20;
            int espacamentoHorizontal = 20;
            int larguraPanel = 350;
            int alturaPanel = 100;
            int colunas = 2;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                string dificuldade = row["dificuldade"].ToString();

                Panel tarefaPanel = new Panel
                {
                    Width = larguraPanel,
                    Height = alturaPanel,
                    BorderStyle = BorderStyle.FixedSingle,
                    Left = margemEsquerda + (i % colunas) * (larguraPanel + espacamentoHorizontal),
                    Top = margemTopo + (i / colunas) * (alturaPanel + espacamentoVertical),
                    Cursor = Cursors.Hand,
                    Tag = row["id_tarefa"],
                    BackColor = Color.White // Fundo branco do painel
                };

                tarefaPanel.Click += (s, e) =>
                {
                    int idTarefa = Convert.ToInt32(((Panel)s).Tag);
                    Tela_Tarefa telaTarefa = new Tela_Tarefa(idEquipe);
                    telaTarefa.CarregarDetalhesTarefa(idTarefa);
                    telaTarefa.Show();
                    this.Hide();
                };

                PictureBox pic = new PictureBox
                {
                    Image = Properties.Resources.icon_EquipLogo,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 40,
                    Height = 40,
                    Left = 10,
                    Top = 10
                };
                tarefaPanel.Controls.Add(pic);

                Label lblNome = new Label
                {
                    Text = row["nomeTarefa"].ToString(),
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Left = 60,
                    Top = 5,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblNome);

                Label lblSub = new Label
                {
                    Text = row["nome_equipe"].ToString(),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Left = 60,
                    Top = 30,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblSub);

                Label lblCategoria = new Label
                {
                    Text = row["nome_categoria"].ToString(),
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 60,
                    Top = 50,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblCategoria);

                Label lblConclusao = new Label
                {
                    Text = "Conclusão em " + Convert.ToDateTime(row["data_entrega"]).ToString("dd/MM/yy") + " às 00:00",
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 60,
                    Top = 70,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblConclusao);

                Label lblStatus = new Label
                {
                    Text = "Pendente",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Black,
                    BackColor = Color.Yellow,
                    Left = larguraPanel - 90,
                    Top = 10,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblStatus);

                Label lblDificuldade = new Label
                {
                    Text = "Dificuldade: " + dificuldade,
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.Black,
                    Left = larguraPanel - 90,
                    Top = 30,
                    AutoSize = true
                };

                // Define a cor de fundo da label conforme a dificuldade
                switch (dificuldade.ToLower())
                {
                    case "difícil":
                        lblDificuldade.BackColor = Color.LightCoral; // vermelho claro
                        break;
                    case "média":
                    case "mediana":
                        lblDificuldade.BackColor = Color.LightGoldenrodYellow; // amarelo claro
                        break;
                    case "fácil":
                        lblDificuldade.BackColor = Color.LightGreen; // verde claro
                        break;
                    default:
                        lblDificuldade.BackColor = Color.Transparent;
                        break;
                }

                tarefaPanel.Controls.Add(lblDificuldade);

                panelTarefas.Controls.Add(tarefaPanel);
            }
        }

        // Eventos e métodos adicionais mantidos conforme seu código original
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

        private void lblRanking_Click(object sender, EventArgs e) {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }
        private void btnPendentes_Click(object sender, EventArgs e) { }
        private void btnEmAtraso_Click(object sender, EventArgs e)
        {
            Tarefas_Atrasadas ta = new Tarefas_Atrasadas();
            ta.Show();
            this.Hide();
        }
        private void btnCompletadas_Click(object sender, EventArgs e)
        {
            Tarefas_Completadas tc = new Tarefas_Completadas();
            tc.Show();
            this.Hide();
        }
        private void Tarefa1_Enter(object sender, EventArgs e) { }
        private void txtPesquisaTarefa_TextChanged(object sender, EventArgs e) { }
        private void btnEquipe_Click(object sender, EventArgs e) {
            PesquisaEquipes P_equipe = new PesquisaEquipes();
            P_equipe.Show();
            this.Hide();
        }
        private void btnHome_Click(object sender, EventArgs e) {
            Home h = new Home();
            h.Show();
            this.Hide();
        }
        private void lblPlanejamento_Click(object sender, EventArgs e) {
            Planejamento p_plano = new Planejamento();
            p_plano.Show();
            this.Hide();
        }

        private void btnConfig_Click(object sender, EventArgs e)
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

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
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
    }
}

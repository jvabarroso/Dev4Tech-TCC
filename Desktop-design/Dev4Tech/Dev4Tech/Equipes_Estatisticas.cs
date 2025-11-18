using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql;
using MySql.Data;
using System.Windows.Forms.DataVisualization.Charting;


namespace Dev4Tech
{
    public partial class Equipes_Estatisticas : Form
    {
        public Equipes_Estatisticas()
        {
            InitializeComponent();
        }


        public int idEquipe;



        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Equipes_Estatisticas_Load(object sender, EventArgs e)
        {
            if (idEquipe <= 0)
            {
                MessageBox.Show("Por favor, selecione uma equipe válida antes de abrir as estatísticas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            CarregarGrafico();
            CarregarEquipeSelecionada();
            CarregarFuncionariosEquipe();
        }


        private void CarregarEquipeSelecionada()
        {
            panelEquipe.Controls.Clear();

            EquipesRanking dao = new EquipesRanking();

            // Get all teams ordered by points descending
            DataTable allTeams = dao.BuscarEquipesComPontuacao();

            // Find rank by iterating teams in order
            int rank = -1;
            for (int i = 0; i < allTeams.Rows.Count; i++)
            {
                int currentIdEquipe = Convert.ToInt32(allTeams.Rows[i]["id_equipe"]);
                if (currentIdEquipe == idEquipe)
                {
                    rank = i + 1; // ranks start at 1
                    break;
                }
            }

            if (rank == -1)
            {
                MessageBox.Show("Equipe não encontrada no ranking.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = dao.BuscarEquipePorId(idEquipe);
            if (dt.Rows.Count == 0) return;
            DataRow row = dt.Rows[0];
            string nomeEquipe = row["nome_equipe"].ToString();

            // Calcular pontos da equipe somando os pontos dos membros
            int pontosEquipe = 0;
            string connectionString = "SERVER=localhost;DATABASE=dev4tech;UID=root;PASSWORD=";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = @"
            SELECT COALESCE(SUM(
                CASE t.dificuldade
                    WHEN 'Fácil' THEN 10
                    WHEN 'Média' THEN 20
                    WHEN 'Difícil' THEN 30
                    ELSE 0
                END
            ), 0) AS pontos
            FROM Equipes_Membros em
            JOIN Funcionarios f ON em.FuncionarioId = f.FuncionarioId
            LEFT JOIN EntregasTarefa et ON et.FuncionarioId = f.FuncionarioId AND et.id_equipe = em.id_equipe AND et.entregue = 1
            LEFT JOIN Tarefas t ON et.id_tarefa = t.id_tarefa
            WHERE em.id_equipe = @id_equipe
            GROUP BY f.FuncionarioId";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pontosEquipe += reader.GetInt32("pontos");
                    }
                }
            }

            List<MembroEquipe> membros = dao.BuscarMembrosEquipe(idEquipe);

            int alturaPainel = 90;
            Panel equipePanel = new Panel
            {
                Width = panelEquipe.ClientSize.Width - 20,
                Height = alturaPainel,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = idEquipe,
                Cursor = Cursors.Hand,
                Location = new Point(10, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            PictureBox picIcone = new PictureBox
            {
                Width = 44,
                Height = 44,
                Left = 10,
                Top = (alturaPainel - 44) / 2,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };

            // Set icon based on actual rank
            if (rank == 1)
                picIcone.Image = Properties.Resources.icon_ranking_1;
            else if (rank == 2)
                picIcone.Image = Properties.Resources.icon_ranking_2;
            else if (rank == 3)
                picIcone.Image = Properties.Resources.icon_ranking_3;
            else
                picIcone.Image = null;

            equipePanel.Controls.Add(picIcone);

            Label lblRank = new Label
            {
                Text = $"#{rank}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Left = picIcone.Right + 10,
                Top = 20,
                ForeColor = Color.Black,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblRank);

            Label lblNomeEquipe = new Label
            {
                Text = nomeEquipe,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Left = picIcone.Right + 60,
                Top = 20,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblNomeEquipe);

            Label lblPontos = new Label
            {
                Text = "Pontuação: " + pontosEquipe,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.DodgerBlue,
                Left = picIcone.Right + 60,
                Top = lblNomeEquipe.Bottom + 7,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblPontos);

            int numMembrosParaMostrar = Math.Min(membros.Count, 3);
            int leftFoto = equipePanel.Width - 40 - (35 * numMembrosParaMostrar);
            ToolTip toolTipMembros = new ToolTip();
            for (int i = 0; i < numMembrosParaMostrar; i++)
            {
                PictureBox picMembro = new PictureBox
                {
                    Image = membros[i].FotoPerfil ?? Properties.Resources.icon_perfil,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 34,
                    Height = 34,
                    Left = leftFoto + (i * 35),
                    Top = (alturaPainel - 34) / 2,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                };
                toolTipMembros.SetToolTip(picMembro, membros[i].Nome);
                equipePanel.Controls.Add(picMembro);
            }

            panelEquipe.Controls.Add(equipePanel);
            panelEquipe.Refresh();
        }



        public void CarregarGrafico()
        {
            chartPontuacao.Series.Clear();
            chartPontuacao.Titles.Clear();
            chartPontuacao.Legends.Clear();


            chartPontuacao.Legends.Add("Pontuação dos Funcionários");
            chartPontuacao.Legends[0].LegendStyle = LegendStyle.Table;
            chartPontuacao.Legends[0].Docking = Docking.Right;
            chartPontuacao.Legends[0].Alignment = StringAlignment.Center;
            chartPontuacao.Legends[0].BorderColor = Color.Black;
            chartPontuacao.Legends[0].Title = "Pontuação dos Funcionários";
            chartPontuacao.Titles.Add("Pontuação dos Funcionários");

            string stringC = "SERVER=localhost;DATABASE=dev4tech;UID=root;PASSWORD=";
            using (MySqlConnection con = new MySqlConnection(stringC))
            {
                MySqlCommand comandos = con.CreateCommand();
                con.Open();
                comandos.Parameters.AddWithValue("@id_equipe", idEquipe);
                comandos.CommandText = @"SELECT 
    f.FuncionarioId, 
    f.Nome,
    COALESCE(pontos_soma, 0) AS pontos
FROM Equipes_Membros em
INNER JOIN Funcionarios f ON em.FuncionarioId = f.FuncionarioId
LEFT JOIN (
    SELECT 
        et.FuncionarioId, 
        SUM(CASE 
            WHEN t.dificuldade = 'Fácil' THEN 10
            WHEN t.dificuldade = 'Média' THEN 20
            WHEN t.dificuldade = 'Difícil' THEN 30
            ELSE 0
        END) AS pontos_soma
    FROM EntregasTarefa et
    INNER JOIN Tarefas t ON et.id_tarefa = t.id_tarefa
    WHERE et.id_equipe = @id_equipe AND et.entregue = 1
    GROUP BY et.FuncionarioId
) AS pontuacoes ON f.FuncionarioId = pontuacoes.FuncionarioId
WHERE em.id_equipe = @id_equipe
ORDER BY pontos DESC;
";

                using (MySqlDataReader resultado = comandos.ExecuteReader())
                {
                    var series = chartPontuacao.Series.Add("Pontuação");
                    series.ChartType = SeriesChartType.Column;
                    series.IsValueShownAsLabel = false;  // Mostrar pontos nas colunas
                    series.MarkerStyle = MarkerStyle.None;

                    while (resultado.Read())
                    {
                        string nomeFuncionario = resultado.GetString("nome");
                        double pontos = resultado.GetDouble("pontos");
                        int pointIndex = series.Points.AddXY(nomeFuncionario, pontos);


                        series.Points[pointIndex].LegendText = nomeFuncionario;
                        series.Points[pointIndex].Label = ""; // pode mostrar os pontos também
                    }
                }
            }



            // Limpa e prepara o gráfico
            chartDificuldade.Series.Clear();
            chartDificuldade.Titles.Clear();
            chartDificuldade.Legends.Clear();


            chartDificuldade.Legends.Add("Dificuldades das Tarefas");
            chartDificuldade.Legends[0].LegendStyle = LegendStyle.Table;
            chartDificuldade.Legends[0].Docking = Docking.Right;
            chartDificuldade.Legends[0].Alignment = StringAlignment.Center;
            chartDificuldade.Legends[0].BorderColor = Color.Black;
            chartDificuldade.Legends[0].Title = "Dificuldades";

            string stringD = "SERVER=localhost;DATABASE=dev4tech;UID=root;PASSWORD=";
            using (MySqlConnection con = new MySqlConnection(stringD))
            {
                MySqlCommand comandos = con.CreateCommand();
                con.Open();
                comandos.Parameters.AddWithValue("@id_equipe", idEquipe);
                comandos.CommandText = "SELECT t.dificuldade FROM tarefas t JOIN entregastarefa e ON t.id_tarefa = e.id_tarefa WHERE e.id_equipe = @id_equipe AND e.entregue = 1";

                Dictionary<string, double> dificuldadePontos = new Dictionary<string, double>()
                    {
                        {"Fácil", 0},
                        {"Média", 0},
                        {"Difícil", 0}
                    };

                using (MySqlDataReader resultado = comandos.ExecuteReader())
                {
                    while (resultado.Read())
                    {
                        string dificuldade = resultado.GetString("dificuldade");
                        switch (dificuldade)
                        {
                            case "Fácil":
                                dificuldadePontos["Fácil"] += 1;
                                break;
                            case "Média":
                                dificuldadePontos["Média"] += 2;
                                break;
                            case "Difícil":
                                dificuldadePontos["Difícil"] += 3;
                                break;
                            default:
                                break;
                        }
                    }
                }

                var total = dificuldadePontos.Values.Sum();

                var series = chartDificuldade.Series.Add("Dificuldades");
                series.ChartType = SeriesChartType.Doughnut;

                foreach (var item in dificuldadePontos)
                {
                    if (item.Value > 0)
                    {
                        int pointIndex = series.Points.AddY(item.Value);
                        series.Points[pointIndex].LegendText = $"{item.Key} ({(item.Value / total * 100):N1}%)";
                        series.Points[pointIndex].Label = "";

                    }
                }
            }

            chartEntregaTarefa.Series.Clear();
            chartEntregaTarefa.Titles.Clear();
            chartEntregaTarefa.Legends.Clear();

            chartEntregaTarefa.Legends.Add("Envio de Tarefas");
            chartEntregaTarefa.Legends[0].LegendStyle = LegendStyle.Table;
            chartEntregaTarefa.Legends[0].Docking = Docking.Right;
            chartEntregaTarefa.Legends[0].Alignment = StringAlignment.Center;
            chartEntregaTarefa.Legends[0].BorderColor = Color.Black;
            chartEntregaTarefa.Legends[0].Title = "Tarefas Enviadas";

            string stringE = "SERVER=localhost;DATABASE=dev4tech;UID=root;PASSWORD=";
            using (MySqlConnection con = new MySqlConnection(stringE))
            {
                MySqlCommand comandos = con.CreateCommand();
                con.Open();
                comandos.Parameters.AddWithValue("@id_equipe", idEquipe);
                comandos.CommandText = @"
                    SELECT f.Nome, COUNT(*) AS Quantidade
                    FROM entregastarefa et
                    JOIN funcionarios f ON et.FuncionarioId = f.FuncionarioId
                    WHERE et.id_equipe = @id_equipe
                    GROUP BY et.FuncionarioId, f.Nome";


                using (MySqlDataReader resultado = comandos.ExecuteReader())
                {
                    var series = chartEntregaTarefa.Series.Add("Entregas por Funcionário");
                    series.ChartType = SeriesChartType.Column;
                    series.IsValueShownAsLabel = false;
                    series.MarkerStyle = MarkerStyle.None; // remover pontos


                    while (resultado.Read())
                    {
                        string nomeFuncionario = resultado.GetString("Nome");
                        int quantidade = resultado.GetInt32("Quantidade");
                        int pointIndex = series.Points.AddXY(nomeFuncionario, quantidade);

                        series.Points[pointIndex].LegendText = nomeFuncionario;
                        series.Points[pointIndex].Label = "";
                    }
                }
            }
        }

        private void btnHome_Click_1(object sender, EventArgs e)
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

        private void btnEquipes_Click_1(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                PesquisaEquipes t_equipe = new PesquisaEquipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRanking_Click_1(object sender, EventArgs e)
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

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            // Limpa a sessão antes de voltar para a tela inicial
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;

            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
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
                // Se for funcionário, abre a tela de adicionar tarefa (exemplo)
                Tarefas_Pendentes t_equipe = new Tarefas_Pendentes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                AvaliaçãoTarefaAdmin t_equipeAdmin = new AvaliaçãoTarefaAdmin();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }

        private void lblMembros_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Integrantes_Equipe h = new Integrantes_Equipe();
                h.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                Integrantes_Equipe t_equipeAdmin = new Integrantes_Equipe();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblPlanejamento_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (Sessao.IdEquipeSelecionada != 0)
            {
                int idEquipe = Sessao.IdEquipeSelecionada;

                if (funcionario != null)
                {
                    Planejamento t_equipe = new Planejamento();
                    t_equipe.Show();
                    this.Hide();
                }
                else if (admin != null)
                {
                    MessageBox.Show("Tela voltada para tarefas dos funcionários.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Equipes_Estatisticas t_equipeAdmin = new Equipes_Estatisticas();
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
                Ranking_Equipes rank = new Ranking_Equipes();
                rank.Show();
                this.Hide();
            }
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Tarefas_Completadas h = new Tarefas_Completadas();
                h.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                // Se for administrador, abre a tela de adicionar tarefa para admin (exemplo)
                AvaliaçãoTarefaAdmin t_equipeAdmin = new AvaliaçãoTarefaAdmin();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CarregarFuncionariosEquipe()
        {
            flpRankFunc.Controls.Clear();

            EquipesRanking dao = new EquipesRanking();
            List<MembroEquipe> membros = dao.BuscarMembrosEquipe(idEquipe);

            if (membros.Count == 0)
            {
                Label lblSemMembros = new Label
                {
                    Text = "Nenhum membro encontrado nesta equipe",
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                flpRankFunc.Controls.Add(lblSemMembros);
                return;
            }

            // Buscar pontuações individuais dos funcionários
            Dictionary<int, int> pontuacoes = ObterPontuacoesFuncionarios();

            // Combinar membros com suas pontuações e ordenar por pontos (decrescente)
            var membrosOrdenados = membros.Select(m => new
            {
                Membro = m,
                Pontos = pontuacoes.ContainsKey(m.FuncionarioId) ? pontuacoes[m.FuncionarioId] : 0
            }).OrderByDescending(x => x.Pontos).ToList();

            foreach (var item in membrosOrdenados)
            {
                int rank = membrosOrdenados.IndexOf(item) + 1; // Posição no ranking (1-based)
                Panel cardFuncionario = CriarCardFuncionario(item.Membro, item.Pontos, rank);
                flpRankFunc.Controls.Add(cardFuncionario);
            }
        }

        private Dictionary<int, int> ObterPontuacoesFuncionarios()
        {
            Dictionary<int, int> pontuacoes = new Dictionary<int, int>();

            string connectionString = "SERVER=localhost;DATABASE=dev4tech;UID=root;PASSWORD=";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = @"
    SELECT
    f.FuncionarioId,
    f.Nome,
    COALESCE(SUM(
        CASE t.dificuldade
            WHEN 'Fácil' THEN 10
            WHEN 'Média' THEN 20
            WHEN 'Difícil' THEN 30
            ELSE 0
        END
    ), 0) AS pontos
FROM Equipes_Membros em
JOIN Funcionarios f ON em.FuncionarioId = f.FuncionarioId
LEFT JOIN EntregasTarefa et ON et.FuncionarioId = f.FuncionarioId AND et.id_equipe = em.id_equipe AND et.entregue = 1
LEFT JOIN Tarefas t ON et.id_tarefa = t.id_tarefa
WHERE em.id_equipe = @id_equipe
GROUP BY f.FuncionarioId, f.Nome
ORDER BY pontos DESC;

";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id_equipe", idEquipe);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int funcionarioId = reader.GetInt32("funcionarioId");
                        int pontos = reader.GetInt32("pontos");
                        pontuacoes[funcionarioId] = pontos;
                    }
                }
            }

            return pontuacoes;
        }

        private Panel CriarCardFuncionario(MembroEquipe membro, int pontos, int rank)
        {
            Panel card = new Panel
            {
                Width = flpRankFunc.Width - 25,
                Height = 80,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Padding = new Padding(10)
            };

            // Ícone de classificação (1°, 2°, 3°)
            if (rank <= 3)
            {
                PictureBox picRank = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 30,
                    Height = 30,
                    Location = new Point(10, 25)
                };

                switch (rank)
                {
                    case 1:
                        picRank.Image = Properties.Resources.icon_ranking_1;
                        break;
                    case 2:
                        picRank.Image = Properties.Resources.icon_ranking_2;
                        break;
                    case 3:
                        picRank.Image = Properties.Resources.icon_ranking_3;
                        break;
                }

                card.Controls.Add(picRank);
            }
            else
            {
                // Para ranks acima de 3, mostrar apenas o número
                Label lblRank = new Label
                {
                    Text = $"#{rank}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(15, 30)
                };
                card.Controls.Add(lblRank);
            }

            // PictureBox para a foto do perfil - posicionada após o ícone de rank
            int leftFoto = 50;
            PictureBox picPerfil = new PictureBox
            {
                Image = membro.FotoPerfil,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 45,
                Height = 45,
                Location = new Point(leftFoto, 17)
            };
            card.Controls.Add(picPerfil);

            // Área de informações do funcionário (nome e cargo)
            Panel pnlInfo = new Panel
            {
                Width = card.Width - 180, // Deixa espaço para a pontuação
                Height = 45,
                Location = new Point(picPerfil.Right + 10, 17),
                BackColor = Color.Transparent
            };

            // Nome do funcionário
            Label lblNome = new Label
            {
                Text = membro.Nome,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                Width = pnlInfo.Width,
                Height = 20,
                Location = new Point(0, 0)
            };
            pnlInfo.Controls.Add(lblNome);

            // Cargo do funcionário
            Label lblCargo = new Label
            {
                Text = membro.Cargo,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = false,
                Width = pnlInfo.Width,
                Height = 20,
                Location = new Point(0, 22)
            };
            pnlInfo.Controls.Add(lblCargo);

            card.Controls.Add(pnlInfo);

            // Container para pontuação (alinhado à direita) - mais compacto
            Panel pnlPontos = new Panel
            {
                Width = 60,
                Height = 30,
                Location = new Point(card.Width - 75, 25),
                BackColor = Color.FromArgb(240, 245, 255), // Azul muito claro
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(2)
            };

            // Label da pontuação
            Label lblPontos = new Label
            {
                Text = $"{pontos} pts",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DodgerBlue,
                AutoSize = false,
                Width = pnlPontos.Width - 4,
                Height = pnlPontos.Height - 4,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlPontos.Controls.Add(lblPontos);
            card.Controls.Add(pnlPontos);

            // Adicionar efeito visual para os primeiros colocados
            if (rank == 1)
            {
                card.BackColor = Color.FromArgb(255, 250, 240); // Dourado claro
                pnlPontos.BackColor = Color.FromArgb(255, 245, 200); // Dourado mais forte
            }
            else if (rank == 2)
            {
                card.BackColor = Color.FromArgb(248, 248, 248); // Prata claro
                pnlPontos.BackColor = Color.FromArgb(230, 230, 230); // Prata mais forte
            }
            else if (rank == 3)
            {
                card.BackColor = Color.FromArgb(255, 245, 238); // Bronze claro
                pnlPontos.BackColor = Color.FromArgb(245, 220, 190); // Bronze mais forte
            }

            return card;
        }
    }
}

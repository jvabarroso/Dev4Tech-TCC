using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class AvaliaçãoTarefaAdmin : Form
    {
        private Dictionary<int, AvaliacaoInfo> avaliacoes = new Dictionary<int, AvaliacaoInfo>();

        public AvaliaçãoTarefaAdmin()
        {
            InitializeComponent();
            CarregarTarefasNaoAvaliadas();
        }

        private void CarregarTarefasNaoAvaliadas()
        {
            panelAvaliacaoEquipes.Controls.Clear();
            List<int> equipesIds = BuscarIdsEquipes();

            int top = 10;
            foreach (int idEquipe in equipesIds)
            {
                AvaliacaoTarefa avaliacaoTarefa = new AvaliacaoTarefa();
                DataTable tarefasNaoAvaliadas = avaliacaoTarefa.BuscarTarefasNaoAvaliadasPorEquipe(idEquipe);

                foreach (DataRow tarefa in tarefasNaoAvaliadas.Rows)
                {
                    int idTarefa = Convert.ToInt32(tarefa["id_tarefa"]);
                    string nomeTarefa = tarefa["nomeTarefa"].ToString();
                    string nomeEquipe = tarefa["nome_equipe"].ToString();
                    string dificuldade = tarefa["dificuldade"].ToString();
                    DataRow relatoProblema = avaliacaoTarefa.BuscarRelatoProblema(idTarefa);
                    bool temProblema = relatoProblema != null;


                    bool atrasada = Convert.ToDateTime(tarefa["data_entrega"]) < DateTime.Today;

                    Panel painelTarefa = new Panel
                    {
                        Width = panelAvaliacaoEquipes.Width - 40,
                        Height = atrasada ? 140 : (temProblema ? 120 : 110), // Aumenta altura se tem problema
                        Top = top,
                        Left = 10,
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = idTarefa,
                        Cursor = Cursors.Default,
                        BackColor = temProblema ? Color.LightYellow : SystemColors.Control // Destaque visual
                    };

                    // Labels
                    Label lblNome = new Label
                    {
                        Text = "Tarefa: " + nomeTarefa,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Left = 10,
                        Top = 10,
                        AutoSize = true
                    };
                    painelTarefa.Controls.Add(lblNome);

                    Label lblEquipe = new Label
                    {
                        Text = "Equipe: " + nomeEquipe,
                        Font = new Font("Segoe UI", 9, FontStyle.Regular),
                        Left = 10,
                        Top = 35,
                        AutoSize = true
                    };
                    painelTarefa.Controls.Add(lblEquipe);

                    Label lblDificuldade = new Label
                    {
                        Text = "Dificuldade: " + dificuldade,
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        Left = 10,
                        Top = 60,
                        AutoSize = true
                    };
                    painelTarefa.Controls.Add(lblDificuldade);

                    // RadioButtons para avaliação
                    RadioButton rbAceita = new RadioButton
                    {
                        Text = "Aceita",
                        Left = 10,
                        Top = 85,
                        AutoSize = true,
                        Name = "rbAceita_" + idTarefa
                    };
                    rbAceita.CheckedChanged += (s, e) =>
                    {
                        if (rbAceita.Checked)
                            AtualizarAvaliacao(idTarefa, true);
                    };

                    RadioButton rbNegada = new RadioButton
                    {
                        Text = "Negada",
                        Left = 80,
                        Top = 85,
                        AutoSize = true,
                        Name = "rbNegada_" + idTarefa
                    };
                    rbNegada.CheckedChanged += (s, e) =>
                    {
                        if (rbNegada.Checked)
                            AtualizarAvaliacao(idTarefa, false);
                    };

                    painelTarefa.Controls.Add(rbAceita);
                    painelTarefa.Controls.Add(rbNegada);

                    if (temProblema)
                    {
                        Button btnVerProblema = new Button
                        {
                            Text = "Ver Problema",
                            Left = 160,
                            Top = 85,
                            Width = 100,
                            Height = 25,
                            BackColor = Color.Orange,
                            ForeColor = Color.White,
                            Font = new Font("Segoe UI", 8, FontStyle.Bold),
                            Name = "btnVerProblema_" + idTarefa,
                            Cursor = Cursors.Hand
                        };

                        btnVerProblema.Click += (s, e) =>
                        {
                            MostrarRelatoProblema(idTarefa, nomeTarefa, relatoProblema);
                        };

                        painelTarefa.Controls.Add(btnVerProblema);

                        // ADICIONAR INDICADOR VISUAL DE QUE TEM PROBLEMA
                        Label lblTemProblema = new Label
                        {
                            Text = "⚠ Tem problema relatado",
                            Font = new Font("Segoe UI", 8, FontStyle.Bold),
                            ForeColor = Color.Red,
                            Left = 270,
                            Top = 90,
                            AutoSize = true
                        };
                        painelTarefa.Controls.Add(lblTemProblema);
                    }

                    // Checkbox para atraso justificado, se atrasada
                    CheckBox cbJustificado = null;
                    if (atrasada)
                    {
                        cbJustificado = new CheckBox
                        {
                            Text = "Atraso justificado",
                            Left = 10,
                            Top = 110,
                            AutoSize = true,
                            Name = "cbJustificado_" + idTarefa
                        };
                        cbJustificado.CheckedChanged += (s, e) =>
                        {
                            AtualizarJustificativa(idTarefa, cbJustificado.Checked);
                        };
                        painelTarefa.Controls.Add(cbJustificado);
                    }

                    panelAvaliacaoEquipes.Controls.Add(painelTarefa);

                    // Inicializa estado da avaliação para essa tarefa
                    avaliacoes[idTarefa] = new AvaliacaoInfo
                    {
                        Aceita = null,
                        AtrasoJustificado = cbJustificado != null ? (bool?)false : null
                    };

                    top += painelTarefa.Height + 10;
                }
            }
        }

        private void MostrarRelatoProblema(int idTarefa, string nomeTarefa, DataRow relatoProblema)
        {
            using (Form formRelato = new Form())
            {
                formRelato.Text = $"Relato de Problema - Tarefa: {nomeTarefa} (ID: {idTarefa})";
                formRelato.Size = new Size(600, 500);
                formRelato.StartPosition = FormStartPosition.CenterParent;
                formRelato.MaximizeBox = false;
                formRelato.MinimizeBox = false;
                formRelato.FormBorderStyle = FormBorderStyle.FixedDialog;

                // PAINEL PRINCIPAL
                Panel panelPrincipal = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20)
                };

                // LABELS COM INFORMAÇÕES DO RELATO
                Label lblEquipe = new Label
                {
                    Text = $"Equipe: {relatoProblema["nome_equipe"]}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, 10)
                };

                Label lblEmpresa = new Label
                {
                    Text = $"Empresa: {relatoProblema["nome_empresa"]}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, 40)
                };

                Label lblDescricao = new Label
                {
                    Text = "Descrição do Problema:",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, 80)
                };

                // TEXTBOX PARA A DESCRIÇÃO
                TextBox txtDescricao = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    Text = relatoProblema["descricao"].ToString(),
                    ScrollBars = ScrollBars.Vertical,
                    Location = new Point(10, 110),
                    Size = new Size(550, 250),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 10)
                };

                // BOTÃO FECHAR
                Button btnFechar = new Button
                {
                    Text = "Fechar",
                    Size = new Size(100, 35),
                    Location = new Point(235, 380),
                    DialogResult = DialogResult.OK,
                    BackColor = Color.SteelBlue,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                btnFechar.Click += (s, e) => formRelato.Close();

                // ADICIONAR CONTROLES AO PAINEL
                panelPrincipal.Controls.Add(lblEquipe);
                panelPrincipal.Controls.Add(lblEmpresa);
                panelPrincipal.Controls.Add(lblDescricao);
                panelPrincipal.Controls.Add(txtDescricao);
                panelPrincipal.Controls.Add(btnFechar);

                formRelato.Controls.Add(panelPrincipal);
                formRelato.ShowDialog();
            }
        }

        private void AtualizarAvaliacao(int idTarefa, bool aceita)
        {
            if (avaliacoes.ContainsKey(idTarefa))
            {
                avaliacoes[idTarefa].Aceita = aceita;
            }
        }

        private void AtualizarJustificativa(int idTarefa, bool justificado)
        {
            if (avaliacoes.ContainsKey(idTarefa))
            {
                avaliacoes[idTarefa].AtrasoJustificado = justificado;
            }
        }

        private void btnSalvarAvaliacoes_Click(object sender, EventArgs e)
        {
            AvaliacaoTarefa av_admin = new AvaliacaoTarefa();

            foreach (var item in avaliacoes)
            {
                int idTarefa = item.Key;
                AvaliacaoInfo info = item.Value;

                if (info.Aceita == null)
                {
                    MessageBox.Show($"Por favor, avalie a tarefa ID {idTarefa}.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // VERIFICAR SE TEM PROBLEMA RELATADO PARA AUXILIAR NA DECISÃO
                DataRow relatoProblema = av_admin.BuscarRelatoProblema(idTarefa);
                bool temProblema = relatoProblema != null;

                // SE TEM PROBLEMA E A TAREFA FOI ACEITA, PERGUNTAR SOBRE O ATRASO JUSTIFICADO
                if (temProblema && info.Aceita.Value && !info.AtrasoJustificado.HasValue)
                {
                    string descricaoProblema = relatoProblema["descricao"].ToString();
                    string nomeEquipe = relatoProblema["nome_equipe"].ToString();

                    var result = MessageBox.Show(
                        $"A equipe '{nomeEquipe}' relatou um problema nesta tarefa.\n\n" +
                        $"Problema: {descricaoProblema}\n\n" +
                        "Deseja considerar o atraso como justificado devido a este problema?",
                        "Problema Relatado",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    info.AtrasoJustificado = (result == DialogResult.Yes);
                }

                av_admin.SalvarAvaliacao(idTarefa, info.Aceita.Value, info.AtrasoJustificado);

                if (info.Aceita.Value)
                {
                    bool computarPontos = info.AtrasoJustificado.HasValue ? info.AtrasoJustificado.Value : true;
                    if (computarPontos)
                    {
                        AvancarPontuacaoFuncionarios(idTarefa);
                    }
                }
            }

            MessageBox.Show("Avaliações salvas com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CarregarTarefasNaoAvaliadas();
        }

        private void AvancarPontuacaoFuncionarios(int idTarefa)
        {
            AvaliacaoTarefa avaliacaoTarefa = new AvaliacaoTarefa();

            DataTable entregas = avaliacaoTarefa.BuscarEntregasPorTarefa(idTarefa);
            if (entregas == null || entregas.Rows.Count == 0)
                return;

            DataRow tarefa = avaliacaoTarefa.BuscarTarefaPorId(idTarefa);
            if (tarefa == null)
                return;

            string dificuldade = (tarefa["dificuldade"] != null) ? tarefa["dificuldade"].ToString().ToLower() : "";

            int pontos;

            switch (dificuldade)
            {
                case "difícil":
                    pontos = 30;
                    break;
                case "média":
                case "mediana":
                    pontos = 20;
                    break;
                case "fácil":
                    pontos = 10;
                    break;
                default:
                    pontos = 5;
                    break;
            }

            foreach (DataRow entrega in entregas.Rows)
            {
                int idFuncionario = Convert.ToInt32(entrega["FuncionarioId"]);
                avaliacaoTarefa.AtualizarPontuacaoFuncionario(idFuncionario, pontos);
            }
        }

        private List<int> BuscarIdsEquipes()
        {
            var ids = new List<int>();
            if (Sessao.AdminLogado == null)
                return ids;

            int adminId = int.Parse(Sessao.AdminLogado.getAdminId());
            string query = "SELECT id_equipe FROM Equipes WHERE AdminId = @adminId";

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd=;"))
            {
                conn.Open();
                var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@adminId", adminId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(Convert.ToInt32(reader["id_equipe"]));
                    }
                }
            }
            return ids;
        }

        private class AvaliacaoInfo
        {
            public bool? Aceita { get; set; }
            public bool? AtrasoJustificado { get; set; }
        }

        // Navegação e outros eventos (mantidos)
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

        private void btnEquipe_Click(object sender, EventArgs e)
        {
            PesquisaEquipes p_equipes = new PesquisaEquipes();
            p_equipes.Show();
            this.Hide();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            Planejamento t_pendentes = new Planejamento();
            t_pendentes.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas E_esta = new Equipes_Estatisticas();
            E_esta.Show();
            this.Hide();
        }

        private void AvaliaçãoTarefaAdmin_Load(object sender, EventArgs e)
        {
            CarregarTarefasNaoAvaliadas();
        }

        private void lblGeral_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes t_pendentes = new Chat_geral_equipes();
            t_pendentes.Show();
            this.Hide();
        }

        private void lblTarefas_Click(object sender, EventArgs e)
        {
            AvaliaçãoTarefaAdmin t_pendentes = new AvaliaçãoTarefaAdmin();
            t_pendentes.Show();
            this.Hide();
        }

        private void lblPlanejamento_Click(object sender, EventArgs e)
        {
            Planejamento t_pendentes = new Planejamento();
            t_pendentes.Show();
            this.Hide();
        }

        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_pendentes = new Integrantes_Equipe();
            t_pendentes.Show();
            this.Hide();
        }

        private void lblRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes t_pendentes = new Ranking_Equipes();
            t_pendentes.Show();
            this.Hide();
        }
    }
}
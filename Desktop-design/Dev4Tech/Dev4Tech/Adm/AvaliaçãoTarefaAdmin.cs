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
                CarregarFotoUsuario();
            }

            private void CarregarTarefasNaoAvaliadas()
            {
                panelAvaliacaoEquipes.Controls.Clear();
                avaliacoes.Clear();
                bool temTarefas = false;

                List<int> equipesIds = BuscarIdsEquipes();

                int top = 10;
                foreach (int idEquipe in equipesIds)
                {
                    AvaliacaoTarefa avaliacaoTarefa = new AvaliacaoTarefa();
                    DataTable tarefasNaoAvaliadas = avaliacaoTarefa.BuscarTarefasNaoAvaliadasPorEquipe(idEquipe);

                    foreach (DataRow tarefa in tarefasNaoAvaliadas.Rows)
                    {
                        temTarefas = true;

                        int idTarefa = Convert.ToInt32(tarefa["id_tarefa"]);
                        string nomeTarefa = tarefa["nomeTarefa"].ToString();
                        string nomeEquipe = tarefa["nome_equipe"].ToString();
                        string dificuldade = tarefa["dificuldade"].ToString();
                        string nomeArquivo = tarefa["nome_arquivo"]?.ToString();
                        string nomeFuncionario = tarefa["nome_funcionario"]?.ToString();

                        byte[] arquivoBytes = null;
                        if (tarefa["arquivo_blob"] != DBNull.Value)
                        {
                            arquivoBytes = (byte[])tarefa["arquivo_blob"];
                        }

                        bool temArquivo = !string.IsNullOrEmpty(nomeArquivo);
                        bool arquivoSalvoComoBlob = (arquivoBytes != null && arquivoBytes.Length > 0);

                        DataRow relatoProblema = avaliacaoTarefa.BuscarRelatoProblema(idTarefa);
                        bool temProblema = relatoProblema != null;
                        bool atrasada = Convert.ToDateTime(tarefa["data_entrega"]) < DateTime.Today;

                        // Design melhorado - cores suaves
                        Color corFundo = temProblema ? Color.LightYellow : (atrasada ? Color.LightCoral : Color.White);
                        Color corBorda = temProblema ? Color.Orange : (atrasada ? Color.Red : Color.LightGray);

                        // Altura ajustável
                        int alturaBase = 120;
                        if (atrasada) alturaBase += 20;
                        if (temProblema) alturaBase += 10;
                        if (temArquivo) alturaBase += 10;

                        Panel painelTarefa = new Panel
                        {
                            Width = panelAvaliacaoEquipes.Width - 40,
                            Height = alturaBase,
                            Top = top,
                            Left = 10,
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = corFundo,
                            Tag = idTarefa,
                            Cursor = Cursors.Default
                        };

                        // Título com fundo colorido
                        Label lblTitulo = new Label
                        {
                            Text = nomeTarefa,
                            Font = new Font("Poppins", 10, FontStyle.Bold),
                            BackColor = Color.SteelBlue,
                            ForeColor = Color.White,
                            Left = 0,
                            Top = 0,
                            Width = painelTarefa.Width,
                            Height = 25,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Padding = new Padding(10, 0, 0, 0)
                        };
                        painelTarefa.Controls.Add(lblTitulo);

                        int infoTop = 30;

                        // Informações básicas
                        Label lblEquipe = new Label
                        {
                            Text = "Equipe: " + nomeEquipe,
                            Font = new Font("Poppins", 9, FontStyle.Regular),
                            Left = 10,
                            Top = infoTop,
                            AutoSize = true
                        };
                        painelTarefa.Controls.Add(lblEquipe);

                        Label lblDificuldade = new Label
                        {
                            Text = "Dificuldade: " + dificuldade,
                            Font = new Font("Poppins", 9, FontStyle.Italic),
                            Left = 10,
                            Top = infoTop + 20,
                            AutoSize = true
                        };
                        painelTarefa.Controls.Add(lblDificuldade);

                        // Funcionário que entregou
                        if (!string.IsNullOrEmpty(nomeFuncionario))
                        {
                            Label lblFuncionario = new Label
                            {
                                Text = "Entregue por: " + nomeFuncionario,
                                Font = new Font("Poppins", 8, FontStyle.Italic),
                                Left = 200,
                                Top = infoTop,
                                AutoSize = true,
                                ForeColor = Color.Gray
                            };
                            painelTarefa.Controls.Add(lblFuncionario);
                        }

                        int controlesTop = infoTop + 45;

                        // RadioButtons com melhor espaçamento
                        RadioButton rbAceita = new RadioButton
                        {
                            Text = "✅ Aceita",
                            Left = 10,
                            Top = controlesTop,
                            AutoSize = true,
                            Name = "rbAceita_" + idTarefa,
                            Font = new Font("Poppins", 9, FontStyle.Regular)
                        };
                        rbAceita.CheckedChanged += (s, e) =>
                        {
                            if (rbAceita.Checked)
                                AtualizarAvaliacao(idTarefa, true);
                        };

                        RadioButton rbNegada = new RadioButton
                        {
                            Text = "❌ Negada",
                            Left = 90,
                            Top = controlesTop,
                            AutoSize = true,
                            Name = "rbNegada_" + idTarefa,
                            Font = new Font("Poppins", 9, FontStyle.Regular)
                        };
                        rbNegada.CheckedChanged += (s, e) =>
                        {
                            if (rbNegada.Checked)
                                AtualizarAvaliacao(idTarefa, false);
                        };

                        painelTarefa.Controls.Add(rbAceita);
                        painelTarefa.Controls.Add(rbNegada);

                        int leftPosition = 180;

                        // Botão do arquivo - SEMPRE que houver arquivo
                        if (temArquivo)
                        {
                            Button btnArquivo = new Button
                            {
                                Text = arquivoSalvoComoBlob ? "📥 Baixar Arquivo" : "📄 Abrir Arquivo",
                                Left = leftPosition,
                                Top = controlesTop,
                                Width = 130,
                                Height = 25,
                                BackColor = arquivoSalvoComoBlob ? Color.LightGreen : Color.LightBlue,
                                ForeColor = Color.Black,
                                Font = new Font("Poppins", 8, FontStyle.Bold),
                                Name = "btnArquivo_" + idTarefa,
                                Cursor = Cursors.Hand,
                                Tag = new { IdTarefa = idTarefa, IdEquipe = idEquipe }
                            };

                            btnArquivo.Click += (s, e) =>
                            {
                                Button botao = s as Button;
                                var info = botao?.Tag as dynamic;
                                if (info != null)
                                {
                                    AvaliacaoTarefa avTarefa = new AvaliacaoTarefa();
                                    avTarefa.CarregarArquivoTarefa(info.IdTarefa, info.IdEquipe);
                                }
                            };
                            painelTarefa.Controls.Add(btnArquivo);
                            leftPosition += 140;
                        }

                        // Botão do problema - APENAS se houver problema
                        if (temProblema)
                        {
                            Button btnVerProblema = new Button
                            {
                                Text = "⚠ Ver Problema",
                                Left = leftPosition,
                                Top = controlesTop,
                                Width = 110,
                                Height = 25,
                                BackColor = Color.Orange,
                                ForeColor = Color.White,
                                Font = new Font("Poppins", 8, FontStyle.Bold),
                                Name = "btnVerProblema_" + idTarefa,
                                Cursor = Cursors.Hand
                            };

                            btnVerProblema.Click += (s, e) =>
                            {
                                MostrarRelatoProblema(idTarefa, nomeTarefa, relatoProblema);
                            };
                            painelTarefa.Controls.Add(btnVerProblema);
                        }

                        // Checkbox de atraso justificado
                        if (atrasada)
                        {
                            CheckBox cbJustificado = new CheckBox
                            {
                                Text = "Atraso justificado",
                                Left = 10,
                                Top = controlesTop + 30,
                                AutoSize = true,
                                Name = "cbJustificado_" + idTarefa,
                                Font = new Font("Poppins", 8, FontStyle.Regular)
                            };
                            cbJustificado.CheckedChanged += (s, e) =>
                            {
                                AtualizarJustificativa(idTarefa, cbJustificado.Checked);
                            };
                            painelTarefa.Controls.Add(cbJustificado);
                        }

                        panelAvaliacaoEquipes.Controls.Add(painelTarefa);

                        // Inicializa estado da avaliação
                        avaliacoes[idTarefa] = new AvaliacaoInfo
                        {
                            Aceita = null,
                            AtrasoJustificado = atrasada ? (bool?)false : null
                        };

                        top += painelTarefa.Height + 10;
                    }
                }

                // Mensagem quando não há tarefas
                if (!temTarefas)
                {
                    Label lblMensagem = new Label
                    {
                        Text = "Não há tarefas pendentes para avaliação no momento.",
                        Font = new Font("Poppins", 11, FontStyle.Bold),
                        ForeColor = Color.Gray,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Width = panelAvaliacaoEquipes.Width - 40,
                        Height = 50,
                        Top = 50,
                        Left = 20
                    };
                    panelAvaliacaoEquipes.Controls.Add(lblMensagem);
                }

                // Atualiza visibilidade do botão de confirmar
                btnSalvarAvaliacoes.Visible = temTarefas;
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
                        Font = new Font("Poppins", 10, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };

                    Label lblEmpresa = new Label
                    {
                        Text = $"Empresa: {relatoProblema["nome_empresa"]}",
                        Font = new Font("Poppins", 10, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 40)
                    };

                    Label lblDescricao = new Label
                    {
                        Text = "Descrição do Problema:",
                        Font = new Font("Poppins", 10, FontStyle.Bold),
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
                        Font = new Font("Poppins", 10)
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
                        Font = new Font("Poppins", 9, FontStyle.Bold)
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
                            "Deseja considerar a entrega mesmo assim?",
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
                CarregarTarefasNaoAvaliadas(); // Recarrega a lista (e atualiza visibilidade do botão)
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

            private void AvaliaçãoTarefaAdmin_Load(object sender, EventArgs e)
            {
                CarregarTarefasNaoAvaliadas();
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
                AvaliaçãoTarefaAdmin t_pendentes = new AvaliaçãoTarefaAdmin();
                t_pendentes.Show();
                this.Hide();
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
                        PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
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
                    PesquisaEquipes rank = new PesquisaEquipes();
                    rank.Show();
                    this.Hide();
                }
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
        private void CarregarFotoUsuario()
        {
            try
            {
                var usuarioFoto = new UsuarioFoto();
                Image foto = usuarioFoto.ObterFotoUsuario();

                if (picPerfil != null) // Verifica se o controle existe no form
                {
                    if (foto != null)
                    {
                        picPerfil.Image = foto;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        // Usar imagem padrão se não encontrar foto
                        picPerfil.Image = Properties.Resources.icon_perfil;
                        picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar foto do usuário: {ex.Message}");
                if (picPerfil != null)
                {
                    picPerfil.Image = Properties.Resources.icon_perfil;
                    picPerfil.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }
        }
    }
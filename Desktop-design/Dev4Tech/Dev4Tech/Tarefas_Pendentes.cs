using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public partial class Tarefas_Pendentes : Form
    {
        private List<int> equipesFuncionario;
        private Dictionary<int, string> equipesNomeMap;
        private int idFuncionarioLogado; // ID do funcionário logado para filtro individual
        private EntregaTarefa entregaTarefa; // Instância da classe para manipular entregas
        private const string TextoPlaceholder = "Pesquisar uma tarefa";

        public Tarefas_Pendentes()
        {
            InitializeComponent();

            entregaTarefa = new EntregaTarefa();

            idFuncionarioLogado = Sessao.FuncionarioLogado != null
                ? int.Parse(Sessao.FuncionarioLogado.getFuncionarioId())
                : 0;

            // Configurar placeholder na textbox
            txtPesquisarTarefa.Text = TextoPlaceholder;
            txtPesquisarTarefa.ForeColor = Color.Gray;

            // Carregar as equipes do funcionário logado
            CarregarEquipes();
            CarregarFotoUsuario();

            // Vincular eventos
            cmbEquipes.SelectedIndexChanged += cmbEquipes_SelectedIndexChanged;
            txtPesquisarTarefa.TextChanged += txtPesquisarTarefa_TextChanged;

            // Carregar tarefas iniciais (todas equipes)
            AtualizarTarefas();
        }

        private void txtPesquisarTarefa_Enter(object sender, EventArgs e)
        {
            if (txtPesquisarTarefa.Text == TextoPlaceholder)
            {
                txtPesquisarTarefa.Text = "";
                txtPesquisarTarefa.ForeColor = Color.Black;
            }
        }

        private void txtPesquisarTarefa_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPesquisarTarefa.Text))
            {
                txtPesquisarTarefa.Text = TextoPlaceholder;
                txtPesquisarTarefa.ForeColor = Color.Gray;
            }
        }

        // Atualiza a lista de tarefas exibidas segundo filtros ativos (equipe/pesquisa)
        private void AtualizarTarefas()
        {
            string filtroNome = txtPesquisarTarefa.Text.Trim();
            List<int> equipesFiltrar = null;

            // Se for o texto placeholder ou vazio, não filtrar por nome
            bool usarFiltroNome = !string.IsNullOrEmpty(filtroNome) && filtroNome != TextoPlaceholder;

            if (cmbEquipes.SelectedItem == null || cmbEquipes.SelectedItem.ToString() == "Todas")
            {
                equipesFiltrar = equipesFuncionario; // todas as equipes do funcionário
            }
            else
            {
                var nomeEquipe = cmbEquipes.SelectedItem.ToString();
                equipesFiltrar = new List<int>();
                foreach (var kvp in equipesNomeMap)
                    if (kvp.Value == nomeEquipe)
                        equipesFiltrar.Add(kvp.Key);
            }

            DataTable tarefas = new DataTable();

            if (!usarFiltroNome)
            {
                tarefas = new DataTable();
                if (equipesFiltrar != null)
                {
                    foreach (var idEquipe in equipesFiltrar)
                    {
                        DataTable dtEquipe = entregaTarefa.BuscarTarefasPendentesPorEquipeFuncionario(idEquipe, idFuncionarioLogado);
                        tarefas.Merge(dtEquipe);
                    }
                }
            }
            else
            {
                // Busca tarefas pendentes para as equipes filtradas e filtra pelo nome
                DataTable tarefasPendentes = new DataTable();
                if (equipesFiltrar != null)
                {
                    foreach (var idEquipe in equipesFiltrar)
                    {
                        DataTable dtEquipe = entregaTarefa.BuscarTarefasPendentesPorEquipeFuncionario(idEquipe, idFuncionarioLogado);
                        tarefasPendentes.Merge(dtEquipe);
                    }
                }

                var rowsFiltrados = tarefasPendentes.AsEnumerable()
                    .Where(r => r.Field<string>("nomeTarefa").IndexOf(filtroNome, StringComparison.OrdinalIgnoreCase) >= 0);

                if (rowsFiltrados.Any())
                    tarefas = rowsFiltrados.CopyToDataTable();
                else
                    tarefas = tarefasPendentes.Clone(); // vazio porém com as colunas
            }

            MostrarTarefas(tarefas);
        }

        private void MostrarTarefas(DataTable tarefas)
        {
            AtualizarListaTarefas(tarefas);
        }


        public List<int> ObterEquipesDoFuncionario(int idFuncionario)
        {
            List<int> equipes = new List<int>();
            string query = "SELECT id_equipe FROM Equipes_Membros WHERE FuncionarioId = @idFuncionario";

            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        equipes.Add(reader.GetInt32("id_equipe"));
                    }
                }
            }
            return equipes;
        }

        public DataTable BuscarTarefasPorEquipes(List<int> idsEquipes)
        {
            DataTable dt = new DataTable();
            if (idsEquipes == null || idsEquipes.Count == 0)
                return dt;
            var parametros = idsEquipes.Select((id, index) => "@id" + index).ToList();
            string query = $"SELECT * FROM Tarefas WHERE id_equipe IN ({string.Join(", ", parametros)}) ORDER BY data_entrega ASC";
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    for (int i = 0; i < idsEquipes.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(parametros[i], idsEquipes[i]);
                    }
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }


        private void CarregarEquipes()
        {
            int idFunc = idFuncionarioLogado;
            equipesFuncionario = ObterEquipesDoFuncionario(idFunc);

            equipesNomeMap = new Dictionary<int, string>();

            foreach (var idEq in equipesFuncionario)
            {
                string nome = BuscarNomeEquipe(idEq);
                equipesNomeMap[idEq] = nome;
            }

            cmbEquipes.Items.Clear();
            cmbEquipes.Items.Add("Todas");
            cmbEquipes.Items.AddRange(equipesNomeMap.Values.ToArray());
            cmbEquipes.SelectedIndex = 0;
        }

        private string BuscarNomeEquipe(int idEquipe)
        {
            string nome = "";
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd="))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT nome_equipe FROM Equipes WHERE id_equipe = @id", conn);
                cmd.Parameters.AddWithValue("@id", idEquipe);
                var result = cmd.ExecuteScalar();
                if (result != null)
                    nome = result.ToString();
            }
            return nome;
        }

        private void cmbEquipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarTarefas();
        }

        // Pesquisa dinâmica na txtPesquisarTarefa e atualiza lista
        private void txtPesquisarTarefa_TextChanged(object sender, EventArgs e)
        {
            // Só atualiza se não for o texto do placeholder
            if (txtPesquisarTarefa.Text != TextoPlaceholder)
            {
                AtualizarTarefas();
            }
        }

        private void AtualizarListaTarefas(DataTable tarefas)
        {
            panelTarefas.Controls.Clear();

            if (tarefas.Rows.Count == 0)
            {
                Label lblSemTarefas = new Label
                {
                    Text = "Nenhuma tarefa pendente encontrada!",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Left = panelTarefas.Width / 2 - 120,
                    Top = 50
                };
                panelTarefas.Controls.Add(lblSemTarefas);
                return;
            }

            int margemTopo = 20;
            int margemEsquerda = 21;
            int espacamentoVertical = 20;
            int espacamentoHorizontal = 20;
            int larguraPanel = 600;
            int alturaPanel = 100;
            int colunas = 2;

            for (int i = 0; i < tarefas.Rows.Count; i++)
            {
                DataRow row = tarefas.Rows[i];
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
                    BackColor = Color.White
                };

                tarefaPanel.Click += (s, e) =>
                {
                    int idTarefa = Convert.ToInt32(((Panel)s).Tag);
                    int idEquipe = Convert.ToInt32(row["id_equipe"]);
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
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Left = larguraPanel - 120,
                    Top = 10,
                    AutoSize = true
                };

                bool entregou = entregaTarefa.FuncionarioEntregou(Convert.ToInt32(row["id_tarefa"]), idFuncionarioLogado);
                if (entregou)
                {
                    lblStatus.Text = "Entregue";
                    lblStatus.ForeColor = Color.White;
                    lblStatus.BackColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "Pendente";
                    lblStatus.ForeColor = Color.Black;
                    lblStatus.BackColor = Color.Yellow;
                }
                tarefaPanel.Controls.Add(lblStatus);

                Label lblDificuldade = new Label
                {
                    Text = "Dificuldade: " + dificuldade,
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.Black,
                    Left = larguraPanel - 120,
                    Top = 30,
                    AutoSize = true
                };
                switch (dificuldade.ToLower())
                {
                    case "difícil":
                        lblDificuldade.BackColor = Color.LightCoral;
                        break;
                    case "média":
                    case "mediana":
                        lblDificuldade.BackColor = Color.LightGoldenrodYellow;
                        break;
                    case "fácil":
                        lblDificuldade.BackColor = Color.LightGreen;
                        break;
                    default:
                        lblDificuldade.BackColor = Color.Transparent;
                        break;
                }
                tarefaPanel.Controls.Add(lblDificuldade);

                panelTarefas.Controls.Add(tarefaPanel);
            }
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;

            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void lblTarefas_Click(object sender, EventArgs e)
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

        private void lblMembros_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Integrantes_Equipe t_equipe = new Integrantes_Equipe();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AdicionarEquipes t_equipeAdmin = new AdicionarEquipes();
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
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Ranking_Equipes t_equipe = new Ranking_Equipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Ranking_Equipes t_equipeAdmin = new Ranking_Equipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
        private void btnEquipe_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                PesquisaEquipes t_equipe = new PesquisaEquipes();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                PesquisaEquipes t_equipeAdmin = new PesquisaEquipes();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                HomeAdm t_equipeAdmin = new HomeAdm();
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
            if (funcionario != null)
            {
                Planejamento t_equipe = new Planejamento();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Planejamento t_equipeAdmin = new Planejamento();
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
        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Planejamento t_equipe = new Planejamento();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                AdicionarTarefa t_equipeAdmin = new AdicionarTarefa();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void groupBox5_Enter(object sender, EventArgs e) { }
        private void panelTarefas_Paint(object sender, PaintEventArgs e) { }
        private void Tarefas_Pendentes_Load(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;
            if (funcionario != null)
            {
                Home t_equipe = new Home();
                t_equipe.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                HomeAdm t_equipeAdmin = new HomeAdm();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
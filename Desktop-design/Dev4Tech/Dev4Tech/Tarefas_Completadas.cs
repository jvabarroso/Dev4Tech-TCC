using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace Dev4Tech
{
    public partial class Tarefas_Completadas : Form
    {
        private List<int> equipesFuncionario;
        private Dictionary<int, string> equipesNomeMap;
        private int idFuncionarioLogado;
        private EntregaTarefa entregaTarefa;
        private const string TextoPlaceholder = "Pesquisar uma tarefa";
        private string baseRoot = @"C:\xampp\htdocs\dev4tech";
        private string baseImgFolder = @"C:\xampp\htdocs\dev4tech\img";

        public Tarefas_Completadas()
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
            txtPesquisarTarefa.Enter += txtPesquisarTarefa_Enter;
            txtPesquisarTarefa.Leave += txtPesquisarTarefa_Leave;
            txtPesquisarTarefa.TextChanged += txtPesquisarTarefa_TextChanged;

            // Carregar tarefas iniciais (todas equipes)
            AtualizarTarefas();
        }

        // MÉTODO PARA OBTER FOTO DA EQUIPE (MESMA LÓGICA DO PesquisaEquipes)
        private Image ObterFotoEquipeDosDados(object fotoData)
        {
            Image fotoEquipe = null;

            if (fotoData != null && fotoData != DBNull.Value)
            {
                if (fotoData is byte[] imageData)
                {
                    // É um blob - tentar carregar como imagem diretamente
                    try
                    {
                        using (var ms = new MemoryStream(imageData))
                        {
                            fotoEquipe = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao carregar imagem do blob: {ex.Message}");
                        // Tentar como string se falhar como imagem
                        try
                        {
                            string nomeArquivo = System.Text.Encoding.UTF8.GetString(imageData);
                            // LIMPAR O NOME DO ARQUIVO DE CARACTERES INVÁLIDOS
                            nomeArquivo = new string(nomeArquivo.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
                            string caminhoImagemEquipe = Path.Combine(baseImgFolder, nomeArquivo);
                            if (File.Exists(caminhoImagemEquipe))
                            {
                                fotoEquipe = Image.FromFile(caminhoImagemEquipe);
                            }
                        }
                        catch
                        {
                            // Se tudo falhar, retorna null e usará imagem padrão
                        }
                    }
                }
                else if (fotoData is string caminhoRelativo)
                {
                    // É um caminho
                    try
                    {
                        // LIMPAR O CAMINHO DE CARACTERES INVÁLIDOS
                        caminhoRelativo = new string(caminhoRelativo.Where(c => !Path.GetInvalidPathChars().Contains(c)).ToArray());
                        string caminhoCompleto = Path.Combine(baseRoot, caminhoRelativo.Replace("/", @"\"));
                        if (File.Exists(caminhoCompleto))
                        {
                            fotoEquipe = Image.FromFile(caminhoCompleto);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao carregar imagem do caminho: {ex.Message}");
                    }
                }
            }
            return fotoEquipe;
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
                        DataTable dtEquipe = entregaTarefa.BuscarTarefasCompletadasPorEquipe(idEquipe);
                        tarefas.Merge(dtEquipe);
                    }
                }
            }
            else
            {
                // Busca tarefas completadas para as equipes filtradas e filtra pelo nome
                DataTable tarefasCompletadas = new DataTable();
                if (equipesFiltrar != null)
                {
                    foreach (var idEquipe in equipesFiltrar)
                    {
                        DataTable dtEquipe = entregaTarefa.BuscarTarefasCompletadasPorEquipe(idEquipe);
                        tarefasCompletadas.Merge(dtEquipe);
                    }
                }

                var rowsFiltrados = tarefasCompletadas.AsEnumerable()
                    .Where(r => r.Field<string>("nomeTarefa").IndexOf(filtroNome, StringComparison.OrdinalIgnoreCase) >= 0);

                if (rowsFiltrados.Any())
                    tarefas = rowsFiltrados.CopyToDataTable();
                else
                    tarefas = tarefasCompletadas.Clone(); // vazio porém com as colunas
            }

            MostrarTarefas(tarefas);
        }

        private void MostrarTarefas(DataTable tarefas)
        {
            panelTarefas.Controls.Clear();

            if (tarefas.Rows.Count == 0)
            {
                Label lblSemTarefas = new Label
                {
                    Text = "Nenhuma tarefa completada encontrada!",
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
            int margemEsquerda = 20;
            int espacamentoVertical = 20;
            int espacamentoHorizontal = 20;
            int larguraPanel = 600;
            int alturaPanel = 100;
            int colunas = 2;

            for (int i = 0; i < tarefas.Rows.Count; i++)
            {
                DataRow row = tarefas.Rows[i];

                string dificuldade = row.Table.Columns.Contains("dificuldade") && row["dificuldade"] != DBNull.Value
                    ? row["dificuldade"].ToString()
                    : "Desconhecida";

                // Capturar os valores antes do evento
                int idTarefa = Convert.ToInt32(row["id_tarefa"]);
                int idEquipe = Convert.ToInt32(row["id_equipe"]);
                string nomeTarefa = row["nomeTarefa"].ToString();
                string nomeEquipe = row["nome_equipe"].ToString();
                string nomeCategoria = row["nome_categoria"].ToString();
                DateTime dataEntrega = Convert.ToDateTime(row["data_entrega"]);

                Panel tarefaPanel = new Panel
                {
                    Width = larguraPanel,
                    Height = alturaPanel,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Left = margemEsquerda + (i % colunas) * (larguraPanel + espacamentoHorizontal),
                    Top = margemTopo + (i / colunas) * (alturaPanel + espacamentoVertical),
                    Cursor = Cursors.Hand,
                    Tag = idTarefa
                };

                tarefaPanel.Click += (s, e) =>
                {
                    Tela_Tarefa telaTarefa = new Tela_Tarefa(idEquipe);
                    telaTarefa.CarregarDetalhesTarefa(idTarefa);
                    telaTarefa.Show();
                    this.Hide();
                };

                PictureBox pic = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 40,
                    Height = 40,
                    Left = 10,
                    Top = 10
                };

                // *** CORREÇÃO - CARREGAR FOTO DA EQUIPE (MESMA LÓGICA DO PesquisaEquipes) ***
                object fotoEquipeData = row["foto_equipe"];
                Image fotoEquipe = ObterFotoEquipeDosDados(fotoEquipeData);
                pic.Image = fotoEquipe ?? Properties.Resources.icon_EquipLogo;
                // *** FIM DA CORREÇÃO ***

                tarefaPanel.Controls.Add(pic);

                Label lblNome = new Label
                {
                    Text = nomeTarefa,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Left = 60,
                    Top = 5,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblNome);

                Label lblSub = new Label
                {
                    Text = nomeEquipe,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Left = 60,
                    Top = 30,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblSub);

                Label lblCategoria = new Label
                {
                    Text = nomeCategoria,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 60,
                    Top = 50,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblCategoria);

                Label lblConclusao = new Label
                {
                    Text = "Conclusão em " + dataEntrega.ToString("dd/MM/yy") + " às 00:00",
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 60,
                    Top = 70,
                    AutoSize = true
                };
                tarefaPanel.Controls.Add(lblConclusao);

                Label lblStatus = new Label
                {
                    Text = "Entregue",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Green,
                    Left = larguraPanel - 120,
                    Top = 10,
                    AutoSize = true
                };
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

        private void CarregarEquipes()
        {
            int idFunc = idFuncionarioLogado;
            equipesFuncionario = entregaTarefa.ObterEquipesDoFuncionario(idFunc);

            equipesNomeMap = new Dictionary<int, string>();

            foreach (var idEq in equipesFuncionario)
            {
                string nome = entregaTarefa.BuscarNomeEquipe(idEq);
                equipesNomeMap[idEq] = nome;
            }

            cmbEquipes.Items.Clear();
            cmbEquipes.Items.Add("Todas");
            cmbEquipes.Items.AddRange(equipesNomeMap.Values.ToArray());
            cmbEquipes.SelectedIndex = 0;
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

        // MÉTODOS DE BANCO DE DADOS (usando a classe EntregaTarefa)
        public List<int> ObterEquipesDoFuncionario(int idFuncionario)
        {
            return entregaTarefa.ObterEquipesDoFuncionario(idFuncionario);
        }

        private string BuscarNomeEquipe(int idEquipe)
        {
            return entregaTarefa.BuscarNomeEquipe(idEquipe);
        }

        // TODOS OS MÉTODOS DE NAVEGAÇÃO E EVENTOS ORIGINAIS (MANTIDOS)
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

        private void lblGeral_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (Sessao.IdEquipeSelecionada != 0)
            {
                string nomeEquipe = Sessao.NomeEquipeSelecionada;
                string categoriaEquipe = Sessao.CategoriaEquipeSelecionada;

                if (funcionario != null)
                {
                    Chat_geral_equipes t_equipe = new Chat_geral_equipes(Sessao.IdEquipeSelecionada, nomeEquipe, categoriaEquipe);
                    t_equipe.Show();
                    this.Hide();
                }
                else if (admin != null)
                {
                    Chat_geral_equipes t_equipeAdmin = new Chat_geral_equipes(Sessao.IdEquipeSelecionada, nomeEquipe, categoriaEquipe);
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

        private void btnPendentes_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes trf_Pendentes = new Tarefas_Pendentes();
            trf_Pendentes.Show();
            this.Hide();
        }

        private void btnEmAtraso_Click(object sender, EventArgs e)
        {
            Tarefas_Atrasadas trf_Atrasadas = new Tarefas_Atrasadas();
            trf_Atrasadas.Show();
            this.Hide();
        }

        private void btnCompletadas_Click(object sender, EventArgs e)
        {
            Tarefas_Completadas trf_Completas = new Tarefas_Completadas();
            trf_Completas.Show();
            this.Hide();
        }

        private void Tarefa1_Enter(object sender, EventArgs e)
        {
        }

        private void txtPesquisaTarefa_TextChanged(object sender, EventArgs e)
        {
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

        private void panelTarefas_Paint(object sender, PaintEventArgs e)
        {
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void Tarefas_Completadas_Load(object sender, EventArgs e)
        {
        }

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
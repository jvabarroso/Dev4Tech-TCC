using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Integrantes_Equipe : Form
    {
        private int equipeSelecionadaId = -1;
        private string baseFolder = @"C:\xampp\htdocs\dev4tech\";

        public Integrantes_Equipe()
        {
            InitializeComponent();
            CarregarEquipes();
        }

        private void Integrantes_Equipe_Load(object sender, EventArgs e)
        {
            // Opcional: CarregarEquipes();
        }

        private void CarregarEquipes()
        {
            panelEquipes.Controls.Clear();
            PesquisaIntegrantes dao = new PesquisaIntegrantes();
            DataTable equipes = dao.BuscarEquipesComCategoriaEMembros();
            int top = 10;
            foreach (DataRow row in equipes.Rows)
            {
                int idEquipe = Convert.ToInt32(row["id_equipe"]);
                string nomeEquipe = row["nome_equipe"].ToString();
                string categoria = row["nome_categoria"].ToString();
                Panel equipePanel = new Panel
                {
                    Width = 300,
                    Height = 70,
                    BackColor = Color.White,
                    Top = top,
                    Left = 10,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = idEquipe
                };
                PictureBox picEquipe = new PictureBox
                {
                    Image = Properties.Resources.icon_EquipLogo,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 40,
                    Height = 40,
                    Left = 10,
                    Top = 15
                };
                equipePanel.Controls.Add(picEquipe);
                Label lblNome = new Label
                {
                    Text = nomeEquipe,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Left = 60,
                    Top = 10,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblNome);
                Label lblCategoria = new Label
                {
                    Text = categoria,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 60,
                    Top = 35,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblCategoria);
                // Fotos dos membros (até 3)
                DataTable membros = dao.BuscarMembrosDaEquipe(idEquipe);
                int leftFoto = 200;
                int count = 0;
                foreach (DataRow m in membros.Rows)
                {
                    if (count >= 3) break;
                    PictureBox picMembro = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 32,
                        Height = 32,
                        Left = leftFoto,
                        Top = 20,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    // Carregar a foto do perfil se existir
                    if (m.Table.Columns.Contains("foto_perfil") && m["foto_perfil"] != DBNull.Value)
                    {
                        // Verificar se é um caminho (string) ou blob (byte[])
                        object fotoData = m["foto_perfil"];

                        if (fotoData is string caminhoRelativo && !string.IsNullOrEmpty(caminhoRelativo))
                        {
                            // É um caminho de arquivo
                            string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));

                            if (File.Exists(caminhoCompleto))
                            {
                                try
                                {
                                    picMembro.Image = Image.FromFile(caminhoCompleto);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erro ao carregar imagem: {ex.Message}");
                                    picMembro.Image = Properties.Resources.icon_perfil;
                                }
                            }
                            else
                            {
                                picMembro.Image = Properties.Resources.icon_perfil;
                            }
                        }
                        else if (fotoData is byte[] imageData)
                        {
                            // É um blob (para compatibilidade com dados antigos)
                            try
                            {
                                using (var ms = new System.IO.MemoryStream(imageData))
                                {
                                    picMembro.Image = Image.FromStream(ms);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao carregar imagem do blob: {ex.Message}");
                                picMembro.Image = Properties.Resources.icon_perfil;
                            }
                        }
                        else
                        {
                            picMembro.Image = Properties.Resources.icon_perfil;
                        }
                    }
                    else
                    {
                        picMembro.Image = Properties.Resources.icon_perfil;
                    }
                    equipePanel.Controls.Add(picMembro);
                    leftFoto += 35;
                    count++;
                }
                equipePanel.Click += (s, e) =>
                {
                    equipeSelecionadaId = idEquipe;
                    CarregarMembrosDaEquipe();
                };
                panelEquipes.Controls.Add(equipePanel);
                top += 80;
            }
        }

        private void CarregarMembrosDaEquipe(string filtroNome = "")
        {
            panelMembros.Controls.Clear();
            if (equipeSelecionadaId == -1) return;
            PesquisaIntegrantes dao = new PesquisaIntegrantes();
            DataTable membros = dao.BuscarMembrosDaEquipe(equipeSelecionadaId, filtroNome);
            int top = 10;
            foreach (DataRow row in membros.Rows)
            {
                Panel membroPanel = new Panel
                {
                    Width = 600,
                    Height = 60,
                    BackColor = Color.WhiteSmoke,
                    Top = top,
                    Left = 10,
                    BorderStyle = BorderStyle.FixedSingle
                };
                PictureBox pic = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 48,
                    Height = 48,
                    Left = 10,
                    Top = 6,
                    BorderStyle = BorderStyle.FixedSingle
                };
                // Carregar a foto do perfil
                if (row.Table.Columns.Contains("foto_perfil") && row["foto_perfil"] != DBNull.Value)
                {
                    // Verificar se é um caminho (string) ou blob (byte[])
                    object fotoData = row["foto_perfil"];

                    if (fotoData is string caminhoRelativo && !string.IsNullOrEmpty(caminhoRelativo))
                    {
                        // É um caminho de arquivo
                        string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));

                        if (File.Exists(caminhoCompleto))
                        {
                            try
                            {
                                pic.Image = Image.FromFile(caminhoCompleto);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao carregar imagem: {ex.Message}");
                                pic.Image = Properties.Resources.icon_perfil;
                            }
                        }
                        else
                        {
                            pic.Image = Properties.Resources.icon_perfil;
                        }
                    }
                    else if (fotoData is byte[] imageData)
                    {
                        // É um blob (para compatibilidade com dados antigos)
                        try
                        {
                            using (var ms = new System.IO.MemoryStream(imageData))
                            {
                                pic.Image = Image.FromStream(ms);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao carregar imagem do blob: {ex.Message}");
                            pic.Image = Properties.Resources.icon_perfil;
                        }
                    }
                    else
                    {
                        pic.Image = Properties.Resources.icon_perfil;
                    }
                }
                else
                {
                    pic.Image = Properties.Resources.icon_perfil;
                }
                membroPanel.Controls.Add(pic);
                Label lblNome = new Label
                {
                    Text = row["Nome"].ToString(),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Left = 70,
                    Top = 8,
                    AutoSize = true
                };
                membroPanel.Controls.Add(lblNome);
                Label lblEmail = new Label
                {
                    Text = "Email: " + row["Email"].ToString(),
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 70,
                    Top = 28,
                    AutoSize = true
                };
                membroPanel.Controls.Add(lblEmail);
                Label lblTelefone = new Label
                {
                    Text = "Telefone: " + row["Telefone"].ToString(),
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Left = 250,
                    Top = 28,
                    AutoSize = true
                };
                membroPanel.Controls.Add(lblTelefone);
                panelMembros.Controls.Add(membroPanel);
                top += 70;
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string filtro = txtPesquisarMembros.Text.Trim();
            CarregarMembrosDaEquipe(filtro);
        }

        // Eventos existentes mantidos...
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

        private void txtProcurarMebros_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnMostrarMembros_Click(object sender, EventArgs e)
        {
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

        private void btnRanking_Click_1(object sender, EventArgs e)
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

        private void btnConfigurações_Click(object sender, EventArgs e)
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

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Sessao.FuncionarioLogado = null;
            Sessao.AdminLogado = null;
            Form1 t_incial = new Form1();
            t_incial.Show();
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
                AdicionarTarefa t_equipeAdmin = new AdicionarTarefa();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label1_Click(object sender, EventArgs e)
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
                HomeAdm t_equipeAdmin = new HomeAdm();
                t_equipeAdmin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
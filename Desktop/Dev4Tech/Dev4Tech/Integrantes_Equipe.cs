using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Integrantes_Equipe : Form
    {
        private int equipeSelecionadaId = -1;
        private string baseFolder = @"C:\xampp\htdocs\dev4tech\";
        private string basePathImagemEquipe = @"C:\xampp\htdocs\dev4tech\img";

        public Integrantes_Equipe()
        {
            InitializeComponent();
            CarregarEquipes();
            CarregarFotoUsuario();
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
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 40,
                    Height = 40,
                    Left = 10,
                    Top = 15
                };

                // CARREGAR FOTO DA EQUIPE - MESMA LÓGICA DO PRIMEIRO CÓDIGO
                object fotoEquipeData = row["foto_equipe"];
                Image fotoEquipe = ObterFotoEquipeDosDados(fotoEquipeData);
                picEquipe.Image = fotoEquipe ?? Properties.Resources.icon_EquipLogo;

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

                    // CARREGAR FOTO DO MEMBRO - MESMA LÓGICA DO PRIMEIRO CÓDIGO
                    object fotoMembroData = m["foto_perfil"];
                    Image fotoMembro = ObterFotoMembroDosDados(fotoMembroData);
                    picMembro.Image = fotoMembro ?? Properties.Resources.icon_perfil;

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

                // CARREGAR FOTO DO MEMBRO - MESMA LÓGICA DO PRIMEIRO CÓDIGO
                object fotoMembroData = row["foto_perfil"];
                Image fotoMembro = ObterFotoMembroDosDados(fotoMembroData);
                pic.Image = fotoMembro ?? Properties.Resources.icon_perfil;

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

        // MÉTODO PARA OBTER FOTO DA EQUIPE (COPIADO DO PRIMEIRO CÓDIGO)
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
                            string caminhoImagemEquipe = Path.Combine(basePathImagemEquipe, nomeArquivo);
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
                        string caminhoCompleto = Path.Combine(baseFolder, caminhoRelativo.Replace("/", @"\"));
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

        // MÉTODO PARA OBTER FOTO DO MEMBRO (LÓGICA SIMPLIFICADA)
        private Image ObterFotoMembroDosDados(object fotoData)
        {
            Image fotoMembro = null;

            if (fotoData != null && fotoData != DBNull.Value)
            {
                if (fotoData is byte[] imageData)
                {
                    // É um blob - carregar diretamente da memória
                    try
                    {
                        using (var ms = new MemoryStream(imageData))
                        {
                            ms.Position = 0;
                            fotoMembro = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        fotoMembro = null;
                    }
                }
                else if (fotoData is string caminhoRelativo)
                {
                    // É um caminho
                    try
                    {
                        string caminhoCorrigido = caminhoRelativo.Replace("/", "\\");
                        string caminhoCompleto = Path.Combine(baseFolder, caminhoCorrigido);
                        if (File.Exists(caminhoCompleto))
                        {
                            using (var imgTemp = Image.FromFile(caminhoCompleto))
                            {
                                fotoMembro = new Bitmap(imgTemp);
                            }
                        }
                    }
                    catch
                    {
                        fotoMembro = null;
                    }
                }
            }
            return fotoMembro;
        }

        // RESTANTE DOS MÉTODOS DO FORMULÁRIO (event handlers)
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string filtro = txtPesquisarMembros.Text.Trim();
            CarregarMembrosDaEquipe(filtro);
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
                string nomeEquipe = "Nome da equipe";
                string categoriaEquipe = "Categoria da equipe";

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

        // Métodos vazios necessários para o designer
        private void txtProcurarMebros_TextChanged(object sender, EventArgs e) { }
        private void btnMostrarMembros_Click(object sender, EventArgs e) { }
        private void Integrantes_Equipe_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }


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
// Ranking_Equipes.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Ranking_Equipes : Form
    {
        private EquipesRanking equipesRanking = new EquipesRanking();

        public Ranking_Equipes()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Ranking_Equipes_Load);

        }

        private void Ranking_Equipes_Load(object sender, EventArgs e)
        {
            CarregarRankingEquipes();
            CarregarFotoUsuario();
        }

        private void CarregarRankingEquipes()
        {
            panelRankingEquipes.Controls.Clear();
            EquipesRanking dao = new EquipesRanking();
            DataTable dtEquipes = dao.BuscarEquipesComPontuacao();
            int top = 10;
            int rank = 1;

            foreach (DataRow row in dtEquipes.Rows)
            {
                // CAPTURAR OS VALORES ANTES DO EVENTO
                int idEquipe = Convert.ToInt32(row["id_equipe"]);
                string nomeEquipe = row["nome_equipe"].ToString();
                int pontosEquipe = row["pontos"] != DBNull.Value ? Convert.ToInt32(row["pontos"]) : 0;
                int currentRank = rank; // Capturar o rank atual

                List<MembroEquipe> membros = dao.BuscarMembrosEquipe(idEquipe);

                int alturaPainel = 90;
                Panel equipePanel = new Panel
                {
                    Width = panelRankingEquipes.Width - 40,
                    Height = alturaPainel,
                    BackColor = Color.White,
                    Top = top,
                    Left = 10,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = idEquipe,
                    Cursor = Cursors.Hand
                };

                PictureBox picIcone = new PictureBox
                {
                    Width = 44,
                    Height = 44,
                    Left = 10,
                    Top = 20,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                if (currentRank == 1)
                    picIcone.Image = Properties.Resources.icon_ranking_1;
                else if (currentRank == 2)
                    picIcone.Image = Properties.Resources.icon_ranking_2;
                else if (currentRank == 3)
                    picIcone.Image = Properties.Resources.icon_ranking_3;
                equipePanel.Controls.Add(picIcone);

                Label lblRank = new Label
                {
                    Text = $"#{currentRank}",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Left = picIcone.Right + 10,
                    Top = 18,
                    ForeColor = Color.Black,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblRank);

                Label lblNomeEquipe = new Label
                {
                    Text = nomeEquipe,
                    Font = new Font("Segoe UI", 13, FontStyle.Bold),
                    Left = picIcone.Right + 60,
                    Top = 18,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblNomeEquipe);

                Label lblPontos = new Label
                {
                    Text = "Pontuação: " + pontosEquipe,
                    Font = new Font("Segoe UI", 11, FontStyle.Regular),
                    ForeColor = Color.DodgerBlue,
                    Left = picIcone.Right + 60,
                    Top = lblNomeEquipe.Bottom + 5,
                    AutoSize = true
                };
                equipePanel.Controls.Add(lblPontos);

                // Mostrar até 3 fotos dos membros com ToolTip com nome
                int numMembrosParaMostrar = Math.Min(membros.Count, 3);
                int leftFoto = equipePanel.Width - 40 - 35 * numMembrosParaMostrar;
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
                        Top = 25,
                        BorderStyle = BorderStyle.FixedSingle,
                        Cursor = Cursors.Hand
                    };
                    toolTipMembros.SetToolTip(picMembro, membros[i].Nome);
                    equipePanel.Controls.Add(picMembro);
                }

                // USAR VARIÁVEIS CAPTURADAS NO EVENTO
                equipePanel.Click += (s, e) =>
                {
                    Equipes_Estatisticas estatisticas = new Equipes_Estatisticas();
                    estatisticas.idEquipe = idEquipe; // Usar variável capturada
                    estatisticas.CarregarGrafico();
                    estatisticas.Show();
                    this.Hide();
                };

                panelRankingEquipes.Controls.Add(equipePanel);
                top += equipePanel.Height + 10;
                rank++;
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

        private void btnEquipes_Click(object sender, EventArgs e)
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

        private void label40_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox9_Click(object sender, EventArgs e)
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

        private void pictureBox6_Click(object sender, EventArgs e)
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

        private void lblMembros_Click(object sender, EventArgs e)
        {
            var funcionario = Sessao.FuncionarioLogado;
            var admin = Sessao.AdminLogado;

            if (funcionario != null)
            {
                Integrantes_Equipe IE = new Integrantes_Equipe();
                IE.Show();
                this.Hide();
            }
            else if (admin != null)
            {
                Integrantes_Equipe IE = new Integrantes_Equipe();
                IE.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nenhum usuário logado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnEstatisticas_Click(object sender, EventArgs e)
        {
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

        private void panelRankingEquipes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Ranking_Equipes_Load_1(object sender, EventArgs e)
        {

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
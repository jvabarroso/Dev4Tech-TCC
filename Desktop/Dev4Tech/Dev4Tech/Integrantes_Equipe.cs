using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class Integrantes_Equipe : Form
    {
        private int equipeSelecionadaId = -1;

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
                        Image = Properties.Resources.icon_perfil,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 32,
                        Height = 32,
                        Left = leftFoto,
                        Top = 20,
                        BorderStyle = BorderStyle.FixedSingle
                    };
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
                    Image = Properties.Resources.icon_perfil,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 48,
                    Height = 48,
                    Left = 10,
                    Top = 6,
                    BorderStyle = BorderStyle.FixedSingle
                };
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

        // Seus eventos existentes abaixo (mantidos)
        private void lblMembros_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_integrantes = new Integrantes_Equipe();
            t_integrantes.Show();
            this.Hide();
        }

        private void lblPlanejamento_Click(object sender, EventArgs e)
        {
            Planejamento p_plano = new Planejamento();
            p_plano.Show();
            this.Hide();
        }

        private void txtProcurarMebros_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMostrarMembros_Click(object sender, EventArgs e)
        {

        }

        private void lblRanking_Click(object sender, EventArgs e) {

            Ranking_Equipes rank_equipe = new Ranking_Equipes();
            rank_equipe.Show();
            this.Hide();
        }
        private void lblTarefas_Click(object sender, EventArgs e) {
            Tarefas_Pendentes trf_pendente = new Tarefas_Pendentes();
            trf_pendente.Show();
            this.Hide();
        }
        private void lblGeral_Click(object sender, EventArgs e) {

            Chat_geral_equipes chatEquipe = new Chat_geral_equipes();
            chatEquipe.Show();
            this.Hide();
        }

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Hide();
        }

        private void btnEquipes_Click_1(object sender, EventArgs e)
        {
            PesquisaEquipes P_equipe = new PesquisaEquipes();
            P_equipe.Show();
            this.Hide();
        }

        private void btnRanking_Click_1(object sender, EventArgs e)
        {
            Ranking_Equipes rank = new Dev4Tech.Ranking_Equipes();
            rank.Show();
            this.Hide();
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
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }
    }
}

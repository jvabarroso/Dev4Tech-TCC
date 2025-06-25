using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class PesquisaEquipes : Form
    {
        public PesquisaEquipes()
        {
            InitializeComponent();
            panelEquipes.AutoScroll = true;

            filtroEquipes.Items.Add("Todos");
            filtroEquipes.Items.Add("Desenvolvedor de software");
            filtroEquipes.Items.Add("Design");
            filtroEquipes.Items.Add("Marketing");
            filtroEquipes.SelectedIndex = 0;
        }

        private int mensagensCount = 0;
        private int margemTopo = 30;
        private int margemEsquerda = 350;
        private int espacamentoVertical = 20;
        private int alturaMensagem = 110;

        private void txtPesquisaEquipe_Click(object sender, EventArgs e)
        {
            txtPesquisaEquipe.Text = "";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home t_Home = new Home();
            t_Home.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes t_Ranking = new Ranking_Equipes();
            t_Ranking.Show();
            this.Hide();
        }

        private void txtPesquisarEquipe_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPesquisaEquipe.Text))
            {
                txtPesquisaEquipe.Text = "Pesquisar Equipe";
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarEquipes(filtroEquipes.SelectedItem?.ToString());
        }

        private void CarregarEquipes(string filtroCategoria = null)
        {
            panelEquipes.Controls.Clear();
            mensagensCount = 0;

            FiltroEquipes filtro = new FiltroEquipes();
            DataTable dt = filtro.ObterEquipesComMembros(filtroCategoria);

            var equipes = dt.AsEnumerable()
                            .GroupBy(row => new
                            {
                                id_equipe = row.Field<int>("id_equipe"),
                                nome_equipe = row.Field<string>("nome_equipe"),
                                categoria = row.Field<string>("nome_categoria"),
                                dias_desde_ultima_atividade = row.IsNull("dias_desde_ultima_atividade") ? -1 : Convert.ToInt32(row["dias_desde_ultima_atividade"])
                            });

            foreach (var equipe in equipes)
            {
                AdicionarPainelEquipe(
                    equipe.Key.nome_equipe,
                    equipe.Key.categoria,
                    equipe.Select(r => r.Field<string>("nome_funcionario")).ToList(),
                    equipe.Key.id_equipe,
                    equipe.Key.dias_desde_ultima_atividade
                );
            }
        }

        private void AdicionarPainelEquipe(string nomeEquipe, string categoria, System.Collections.Generic.List<string> membros, int idEquipe, int diasDesdeUltimaAtividade)
        {
            int x = margemEsquerda;
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;

            Panel equipePanel = new Panel
            {
                Width = 350,
                Height = alturaMensagem,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Left = x,
                Top = y,
                Cursor = Cursors.Hand
            };

            equipePanel.Click += (s, e) =>
            {
                Chat_geral_equipes chatForm = new Chat_geral_equipes(idEquipe, nomeEquipe, categoria);
                chatForm.Show();
                this.Hide();
            };

            PictureBox picEquipe = new PictureBox
            {
                Image = Properties.Resources.icon_EquipLogo,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 40,
                Height = 40,
                Left = 10,
                Top = 10,
                BorderStyle = BorderStyle.FixedSingle
            };
            equipePanel.Controls.Add(picEquipe);

            Label lblNomeEquipe = new Label
            {
                Text = nomeEquipe,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 60,
                Top = 5,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblNomeEquipe);

            Label lblCategoria = new Label
            {
                Text = categoria,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Left = 60,
                Top = 30,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblCategoria);

            string textoAtividade = diasDesdeUltimaAtividade == -1
                ? "Nunca ativo"
                : $"Ativo há {diasDesdeUltimaAtividade} dia(s)";

            Label lblAtividade = new Label
            {
                Text = textoAtividade,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = diasDesdeUltimaAtividade > 7 ? Color.Red : Color.Green,
                Left = 60,
                Top = 50,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblAtividade);

            Label lblColaboradores = new Label
            {
                Text = "Colaboradores",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Left = 60,
                Top = 70,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblColaboradores);

            int fotoLeft = 60;
            int fotoTop = 90;
            foreach (var membro in membros)
            {
                PictureBox picMembro = new PictureBox
                {
                    Image = Properties.Resources.icon_perfil,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 32,
                    Height = 32,
                    Left = fotoLeft,
                    Top = fotoTop,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                    Tag = membro
                };
                picMembro.MouseHover += (s, e) =>
                {
                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(picMembro, membro);
                };

                equipePanel.Controls.Add(picMembro);
                fotoLeft += 38;
            }

            panelEquipes.Controls.Add(equipePanel);
            mensagensCount++;
        }

        // Métodos e eventos que você já tinha (mantidos)
        private void btnEquipe_Click(object sender, EventArgs e) {

            PesquisaEquipes P_equipe = new PesquisaEquipes();
            P_equipe.Show();
            this.Hide();
        }
        private void btnCalendar_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }
        private void txtPesquisaEquipe_TextChanged(object sender, EventArgs e) { }
        private void filtroEquipes_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }

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

        private void PesquisaEquipes_Load(object sender, EventArgs e)
        {

        }
    }
}

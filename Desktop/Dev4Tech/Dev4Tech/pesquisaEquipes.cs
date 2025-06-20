using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class PesquisaEquipes : Form
    {
        public PesquisaEquipes()
        {
            InitializeComponent();

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
        private int alturaMensagem = 50;

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
                                categoria = row.Field<string>("nome_categoria")
                            });

            foreach (var equipe in equipes)
            {
                AdicionarPainelEquipe(equipe.Key.nome_equipe, equipe.Key.categoria, equipe.Select(r => r.Field<string>("nome_funcionario")).ToList());
            }
        }

        private void AdicionarPainelEquipe(string nomeEquipe, string categoria, List<string> membros)
        {
            int x = margemEsquerda;
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount;

            Panel equipePanel = new Panel
            {
                Width = 600,
                Height = 80,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Left = x,
                Top = y
            };

            PictureBox picEquipe = new PictureBox
            {
                Image = Properties.Resources.icon_EquipLogo,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 50,
                Height = 50,
                Left = 10,
                Top = 15,
                BorderStyle = BorderStyle.FixedSingle
            };
            equipePanel.Controls.Add(picEquipe);

            Label lblNomeEquipe = new Label
            {
                Text = nomeEquipe,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Left = 70,
                Top = 10,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblNomeEquipe);

            Label lblCategoria = new Label
            {
                Text = "Categoria: " + categoria,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Left = 70,
                Top = 35,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblCategoria);

            int fotoLeft = 300;
            foreach (var membro in membros)
            {
                PictureBox picMembro = new PictureBox
                {
                    Image = Properties.Resources.icon_perfil,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 32,
                    Height = 32,
                    Left = fotoLeft,
                    Top = 24,
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
                fotoLeft += 40;
            }

            panelEquipes.Controls.Add(equipePanel);
            mensagensCount++;
        }

        private void btnEquipe_Click(object sender, EventArgs e) { }
        private void btnCalendar_Click(object sender, EventArgs e) { }
        private void txtPesquisaEquipe_TextChanged(object sender, EventArgs e) { }
        private void filtroEquipes_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panelEquipes_Paint(object sender, PaintEventArgs e) { }
    }
}

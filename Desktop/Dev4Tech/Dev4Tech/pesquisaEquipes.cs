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
            filtroEquipes.SelectedIndex = 0; //Seleciona "Todos" por padrão
        }

        private int mensagensCount = 0;
        private int margemTopo = 30;
        private int margemEsquerda = 350;
        private int espacamentoVertical = 20;
        private int alturaMensagem = 50;

        private void AdicionarEquipes(string texto)
        {
            int x = margemEsquerda; // fixa na coluna esquerda
            int y = margemTopo + (alturaMensagem + espacamentoVertical) * mensagensCount; // empilha verticalmente

            // Cria painel da mensagem
            Panel EquipesPanel = new Panel();
            EquipesPanel.Width = 350;
            EquipesPanel.Height = alturaMensagem;
            EquipesPanel.BackColor = Color.White;
            EquipesPanel.BorderStyle = BorderStyle.FixedSingle;
            EquipesPanel.Left = x;
            EquipesPanel.Top = y;

            // Avatar
            PictureBox avatar = new PictureBox();
            avatar.Image = Properties.Resources.icon_equip;
            avatar.SizeMode = PictureBoxSizeMode.StretchImage;
            avatar.Width = 32;
            avatar.Height = 32;
            avatar.Left = 10;
            avatar.Top = 6;
            avatar.BorderStyle = BorderStyle.FixedSingle;

            // Label da mensagem
            Label lblMensagem = new Label();
            lblMensagem.Text = texto;
            lblMensagem.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblMensagem.AutoSize = false;
            lblMensagem.Width = 270;
            lblMensagem.Height = 32;
            lblMensagem.Left = 50;
            lblMensagem.Top = 6;
            lblMensagem.TextAlign = ContentAlignment.MiddleLeft;

            EquipesPanel.Controls.Add(avatar);
            EquipesPanel.Controls.Add(lblMensagem);

            panelEquipes.Controls.Add(EquipesPanel);

            mensagensCount++;
        }

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
<<<<<<< HEAD

        private void CarregarEquipes(string filtroCategoria = null)
        {
            panelEquipes.Controls.Clear();
            mensagensCount = 0;

            FiltroEquipes filtro = new FiltroEquipes();
            DataTable dt = filtro.ObterEquipesComMembros(filtroCategoria);

            // Agrupar por equipe para exibir membros juntos
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

            // Foto da equipe (ícone fixo)
            PictureBox picEquipe = new PictureBox
            {
                Image = Properties.Resources.icon_equip,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 50,
                Height = 50,
                Left = 10,
                Top = 15,
                BorderStyle = BorderStyle.FixedSingle
            };
            equipePanel.Controls.Add(picEquipe);

            // Nome da equipe
            Label lblNomeEquipe = new Label
            {
                Text = nomeEquipe,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Left = 70,
                Top = 10,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblNomeEquipe);

            // Categoria
            Label lblCategoria = new Label
            {
                Text = "Categoria: " + categoria,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Left = 70,
                Top = 35,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblCategoria);

            // Fotos fixas dos membros (exemplo: 32x32 cada)
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
                    Tag = membro // armazena o nome do membro para uso futuro (ex: tooltip)
                };
                picMembro.MouseHover += (s, e) =>
                {
                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(picMembro, membro);
                };

                equipePanel.Controls.Add(picMembro);
                fotoLeft += 40; // espaço entre as fotos
            }

            panelEquipes.Controls.Add(equipePanel);
            mensagensCount++;
        }

=======
>>>>>>> c1e5d468858d85b13d37cd5c5733fe2d1fcfd1ef

        private void btnEquipe_Click(object sender, EventArgs e) { }
        private void btnCalendar_Click(object sender, EventArgs e) { }
        private void txtPesquisaEquipe_TextChanged(object sender, EventArgs e)
        {

        }

        private void filtroEquipes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panelEquipes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CarregarEquipes(string filtroCategoria = null)
        {
            panelEquipes.Controls.Clear();
            mensagensCount = 0;

            FiltroEquipes filtro = new FiltroEquipes();
            DataTable dt = filtro.ObterEquipesComMembros(filtroCategoria);

            // Agrupar por equipe para exibir membros juntos
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

            // Foto da equipe (ícone fixo)
            PictureBox picEquipe = new PictureBox
            {
                Image = Properties.Resources.icon_EquipLogo, // sua imagem fixa da equipe
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 50,
                Height = 50,
                Left = 10,
                Top = 15,
                BorderStyle = BorderStyle.FixedSingle
            };
            equipePanel.Controls.Add(picEquipe);

            // Nome da equipe
            Label lblNomeEquipe = new Label
            {
                Text = nomeEquipe,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Left = 70,
                Top = 10,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblNomeEquipe);

            // Categoria
            Label lblCategoria = new Label
            {
                Text = "Categoria: " + categoria,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Left = 70,
                Top = 35,
                AutoSize = true
            };
            equipePanel.Controls.Add(lblCategoria);

            // Fotos fixas dos membros (exemplo: 32x32 cada)
            int fotoLeft = 300;
            foreach (var membro in membros)
            {
                PictureBox picMembro = new PictureBox
                {
                    Image = Properties.Resources.icon_perfil, // sua imagem fixa do membro
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 32,
                    Height = 32,
                    Left = fotoLeft,
                    Top = 24,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand,
                    Tag = membro // armazena o nome do membro para uso futuro (ex: tooltip)
                };
                picMembro.MouseHover += (s, e) =>
                {
                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(picMembro, membro);
                };

                equipePanel.Controls.Add(picMembro);
                fotoLeft += 40; // espaço entre as fotos
            }

            panelEquipes.Controls.Add(equipePanel);
            mensagensCount++;
        }

    }
}

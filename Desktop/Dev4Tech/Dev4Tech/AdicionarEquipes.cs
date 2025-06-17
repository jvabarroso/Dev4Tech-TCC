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
    public partial class AdicionarEquipes : Form
    {
<<<<<<< HEAD

        AddEquipes equipe = new AddEquipes();
        private List<string> membrosSelecionados = new List<string>();

        public AdicionarEquipes()
        {
            InitializeComponent();
            this.Load += AdicionarEquipes_Load;
            btnAddMembro.Click += btnAddMembro_Click;
            btnCriarEquipe.Click += btnCriarEquipe_Click;

        }

        private void AdicionarEquipes_Load(object sender, EventArgs e)
        {
            CarregarCategorias();
            CarregarEmailsFuncionarios();

            cmbCategoriaEquipe.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void CarregarCategorias()
        {
            try
            {
                DataTable dtCategorias = equipe.ConsultarCategorias();
                cmbCategoriaEquipe.Items.Clear();

                foreach (DataRow row in dtCategorias.Rows)
                {
                    cmbCategoriaEquipe.Items.Add(row["nome_categoria"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
            }
        }
        private void CarregarEmailsFuncionarios()
        {
            try
            {
                DataTable dtEmails = equipe.ConsultarEmailsFuncionarios();
                cbmEmailMembro.Items.Clear();

                foreach (DataRow row in dtEmails.Rows)
                {
                    cbmEmailMembro.Items.Add(row["email"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar emails: " + ex.Message);
            }
=======
        public AdicionarEquipes()
        {
            InitializeComponent();
>>>>>>> 84c1efa37aa2833043764843579cbc8d64b56f55
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home t_Home = new Home();
            t_Home.Show();
            this.Hide();
        }

        private void btnEquipes_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas t_Equipes = new Equipes_Estatisticas();
            t_Equipes.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Ranking_Equipes t_Ranking = new Ranking_Equipes();
            t_Ranking.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnTarefas_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_Tarefas = new Tarefas_Pendentes();
            t_Tarefas.Show();
            this.Hide();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            Chat_geral_equipes t_Chat = new Chat_geral_equipes();
            t_Chat.Show();
            this.Hide();
        }

        private void btnIntegrantes_Click(object sender, EventArgs e)
        {
            Integrantes_Equipe t_Integrantes = new Integrantes_Equipe();
            t_Integrantes.Show();
            this.Hide();
        }

<<<<<<< HEAD
=======
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            // TODO: Implementar adição de equipe
            MessageBox.Show("Funcionalidade em desenvolvimento");
        }

>>>>>>> 84c1efa37aa2833043764843579cbc8d64b56f55
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            PesquisaEquipes pesquisaEquipes = new PesquisaEquipes();
            pesquisaEquipes.Show();
            this.Hide();
        }

        private void txtNomeEquipe_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbCategoriaEquipe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pbImgEquipe_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
<<<<<<< HEAD

        private void cbmEmailMembro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddMembro_Click(object sender, EventArgs e)
        {
            string emailSelecionado = cbmEmailMembro.Text.Trim();

            if (!string.IsNullOrEmpty(emailSelecionado) && !membrosSelecionados.Contains(emailSelecionado))
            {
                membrosSelecionados.Add(emailSelecionado);
                MessageBox.Show($"Membro {emailSelecionado} adicionado.");
                // Aqui você pode atualizar uma lista visual para mostrar os membros adicionados, se desejar
            }
            else
            {
                MessageBox.Show("Selecione um email válido que ainda não foi adicionado.");
            }
        }

        private void btnCriarEquipe_Click(object sender, EventArgs e)
        {
            try
            {
                string nomeEquipe = txtNomeEquipe.Text.Trim();
                string categoria = cmbCategoriaEquipe.Text.Trim();

                if (string.IsNullOrEmpty(nomeEquipe))
                {
                    MessageBox.Show("Informe o nome da equipe.");
                    return;
                }

                if (string.IsNullOrEmpty(categoria))
                {
                    MessageBox.Show("Informe a categoria da equipe.");
                    return;
                }

                if (membrosSelecionados.Count == 0)
                {
                    MessageBox.Show("Adicione pelo menos um membro (email) para a equipe.");
                    return;
                }

                equipe.setNomeEquipe(nomeEquipe);
                equipe.setCategoria(categoria);

                // Insere equipe e obtém o id
                int idEquipe = equipe.InserirEquipeRetornandoId();

                // Insere membros na tabela associativa
                foreach (string email in membrosSelecionados)
                {
                    equipe.InserirMembroEquipe(idEquipe, email);
                }

                MessageBox.Show("Equipe cadastrada com sucesso!");
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar equipe: " + ex.Message);
            }
        }


        private void LimparFormulario()
        {
            txtNomeEquipe.Clear();
            cmbCategoriaEquipe.Text = "";
            cbmEmailMembro.Text = "";
            membrosSelecionados.Clear();
            // Atualize a lista visual de membros adicionados, se houver
        }
=======
>>>>>>> 84c1efa37aa2833043764843579cbc8d64b56f55
    }
}

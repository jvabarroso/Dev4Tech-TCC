using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public partial class Tela_Tarefa : Form
    {
        private int idEquipeAtual = 0;
        private int idTarefaExibida = 0;
        private string caminhoArquivoEntrega = "";
        private int idFuncionarioAtual = 0;
        private DataRow entregaAtual = null;
        private string nomeArquivoEntrega = "";
        private bool eventosConfigurados = false;

        public Tela_Tarefa(int idEquipe)
        {
            InitializeComponent();
            idEquipeAtual = idEquipe;
            idFuncionarioAtual = Sessao.FuncionarioLogado != null ? int.Parse(Sessao.FuncionarioLogado.getFuncionarioId()) : 0;
            txtNomeEquipe.Text = BuscarNomeEquipe(idEquipeAtual);

            // Configurar eventos UMA ÚNICA VEZ
            ConfigurarEventos();
            CarregarFotoUsuario();

            btnRelatarProblema.Click -= btnRelatarProblema_Click;
            btnRelatarProblema.Click += btnRelatarProblema_Click;
        }

        private void ConfigurarEventos()
        {
            if (!eventosConfigurados)
            {
                // Remover event handlers existentes para evitar duplicação
                lblArquivoEntregaTarefa.Click -= LblArquivoEntregaTarefa_Click;
                lblArquivoEntregaTarefa.Click -= LblArquivoEntregaVisualizar_Click;

                // Adicionar event handler para anexar arquivo
                lblArquivoEntregaTarefa.Click += LblArquivoEntregaTarefa_Click;

                eventosConfigurados = true;
            }
        }

        private int ObterPontuacaoPorDificuldade(int idTarefa)
        {
            int pontuacao = 0;
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd=;"))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT dificuldade FROM Tarefas WHERE id_tarefa = @id", conn);
                cmd.Parameters.AddWithValue("@id", idTarefa);
                var result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string dificuldade = result.ToString();
                    switch (dificuldade)
                    {
                        case "Fácil":
                            pontuacao = 10;
                            break;
                        case "Média":
                            pontuacao = 20;
                            break;
                        case "Difícil":
                            pontuacao = 30;
                            break;
                    }
                }
            }
            return pontuacao;
        }

        public void CarregarDetalhesTarefa(int idTarefa)
        {
            EntregaTarefa entrTarefa = new EntregaTarefa();
            DataRow tarefa = entrTarefa.BuscarTarefaPorId(idTarefa);

            if (tarefa != null)
            {
                idTarefaExibida = Convert.ToInt32(tarefa["id_tarefa"]);

                lblNomeTarefa.Text = tarefa["nomeTarefa"].ToString();
                lblCategoriaEquipe.Text = tarefa["nome_categoria"].ToString();

                DateTime dataEntrega = Convert.ToDateTime(tarefa["data_entrega"]);
                lblDataEntrega.Text = dataEntrega.ToString("dd/MM/yyyy");

                lblInstrucoes.Text = tarefa["instrucoes"].ToString();

                int pontuacao = ObterPontuacaoPorDificuldade(idTarefa);
                label13.Text = $"{pontuacao}";

                if (tarefa.Table.Columns.Contains("dificuldade") && tarefa["dificuldade"] != DBNull.Value)
                {
                    lblDificuldade.Text = "Dificuldade: " + tarefa["dificuldade"].ToString();
                    lblDificuldade.Visible = true;
                }
                else
                {
                    lblDificuldade.Visible = false;
                }

                lblArquivoTarefa.Click -= LblArquivoTarefa_Click;
                if (tarefa["nome_arquivo"] != DBNull.Value && !string.IsNullOrEmpty(tarefa["nome_arquivo"].ToString()))
                {
                    lblArquivoTarefa.Text = "Arquivo: " + tarefa["nome_arquivo"].ToString();
                    lblArquivoTarefa.ForeColor = Color.Blue;
                    lblArquivoTarefa.Cursor = Cursors.Hand;
                    lblArquivoTarefa.Click += LblArquivoTarefa_Click;
                }
                else
                {
                    lblArquivoTarefa.Text = "Nenhum arquivo anexado à tarefa.";
                    lblArquivoTarefa.ForeColor = SystemColors.ControlText;
                    lblArquivoTarefa.Cursor = Cursors.Default;
                }

                btnEnviar.Enabled = true;
                LimparCamposEntrega();

                CarregarEntregaFuncionario(idTarefa);
                AtualizarEstadoEntrega();
            }
            else
            {
                LimparDetalhesTarefa();
            }
        }

        private void CarregarEntregaFuncionario(int idTarefa)
        {
            if (idFuncionarioAtual == 0) return;

            EntregaTarefa entrTarefa = new EntregaTarefa();
            entregaAtual = entrTarefa.BuscarEntregaPorTarefaEFuncionario(idTarefa, idFuncionarioAtual);

            // SEMPRE remover ambos os eventos primeiro para evitar conflitos
            lblArquivoEntregaTarefa.Click -= LblArquivoEntregaTarefa_Click;
            lblArquivoEntregaTarefa.Click -= LblArquivoEntregaVisualizar_Click;

            if (entregaAtual != null)
            {
                txtDescrição.Text = entregaAtual["descricao"].ToString();

                if (entregaAtual["nome_arquivo"] != DBNull.Value && !string.IsNullOrEmpty(entregaAtual["nome_arquivo"].ToString()))
                {
                    nomeArquivoEntrega = entregaAtual["nome_arquivo"].ToString();
                    lblArquivoEntregaTarefa.Text = "Arquivo entregue: " + nomeArquivoEntrega;
                    lblArquivoEntregaTarefa.ForeColor = Color.Blue;
                    lblArquivoEntregaTarefa.Cursor = Cursors.Hand;

                    // Adicionar APENAS o evento de visualização
                    lblArquivoEntregaTarefa.Click += LblArquivoEntregaVisualizar_Click;
                }
                else
                {
                    lblArquivoEntregaTarefa.Text = "Nenhum arquivo foi anexado na entrega";
                    lblArquivoEntregaTarefa.ForeColor = SystemColors.ControlText;
                    lblArquivoEntregaTarefa.Cursor = Cursors.Default;
                }

                txtDescrição.Enabled = false;
                btnEnviar.Enabled = false;
            }
            else
            {
                txtDescrição.Enabled = true;
                btnEnviar.Enabled = true;

                // Configurar para modo de anexar arquivo
                lblArquivoEntregaTarefa.Text = "Clique para anexar arquivo";
                lblArquivoEntregaTarefa.ForeColor = Color.Gray;
                lblArquivoEntregaTarefa.Cursor = Cursors.Hand;

                // Adicionar APENAS o evento de anexar arquivo
                lblArquivoEntregaTarefa.Click += LblArquivoEntregaTarefa_Click;
            }
        }

        private void LblArquivoEntregaVisualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nomeArquivoEntrega)) return;

            try
            {
                string pastaArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";
                string caminhoArquivo = Path.Combine(pastaArquivos, nomeArquivoEntrega);

                if (File.Exists(caminhoArquivo))
                    System.Diagnostics.Process.Start(caminhoArquivo);
                else
                    MessageBox.Show("Arquivo de entrega não encontrado no servidor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o arquivo de entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparDetalhesTarefa()
        {
            idTarefaExibida = 0;
            lblInstrucoes.Text = "";
            lblArquivoTarefa.Text = "";
            btnEnviar.Enabled = false;
            lblCategoriaEquipe.Text = "";
            LimparCamposEntrega();
        }

        private void LblArquivoTarefa_Click(object sender, EventArgs e)
        {
            if (idTarefaExibida == 0) return;

            EntregaTarefa entrTarefa = new EntregaTarefa();
            DataRow tarefa = entrTarefa.BuscarTarefaPorId(idTarefaExibida);

            if (tarefa != null && tarefa["nome_arquivo"] != DBNull.Value)
            {
                try
                {
                    string nomeArquivo = tarefa["nome_arquivo"].ToString();
                    string pastaArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";
                    string caminhoArquivo = Path.Combine(pastaArquivos, nomeArquivo);

                    if (File.Exists(caminhoArquivo))
                        System.Diagnostics.Process.Start(caminhoArquivo);
                    else
                        MessageBox.Show("Arquivo não encontrado no servidor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o arquivo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nenhum arquivo anexado à tarefa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string BuscarNomeEquipe(int idEquipe)
        {
            string nome = "";
            using (var conn = new MySqlConnection("server=localhost;database=Dev4Tech;uid=root;pwd=;"))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT nome_equipe FROM Equipes WHERE id_equipe = @id", conn);
                cmd.Parameters.AddWithValue("@id", idEquipe);
                var result = cmd.ExecuteScalar();
                nome = result != null ? result.ToString() : "";
            }
            return nome;
        }

        private void LblArquivoEntregaTarefa_Click(object sender, EventArgs e)
        {
            // VERIFICAR SE JÁ EXISTE UMA JANELA ABERTA
            if (Application.OpenForms.Count > 1) // Mais de 1 form aberto (incluindo este)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is FileDialog)
                    {
                        form.BringToFront();
                        return; // Já existe uma janela de arquivo aberta
                    }
                    continue;
                }
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Todos os arquivos (*.*)|*.*";
            ofd.Multiselect = false; // Garantir que só selecione um arquivo

            // Configurar para evitar múltiplas instâncias
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string pastaArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";
                if (!Directory.Exists(pastaArquivos))
                    Directory.CreateDirectory(pastaArquivos);

                string arquivoSelecionado = ofd.FileName;
                string extensao = Path.GetExtension(arquivoSelecionado);
                string nomeArquivoUnico = Guid.NewGuid().ToString() + extensao;

                string caminhoDestino = Path.Combine(pastaArquivos, nomeArquivoUnico);

                try
                {
                    File.Copy(arquivoSelecionado, caminhoDestino, true);
                    caminhoArquivoEntrega = caminhoDestino;
                    lblArquivoEntregaTarefa.Text = Path.GetFileName(caminhoArquivoEntrega);
                    lblArquivoEntregaTarefa.ForeColor = Color.Blue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao copiar arquivo para pasta: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (idTarefaExibida == 0)
            {
                MessageBox.Show("Não há tarefa para ser entregue.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescrição.Text))
            {
                MessageBox.Show("Por favor, descreva sua entrega.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idFuncionarioAtual = Sessao.FuncionarioLogado != null ? int.Parse(Sessao.FuncionarioLogado.getFuncionarioId()) : 0;
            if (idFuncionarioAtual == 0)
            {
                MessageBox.Show("Funcionário não está logado corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            EntregaTarefa entrTarefa = new EntregaTarefa();
            if (entrTarefa.FuncionarioEntregou(idTarefaExibida, idFuncionarioAtual))
            {
                MessageBox.Show("Você já entregou essa tarefa e não pode entregar novamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string pastaArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";
            if (!Directory.Exists(pastaArquivos))
                Directory.CreateDirectory(pastaArquivos);

            string nomeArquivo = null;
            if (!string.IsNullOrEmpty(caminhoArquivoEntrega))
            {
                try
                {
                    string extensao = Path.GetExtension(caminhoArquivoEntrega);
                    nomeArquivo = Guid.NewGuid().ToString() + extensao;
                    string caminhoCompleto = Path.Combine(pastaArquivos, nomeArquivo);
                    File.Copy(caminhoArquivoEntrega, caminhoCompleto, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar arquivo de entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            try
            {
                entrTarefa.RegistrarEntrega(idTarefaExibida, idEquipeAtual, idFuncionarioAtual, txtDescrição.Text, nomeArquivo, null);

                MessageBox.Show("Entrega registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregarEntregaFuncionario(idTarefaExibida);
                AtualizarEstadoEntrega();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar a entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCamposEntrega()
        {
            txtDescrição.Clear();

            // Remover eventos antes de modificar o texto
            lblArquivoEntregaTarefa.Click -= LblArquivoEntregaTarefa_Click;
            lblArquivoEntregaTarefa.Click -= LblArquivoEntregaVisualizar_Click;

            lblArquivoEntregaTarefa.Text = "Clique para anexar arquivo";
            lblArquivoEntregaTarefa.ForeColor = Color.Gray;
            caminhoArquivoEntrega = "";
            nomeArquivoEntrega = "";
            entregaAtual = null;

            // Re-adicionar o evento padrão (anexar arquivo)
            lblArquivoEntregaTarefa.Click += LblArquivoEntregaTarefa_Click;
        }

        // ... OS MÉTODOS DE NAVEGAÇÃO PERMANECEM OS MESMOS ...
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
            PesquisaEquipes p_equipe = new PesquisaEquipes();
            p_equipe.Show();
            this.Hide();
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

        private void btnRelatarProblema_Click(object sender, EventArgs e)
        {
            if (idTarefaExibida == 0 || idEquipeAtual == 0)
            {
                MessageBox.Show("Selecione uma tarefa válida para relatar um problema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // VERIFICAR SE A TAREFA JÁ FOI AVALIADA
            AvaliacaoTarefa avaliacao = new AvaliacaoTarefa();
            bool tarefaAvaliada = avaliacao.TarefaFoiAvaliada(idTarefaExibida);

            if (tarefaAvaliada)
            {
                MessageBox.Show("Esta tarefa já foi avaliada pelo administrador e não é mais possível relatar problemas.", "Tarefa Avaliada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Relato_Problema relatoForm = new Relato_Problema(idTarefaExibida, idEquipeAtual);
            relatoForm.Show();
            this.Hide();
        }

        private void txtDescrição_TextChanged(object sender, EventArgs e) { }

        private void btnConfigurações_Click(object sender, EventArgs e)
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

        private void pictureBox9_Click(object sender, EventArgs e)
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

        private void label5_Click(object sender, EventArgs e)
        {
            Tarefas_Pendentes t_pendente = new Tarefas_Pendentes();
            t_pendente.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e) { }

        private void btnDesfazerEntrega_Click(object sender, EventArgs e)
        {
            if (idTarefaExibida == 0)
            {
                MessageBox.Show("Nenhuma tarefa selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idFuncionarioAtual = int.Parse(Sessao.FuncionarioLogado.getFuncionarioId());
            EntregaTarefa entrTarefa = new EntregaTarefa();

            try
            {
                entrTarefa.RemoverEntrega(idTarefaExibida, idFuncionarioAtual);
                MessageBox.Show("Entrega desfeita. Agora você pode entregar novamente.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregarEntregaFuncionario(idTarefaExibida);
                AtualizarEstadoEntrega();
                CarregarDetalhesTarefa(idTarefaExibida);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao desfazer entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarEstadoEntrega()
        {
            if (idTarefaExibida == 0 || Sessao.FuncionarioLogado == null)
            {
                btnDesfazerEntrega.Visible = false;
                btnEnviar.Enabled = false;
                lblStatusFinalizado.Visible = false;
                return;
            }

            int idFuncionarioAtual = int.Parse(Sessao.FuncionarioLogado.getFuncionarioId());
            EntregaTarefa entrTarefa = new EntregaTarefa();

            bool jaEntregou = entrTarefa.FuncionarioEntregou(idTarefaExibida, idFuncionarioAtual);
            bool todosEntregaram = entrTarefa.TodosEntregaram(idTarefaExibida, idEquipeAtual);

            btnDesfazerEntrega.Visible = jaEntregou;
            btnEnviar.Enabled = !jaEntregou;

            lblStatusFinalizado.Visible = todosEntregaram;
            if (todosEntregaram)
            {
                btnDesfazerEntrega.Enabled = false;
                btnEnviar.Enabled = false;
            }
        }

        private void label13_Click(object sender, EventArgs e)
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
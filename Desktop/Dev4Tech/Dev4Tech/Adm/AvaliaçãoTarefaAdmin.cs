using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Dev4Tech
{
    public partial class AvaliaçãoTarefaAdmin : Form
    {
        private Dictionary<int, AvaliacaoInfo> avaliacoes = new Dictionary<int, AvaliacaoInfo>();

        public AvaliaçãoTarefaAdmin()
        {
            InitializeComponent();
            CarregarTarefasEntregues();
        }

        private void CarregarTarefasEntregues()
        {
            panelAvaliacaoEquipes.Controls.Clear();

            List<int> equipesIds = BuscarIdsEquipes();

            int top = 10;
            foreach (int idEquipe in equipesIds)
            {
                EntregaTarefa entregaTarefa = new EntregaTarefa();
                DataTable tarefasEntregues = entregaTarefa.BuscarTarefasCompletadasPorEquipe(idEquipe);

                foreach (DataRow tarefa in tarefasEntregues.Rows)
                {
                    int idTarefa = Convert.ToInt32(tarefa["id_tarefa"]);
                    string nomeTarefa = tarefa["nomeTarefa"].ToString();
                    string nomeEquipe = tarefa["nome_equipe"].ToString();
                    string dificuldade = tarefa["dificuldade"].ToString();

                    bool atrasada = Convert.ToDateTime(tarefa["data_entrega"]) < DateTime.Today;

                    Panel painelTarefa = new Panel
                    {
                        Width = panelAvaliacaoEquipes.Width - 40,
                        Height = atrasada ? 140 : 110,
                        Top = top,
                        Left = 10,
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = idTarefa,
                        Cursor = Cursors.Hand
                    };

                    // Evento para abrir Tela_TarefaAdmin ao clicar
                    painelTarefa.Click += (s, e) =>
                    {
                        int tarefaId = Convert.ToInt32(((Panel)s).Tag);
                        Tela_TarefasAdmin telaTarefaAdmin = new Tela_TarefasAdmin(tarefaId);
                        telaTarefaAdmin.Show();
                        this.Hide();
                    };

                    Label lblNome = new Label
                    {
                        Text = "Tarefa: " + nomeTarefa,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Left = 10,
                        Top = 10,
                        AutoSize = true
                    };
                    painelTarefa.Controls.Add(lblNome);

                    Label lblEquipe = new Label
                    {
                        Text = "Equipe: " + nomeEquipe,
                        Font = new Font("Segoe UI", 9, FontStyle.Regular),
                        Left = 10,
                        Top = 35,
                        AutoSize = true
                    };
                    painelTarefa.Controls.Add(lblEquipe);

                    Label lblDificuldade = new Label
                    {
                        Text = "Dificuldade: " + dificuldade,
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        Left = 10,
                        Top = 60,
                        AutoSize = true
                    };
                    painelTarefa.Controls.Add(lblDificuldade);

                    RadioButton rbAceita = new RadioButton
                    {
                        Text = "Aceita",
                        Left = 10,
                        Top = 85,
                        AutoSize = true,
                        Name = "rbAceita_" + idTarefa
                    };
                    rbAceita.CheckedChanged += (s, e) => AtualizarAvaliacao(idTarefa, true);

                    RadioButton rbNegada = new RadioButton
                    {
                        Text = "Negada",
                        Left = 80,
                        Top = 85,
                        AutoSize = true,
                        Name = "rbNegada_" + idTarefa
                    };
                    rbNegada.CheckedChanged += (s, e) => AtualizarAvaliacao(idTarefa, false);

                    painelTarefa.Controls.Add(rbAceita);
                    painelTarefa.Controls.Add(rbNegada);

                    CheckBox cbJustificado = null;
                    if (atrasada)
                    {
                        cbJustificado = new CheckBox
                        {
                            Text = "Atraso justificado",
                            Left = 10,
                            Top = 110,
                            AutoSize = true,
                            Name = "cbJustificado_" + idTarefa
                        };
                        cbJustificado.CheckedChanged += (s, e) => AtualizarJustificativa(idTarefa, cbJustificado.Checked);
                        painelTarefa.Controls.Add(cbJustificado);
                    }

                    panelAvaliacaoEquipes.Controls.Add(painelTarefa);

                    avaliacoes[idTarefa] = new AvaliacaoInfo
                    {
                        Aceita = null,
                        AtrasoJustificado = cbJustificado != null ? (bool?)false : null
                    };

                    top += painelTarefa.Height + 10;
                }
            }
        }

        private void AtualizarAvaliacao(int idTarefa, bool aceita)
        {
            if (avaliacoes.ContainsKey(idTarefa))
            {
                avaliacoes[idTarefa].Aceita = aceita;
            }
        }

        private void AtualizarJustificativa(int idTarefa, bool justificado)
        {
            if (avaliacoes.ContainsKey(idTarefa))
            {
                avaliacoes[idTarefa].AtrasoJustificado = justificado;
            }
        }

        private void btnSalvarAvaliacoes_Click(object sender, EventArgs e)
        {
            AvaliacaoTarefa av_admin = new AvaliacaoTarefa();

            foreach (var item in avaliacoes)
            {
                int idTarefa = item.Key;
                AvaliacaoInfo info = item.Value;

                if (info.Aceita == null)
                {
                    MessageBox.Show($"Por favor, avalie a tarefa ID {idTarefa}.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                av_admin.SalvarAvaliacao(idTarefa, info.Aceita.Value, info.AtrasoJustificado);
            }

            MessageBox.Show("Avaliações salvas com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<int> BuscarIdsEquipes()
        {
            // Implementar busca real no banco, aqui exemplo fixo:
            return new List<int> { 1, 2, 3 };
        }

        private class AvaliacaoInfo
        {
            public bool? Aceita { get; set; }
            public bool? AtrasoJustificado { get; set; }
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void btnEquipe_Click(object sender, EventArgs e)
        {
            PesquisaEquipes p_equipes = new PesquisaEquipes();
            p_equipes.Show();
            this.Hide();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            AvaliaçãoTarefaAdmin t_pendentes = new AvaliaçãoTarefaAdmin();
            t_pendentes.Show();
            this.Hide();
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            Equipes_Estatisticas E_esta = new Equipes_Estatisticas();
            E_esta.Show();
            this.Hide();
        }

        private void AvaliaçãoTarefaAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}

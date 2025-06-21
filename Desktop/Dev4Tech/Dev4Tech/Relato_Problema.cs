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
    public partial class Relato_Problema : Form
    {
        public Relato_Problema()
        {
            InitializeComponent();
        }

        private void txtDescriçãoProblema_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // Pega o texto da TextBox e remove espaços extras
            string descricao = txtDescriçãoProblema.Text.Trim();

            if (string.IsNullOrEmpty(descricao))
            {
                MessageBox.Show("Por favor, escreva a descrição do problema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cria o objeto e atribui a descrição
            enviarProblema problem = new enviarProblema
            {
                descrição = descricao
            };

            try
            {
                // Chama o método para inserir no banco
                problem.Inserir();
                MessageBox.Show("Problema enviado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDescriçãoProblema.Clear(); // Limpa o campo após enviar
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar problema: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}

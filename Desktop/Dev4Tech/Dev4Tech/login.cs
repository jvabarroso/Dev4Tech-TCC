using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void lblCadastrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cadastro_funcionário cad_funcionario = new cadastro_funcionário();
            cad_funcionario.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = textBox1.Text;
                string senha = textBox2.Text;

                // Primeiro verifica se é um administrador
                string queryAdmin = "SELECT * FROM Administradores WHERE Email = @email AND Senha = @senha";
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=Dev4Tech;Uid=root;Pwd=;"))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(queryAdmin, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@senha", senha);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string tipoUsuario = reader["tipo_usuario"].ToString();
                                if (tipoUsuario == "admin")
                                {
                                    HomeAdm t_HomeAdm = new HomeAdm();
                                    t_HomeAdm.Show();
                                    this.Hide();
                                }
                                else // gestor
                                {
                                    Home t_Home = new Home();
                                    t_Home.Show();
                                    this.Hide();
                                }
                                return;
                            }
                        }
                    }

                    // Se não encontrou como administrador, verifica se é funcionário
                    string queryFunc = "SELECT * FROM Funcionarios WHERE Email = @email AND Senha = @senha";
                    using (MySqlCommand cmd = new MySqlCommand(queryFunc, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@senha", senha);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Home t_Home = new Home();
                                t_Home.Show();
                                this.Hide();
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show("Email ou senha incorretos!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao fazer login: {ex.Message}");
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 t_incial = new Form1();
            t_incial.Show();
            this.Hide();
        }
    }
}

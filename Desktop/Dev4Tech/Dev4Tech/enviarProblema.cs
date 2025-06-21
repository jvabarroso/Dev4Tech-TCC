using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    class enviarProblema : conexao
    {
        public int idProblema { get; set; }
        public string descrição { get; set; }

        public void Inserir()
        {
            string query = "insert into RelatoProblema (idProblema, descrição) values (@idProblema, @descr)";

            if (abrirConexao())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    cmd.Parameters.AddWithValue("@idProblema", idProblema);
                    cmd.Parameters.AddWithValue("@descr", descrição);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    fecharConexao();
                }
            }
        }
    }
    }

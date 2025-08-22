// EquipesRanking.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace Dev4Tech
{
    public class MembroEquipe
    {
        public string Nome { get; set; }
        public Image FotoPerfil { get; set; }
    }

    public class EquipesRanking : conexao
    {
        private string conexaoString = "server=localhost;database=Dev4Tech;uid=root;pwd=;";

        public DataTable BuscarEquipesComPontuacao()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT e.id_equipe, e.nome_equipe, 
                       IFNULL(SUM(pf.pontos),0) AS pontos
                FROM Equipes e
                LEFT JOIN Equipes_Membros em ON em.id_equipe = e.id_equipe
                LEFT JOIN PontuacaoFuncionario pf ON pf.id_funcionario = em.FuncionarioId
                GROUP BY e.id_equipe, e.nome_equipe
                ORDER BY pontos DESC, e.data_criacao ASC
            ";

            using (var conn = new MySqlConnection(conexaoString))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public List<MembroEquipe> BuscarMembrosEquipe(int idEquipe)
        {
            List<MembroEquipe> membros = new List<MembroEquipe>();

            string query = @"
                SELECT f.Nome, f.foto_perfil
                FROM Equipes_Membros em
                INNER JOIN Funcionarios f ON f.FuncionarioId = em.FuncionarioId
                WHERE em.id_equipe = @idEquipe
                ORDER BY f.Nome";

            using (var conn = new MySqlConnection(conexaoString))
            {
                conn.Open();

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idEquipe", idEquipe);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MembroEquipe membro = new MembroEquipe();
                        membro.Nome = reader["Nome"].ToString();

                        if (!reader.IsDBNull(reader.GetOrdinal("foto_perfil")))
                        {
                            byte[] fotoBytes = (byte[])reader["foto_perfil"];
                            using (var ms = new System.IO.MemoryStream(fotoBytes))
                            {
                                membro.FotoPerfil = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            membro.FotoPerfil = Properties.Resources.icon_perfil;
                        }

                        membros.Add(membro);
                    }
                }
            }

            return membros;
        }
    }
}

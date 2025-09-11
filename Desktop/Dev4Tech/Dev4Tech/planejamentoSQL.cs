using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Dev4Tech
{
    // Classe para representar dados da tarefa com arquivo
    public class TarefaArquivo
    {
        public int IdTarefa { get; set; }
        public string NomeArquivo { get; set; }
    }

    public class planejamentoSQL : conexao
    {
        public List<int> ObterIdsEquipesFuncionario(int idFuncionario)
        {
            var idsEquipes = new List<int>();
            string query = "SELECT id_equipe FROM Equipes_Membros WHERE FuncionarioId = @idFuncionario";
            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idsEquipes.Add(reader.GetInt32("id_equipe"));
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return idsEquipes;
        }

        public string ObterNomeArquivoTarefa(int idTarefa)
{
    string nomeArquivo = null;
    string query = "SELECT nome_arquivo FROM Tarefas WHERE id_tarefa=@idTarefa";
    if (abrirConexao())
    {
        try
        {
            using (var cmd = new MySqlCommand(query, conectar))
            {
                cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                var resultado = cmd.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                    nomeArquivo = resultado.ToString();
            }
        }
        finally
        {
            fecharConexao();
        }
    }
    return nomeArquivo;
}


        public List<TarefaArquivo> ObterTarefasPendentesPorEquipesComArquivo(List<int> idsEquipes)
        {
            var tarefas = new List<TarefaArquivo>();
            if (idsEquipes == null || idsEquipes.Count == 0)
                return tarefas;

            string ids = string.Join(",", idsEquipes);
            string query = $@"
                SELECT t.id_tarefa, t.nome_arquivo
                FROM Tarefas t
                LEFT JOIN EntregasTarefa e ON t.id_tarefa = e.id_tarefa
                WHERE t.id_equipe IN ({ids})
                    AND (e.id_entrega IS NULL OR e.id_entrega = 0)
                    AND t.nome_arquivo IS NOT NULL AND t.nome_arquivo <> ''";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tarefa = new TarefaArquivo
                                {
                                    IdTarefa = reader.GetInt32("id_tarefa"),
                                    NomeArquivo = reader.GetString("nome_arquivo")
                                };
                                tarefas.Add(tarefa);
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }

            return tarefas;
        }

        public DateTime ObterDataEntregaTarefa(int idTarefa)
        {
            DateTime dataEntrega = DateTime.Today;
            string query = "SELECT data_entrega FROM Tarefas WHERE id_tarefa = @idTarefa";
            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        var resultado = cmd.ExecuteScalar();
                        if (resultado != null && resultado != DBNull.Value)
                        {
                            dataEntrega = Convert.ToDateTime(resultado);
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return dataEntrega;
        }

        public string ObterStatusTarefa(int idTarefa)
        {
            string status = "Pendente";
            string queryEntrega = "SELECT COUNT(*) FROM EntregasTarefa WHERE id_tarefa = @idTarefa";
            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(queryEntrega, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                            status = "Concluida";
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return status;
        }

        public List<Image> ObterAvataresPorTarefa(int idTarefa)
        {
            var avatares = new List<Image>();
            string query = @"
                SELECT f.foto_perfil
                FROM Funcionarios f
                JOIN Equipes_Membros em ON f.FuncionarioId = em.FuncionarioId
                JOIN Tarefas t ON em.id_equipe = t.id_equipe
                WHERE t.id_tarefa = @idTarefa";
            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    byte[] fotoBytes = (byte[])reader["foto_perfil"];
                                    using (var ms = new MemoryStream(fotoBytes))
                                    {
                                        avatares.Add(Image.FromStream(ms));
                                    }
                                }
                                else
                                {
                                    avatares.Add(Properties.Resources.icon_perfil);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            if (avatares.Count == 0)
                avatares.Add(Properties.Resources.icon_perfil);

            return avatares;
        }

        public string CriarPastaTemporaria()
        {
            string pastaTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(pastaTemp);
            return pastaTemp;
        }

        public List<string> DividirPdfEmPaginas(string caminhoArquivoEntrada, string pastaSaida)
        {
            List<string> arquivosPaginas = new List<string>();
            PdfDocument documento = PdfReader.Open(caminhoArquivoEntrada, PdfDocumentOpenMode.Import);
            int totalPaginas = documento.PageCount;
            if (!Directory.Exists(pastaSaida))
                Directory.CreateDirectory(pastaSaida);
            for (int i = 0; i < totalPaginas; i++)
            {
                PdfDocument novoDocumento = new PdfDocument
                {
                    Version = documento.Version
                };
                novoDocumento.AddPage(documento.Pages[i]);
                string caminhoNovoArquivo = Path.Combine(pastaSaida, $"pagina_{i + 1}.pdf");
                novoDocumento.Save(caminhoNovoArquivo);
                arquivosPaginas.Add(caminhoNovoArquivo);
            }
            return arquivosPaginas;
        }
    }
}

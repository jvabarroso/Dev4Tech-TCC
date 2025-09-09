using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
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
        // Obtém os ids das equipes que o funcionário pertence
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
                finally { fecharConexao(); }
            }
            return idsEquipes;
        }

        // Obtém as tarefas pendentes (não entregues) das equipes informadas com arquivo anexado
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

        // Retorna o nome do arquivo PDF de uma tarefa
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
                finally { fecharConexao(); }
            }
            return nomeArquivo;
        }

        // Cria uma pasta temporária exclusiva e retorna o caminho
        public string CriarPastaTemporaria()
        {
            string pastaTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(pastaTemp);
            return pastaTemp;
        }

        // Divide o PDF em páginas separadas, salvando na pastaSaida, e retorna lista dos arquivos gerados
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

        // Remove arquivos PDF temporários da pasta especificada
        public void LimparArquivosTemporarios(string pastaTemporaria)
        {
            if (Directory.Exists(pastaTemporaria))
            {
                var arquivos = Directory.GetFiles(pastaTemporaria, "*.pdf");
                foreach (var arquivo in arquivos)
                {
                    try
                    {
                        File.Delete(arquivo);
                    }
                    catch
                    {
                        MessageBox.Show($"Não foi possível deletar o arquivo {arquivo}");
                    }
                }
            }
        }
    }
}

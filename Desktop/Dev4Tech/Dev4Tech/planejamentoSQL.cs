using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Dev4Tech
{
    public class planejamentoSQL : conexao
    {
        // Obter nome do arquivo PDF da tarefa
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

        // Salva pdfBytes em arquivo temporário único e retorna caminho
        public string SalvarPdfTemporario(byte[] pdfBytes)
        {
            string caminhoTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
            File.WriteAllBytes(caminhoTemp, pdfBytes);
            return caminhoTemp;
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
                PdfDocument novoDocumento = new PdfDocument();
                novoDocumento.Version = documento.Version;
                novoDocumento.AddPage(documento.Pages[i]);
                string caminhoNovoArquivo = Path.Combine(pastaSaida, $"pagina_{i + 1}.pdf");
                novoDocumento.Save(caminhoNovoArquivo);
                arquivosPaginas.Add(caminhoNovoArquivo);
            }

            return arquivosPaginas;
        }

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

        // Fluxo completo do banco até a lista das páginas para exibição
        public List<string> ObterArquivosPdfParaExibicao(int idTarefa)
        {
            // Caso armazene os arquivos no banco como blob, método para obter bytes precisaria existir
            // Agora baseando no nome do arquivo em pasta

            string nomeArquivo = ObterNomeArquivoTarefa(idTarefa);
            if (string.IsNullOrEmpty(nomeArquivo))
                return new List<string>();

            string pastaArquivos = @"C:\Dev4Tech\ArquivosTarefas";
            string caminhoArquivoPdf = Path.Combine(pastaArquivos, nomeArquivo);

            string pastaPaginas = CriarPastaTemporaria();
            return DividirPdfEmPaginas(caminhoArquivoPdf, pastaPaginas);
        }
    }
}

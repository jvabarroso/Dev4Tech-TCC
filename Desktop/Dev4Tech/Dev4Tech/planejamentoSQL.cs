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
        // Obtém o arquivo PDF em bytes do banco pela tarefa
        public byte[] ObterArquivoPdfTarefa(int idTarefa)
        {
            byte[] arquivoBytes = null;
            string query = "SELECT arquivo_blob FROM Tarefas WHERE id_tarefa = @idTarefa";
            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        var resultado = cmd.ExecuteScalar();
                        if (resultado != DBNull.Value)
                            arquivoBytes = (byte[])resultado;
                    }
                }
                finally { fecharConexao(); }
            }
            return arquivoBytes;
        }

        // Salva o arquivo PDF temporariamente e retorna o caminho do arquivo salvo
        public string SalvarPdfTemporario(byte[] arquivoPdf)
        {
            string caminhoTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
            File.WriteAllBytes(caminhoTemp, arquivoPdf);
            return caminhoTemp;
        }

        // Cria uma pasta temporária exclusiva para armazenar as páginas divididas
        public string CriarPastaTemporaria()
        {
            string pastaTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(pastaTemp);
            return pastaTemp;
        }

        // Divide o PDF em páginas separadas e salva na pasta de saída
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

        // Fluxo completo: do banco até a lista de arquivos PDF das páginas para exibição
        public List<string> ObterArquivosPdfParaExibicao(int idTarefa)
        {
            byte[] arquivoPdf = ObterArquivoPdfTarefa(idTarefa);
            if (arquivoPdf == null)
                return new List<string>();

            string caminhoPdfTemp = SalvarPdfTemporario(arquivoPdf);
            string pastaPaginas = CriarPastaTemporaria();

            return DividirPdfEmPaginas(caminhoPdfTemp, pastaPaginas);
        }
    }
}

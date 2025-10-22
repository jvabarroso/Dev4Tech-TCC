using MySql.Data.MySqlClient;
using OfficeConverter;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Dev4Tech
{
    public class TarefaArquivo
    {
        public int IdTarefa { get; set; }
        public string NomeArquivo { get; set; }
        public string NomeTarefa { get; set; }
    }

    public class ProgressoLeitura
    {
        public int IdProgresso { get; set; }
        public int IdTarefa { get; set; }
        public int IdFuncionario { get; set; }
        public int TotalPaginasVisualizadas { get; set; }
        public int TotalPaginas { get; set; }
        public decimal PercentualConcluido { get; set; }
        public bool Concluida { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }

    public class planejamentoSQL : conexao
    {
        private readonly string PastaBaseArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";

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
            string query = "SELECT nome_arquivo FROM Tarefas WHERE id_tarefa = @idTarefa";

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
                            nomeArquivo = resultado.ToString();
                            // Verificar se o arquivo existe fisicamente
                            if (!VerificarArquivoExiste(nomeArquivo))
                            {
                                MessageBox.Show($"Arquivo '{nomeArquivo}' não encontrado no servidor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return null;
                            }
                        }
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
                SELECT t.id_tarefa, t.nome_arquivo, t.nomeTarefa
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
                                    NomeArquivo = reader.GetString("nome_arquivo"),
                                    NomeTarefa = reader.GetString("nomeTarefa")
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
            string pastaTemp = Path.Combine(Path.GetTempPath(), "Dev4Tech_PDFs", Guid.NewGuid().ToString());
            Directory.CreateDirectory(pastaTemp);
            return pastaTemp;
        }

        public List<string> DividirPdfEmPaginas(string caminhoArquivoEntrada, string pastaSaida)
        {
            List<string> arquivosPaginas = new List<string>();

            if (!File.Exists(caminhoArquivoEntrada))
            {
                MessageBox.Show($"Arquivo PDF não encontrado: {caminhoArquivoEntrada}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return arquivosPaginas;
            }

            try
            {
                // Verificar se o arquivo não está corrompido
                FileInfo fileInfo = new FileInfo(caminhoArquivoEntrada);
                if (fileInfo.Length == 0)
                {
                    MessageBox.Show("O arquivo PDF está vazio ou corrompido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return arquivosPaginas;
                }

                PdfDocument documento = PdfReader.Open(caminhoArquivoEntrada, PdfDocumentOpenMode.Import);
                int totalPaginas = documento.PageCount;

                if (totalPaginas == 0)
                {
                    MessageBox.Show("O PDF não contém páginas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return arquivosPaginas;
                }

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

                MessageBox.Show($"PDF dividido com sucesso em {totalPaginas} páginas.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao dividir PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return arquivosPaginas;
        }

        public string ConverterParaPdf(string caminhoArquivoOriginal, string pastaDestino)
        {
            try
            {
                string extensao = Path.GetExtension(caminhoArquivoOriginal).ToLower();
                string nomeArquivoUnico = Guid.NewGuid().ToString() + ".pdf";
                string caminhoCompleto = Path.Combine(pastaDestino, nomeArquivoUnico);

                // Se já for PDF, apenas copia
                if (extensao == ".pdf")
                {
                    File.Copy(caminhoArquivoOriginal, caminhoCompleto, true);
                    return nomeArquivoUnico;
                }

                // Para arquivos Office, converte usando OfficeConverter
                using (var converter = new Converter())
                {
                    converter.Convert(caminhoArquivoOriginal, caminhoCompleto);
                }

                return nomeArquivoUnico;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao converter arquivo para PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public string ObterCaminhoCompletoPdf(string nomeArquivo)
        {
            if (string.IsNullOrEmpty(nomeArquivo))
                return null;

            return Path.Combine(PastaBaseArquivos, nomeArquivo);
        }

        public bool VerificarArquivoExiste(string nomeArquivo)
        {
            if (string.IsNullOrEmpty(nomeArquivo))
                return false;

            string caminhoCompleto = ObterCaminhoCompletoPdf(nomeArquivo);
            return File.Exists(caminhoCompleto);
        }

        public bool VerificarPastaArquivos()
        {
            try
            {
                if (!Directory.Exists(PastaBaseArquivos))
                {
                    Directory.CreateDirectory(PastaBaseArquivos);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar pasta de arquivos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void RegistrarVisualizacaoPagina(int idTarefa, int idFuncionario, int numeroPagina)
        {
            string query = @"
                INSERT INTO TarefaPaginasVisualizadas (id_tarefa, id_funcionario, numero_pagina) 
                VALUES (@idTarefa, @idFuncionario, @numeroPagina) 
                ON DUPLICATE KEY UPDATE data_visualizacao = NOW()";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.Parameters.AddWithValue("@numeroPagina", numeroPagina);
                        cmd.ExecuteNonQuery();
                    }

                    AtualizarProgressoAgregado(idTarefa, idFuncionario);
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        private void AtualizarProgressoAgregado(int idTarefa, int idFuncionario)
        {
            string queryProgresso = @"
                INSERT INTO TarefaProgressoLeitura (id_tarefa, id_funcionario, total_paginas_visualizadas, total_paginas, percentual_concluido, concluida)
                SELECT 
                    @idTarefa,
                    @idFuncionario,
                    COUNT(DISTINCT numero_pagina) as visualizadas,
                    (SELECT total_paginas FROM TarefaPdfMetadata WHERE id_tarefa = @idTarefa) as total,
                    (COUNT(DISTINCT numero_pagina) / (SELECT total_paginas FROM TarefaPdfMetadata WHERE id_tarefa = @idTarefa)) * 100 as percentual,
                    (COUNT(DISTINCT numero_pagina) >= (SELECT total_paginas FROM TarefaPdfMetadata WHERE id_tarefa = @idTarefa)) as concluida
                FROM TarefaPaginasVisualizadas 
                WHERE id_tarefa = @idTarefa AND id_funcionario = @idFuncionario
                ON DUPLICATE KEY UPDATE 
                    total_paginas_visualizadas = VALUES(total_paginas_visualizadas),
                    percentual_concluido = VALUES(percentual_concluido),
                    concluida = VALUES(concluida),
                    data_ultima_atualizacao = NOW()";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(queryProgresso, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        public ProgressoLeitura ObterProgressoLeitura(int idTarefa, int idFuncionario)
        {
            ProgressoLeitura progresso = null;
            string query = @"
                SELECT id_progresso, total_paginas_visualizadas, total_paginas, percentual_concluido, concluida, data_ultima_atualizacao
                FROM TarefaProgressoLeitura 
                WHERE id_tarefa = @idTarefa AND id_funcionario = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                progresso = new ProgressoLeitura
                                {
                                    IdProgresso = reader.GetInt32("id_progresso"),
                                    IdTarefa = idTarefa,
                                    IdFuncionario = idFuncionario,
                                    TotalPaginasVisualizadas = reader.GetInt32("total_paginas_visualizadas"),
                                    TotalPaginas = reader.GetInt32("total_paginas"),
                                    PercentualConcluido = reader.GetDecimal("percentual_concluido"),
                                    Concluida = reader.GetBoolean("concluida"),
                                    DataUltimaAtualizacao = reader.GetDateTime("data_ultima_atualizacao")
                                };
                            }
                        }
                    }

                    if (progresso == null)
                    {
                        int totalPaginas = ObterTotalPaginasTarefa(idTarefa);
                        progresso = new ProgressoLeitura
                        {
                            IdTarefa = idTarefa,
                            IdFuncionario = idFuncionario,
                            TotalPaginasVisualizadas = 0,
                            TotalPaginas = totalPaginas,
                            PercentualConcluido = 0,
                            Concluida = false,
                            DataUltimaAtualizacao = DateTime.Now
                        };
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return progresso;
        }

        public List<int> ObterPaginasVisualizadas(int idTarefa, int idFuncionario)
        {
            var paginas = new List<int>();
            string query = @"
                SELECT numero_pagina 
                FROM TarefaPaginasVisualizadas 
                WHERE id_tarefa = @idTarefa AND id_funcionario = @idFuncionario
                ORDER BY numero_pagina";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                paginas.Add(reader.GetInt32("numero_pagina"));
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return paginas;
        }

        public int ObterTotalPaginasTarefa(int idTarefa)
        {
            int totalPaginas = 0;
            string query = "SELECT total_paginas FROM TarefaPdfMetadata WHERE id_tarefa = @idTarefa";

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
                            totalPaginas = Convert.ToInt32(resultado);
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return totalPaginas;
        }

        public void SalvarPdfMetadata(int idTarefa, string nomeArquivo, int totalPaginas)
        {
            string hashArquivo = CalcularHashArquivo(ObterCaminhoCompletoPdf(nomeArquivo));

            string query = @"
                INSERT INTO TarefaPdfMetadata (id_tarefa, nome_arquivo, total_paginas, hash_arquivo) 
                VALUES (@idTarefa, @nomeArquivo, @totalPaginas, @hashArquivo) 
                ON DUPLICATE KEY UPDATE 
                    total_paginas = @totalPaginas, 
                    hash_arquivo = @hashArquivo,
                    data_processamento = NOW()";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@nomeArquivo", nomeArquivo);
                        cmd.Parameters.AddWithValue("@totalPaginas", totalPaginas);
                        cmd.Parameters.AddWithValue("@hashArquivo", hashArquivo);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        private string CalcularHashArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(caminhoArquivo))
            {
                byte[] hashBytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerificarPdfAlterado(int idTarefa, string nomeArquivo)
        {
            string hashAtual = CalcularHashArquivo(ObterCaminhoCompletoPdf(nomeArquivo));
            string hashArmazenado = ObterHashArmazenado(idTarefa);

            return hashAtual != hashArmazenado;
        }

        private string ObterHashArmazenado(int idTarefa)
        {
            string hash = string.Empty;
            string query = "SELECT hash_arquivo FROM TarefaPdfMetadata WHERE id_tarefa = @idTarefa";

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
                            hash = resultado.ToString();
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return hash;
        }

        public void LimparVisualizacoesTarefa(int idTarefa, int idFuncionario)
        {
            string queryVisualizacoes = @"
                DELETE FROM TarefaPaginasVisualizadas 
                WHERE id_tarefa = @idTarefa AND id_funcionario = @idFuncionario";

            string queryProgresso = @"
                DELETE FROM TarefaProgressoLeitura 
                WHERE id_tarefa = @idTarefa AND id_funcionario = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(queryVisualizacoes, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new MySqlCommand(queryProgresso, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
        }
    }
}
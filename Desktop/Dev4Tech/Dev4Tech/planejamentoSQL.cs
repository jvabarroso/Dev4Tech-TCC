using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Dev4Tech
{
    public class TarefaArquivo
    {
        public int IdTarefa { get; set; }
        public string NomeArquivo { get; set; }
        public string NomeTarefa { get; set; }
    }

    public class VisualizacaoPagina
    {
        public int IdVisualizacao { get; set; }
        public int IdTarefa { get; set; }
        public int IdFuncionario { get; set; }
        public int NumeroPagina { get; set; }
        public DateTime DataVisualizacao { get; set; }
        public int TempoVisualizacao { get; set; }
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
        // Definir caminho base consistente para todos os arquivos PDF
        private readonly string PastaBaseArquivos = @"C:\xampp\htdocs\dev4tech\arquivos";

        // ==================================================
        // MÉTODOS EXISTENTES (MANTIDOS PARA COMPATIBILIDADE)
        // ==================================================

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao dividir PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return arquivosPaginas;
        }

        public string ObterCaminhoCompletoPdf(string nomeArquivo)
        {
            return Path.Combine(PastaBaseArquivos, nomeArquivo);
        }

        public bool VerificarPastaArquivos()
        {
            return Directory.Exists(PastaBaseArquivos);
        }

        // ==================================================
        // NOVOS MÉTODOS PARA CONTROLE DE PROGRESSO DETALHADO
        // ==================================================

        /// <summary>
        /// Registra a visualização de uma página específica no banco de dados
        /// </summary>
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

                    // Atualizar progresso agregado automaticamente
                    AtualizarProgressoAgregado(idTarefa, idFuncionario);
                }
                finally
                {
                    fecharConexao();
                }
            }
        }

        /// <summary>
        /// Atualiza o progresso agregado baseado nas páginas visualizadas
        /// </summary>
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

        /// <summary>
        /// Obtém o progresso de leitura de uma tarefa para um funcionário
        /// </summary>
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

                    // Se não encontrou progresso, criar registro inicial
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

        /// <summary>
        /// Obtém a lista de páginas já visualizadas por um funcionário em uma tarefa
        /// </summary>
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

        /// <summary>
        /// Obtém o total de páginas de uma tarefa a partir dos metadados
        /// </summary>
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

        /// <summary>
        /// Salva os metadados do PDF após dividi-lo em páginas
        /// </summary>
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

        /// <summary>
        /// Calcula o hash SHA256 de um arquivo para detectar alterações
        /// </summary>
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

        /// <summary>
        /// Verifica se o PDF foi alterado comparando o hash
        /// </summary>
        public bool VerificarPdfAlterado(int idTarefa, string nomeArquivo)
        {
            string hashAtual = CalcularHashArquivo(ObterCaminhoCompletoPdf(nomeArquivo));
            string hashArmazenado = ObterHashArmazenado(idTarefa);

            return hashAtual != hashArmazenado;
        }

        /// <summary>
        /// Obtém o hash armazenado no banco para uma tarefa
        /// </summary>
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

        /// <summary>
        /// Remove todas as visualizações de uma tarefa (útil quando o PDF é atualizado)
        /// </summary>
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
                    // Remover visualizações
                    using (var cmd = new MySqlCommand(queryVisualizacoes, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        cmd.ExecuteNonQuery();
                    }

                    // Remover progresso
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

        /// <summary>
        /// Obtém estatísticas de progresso para exibição no dashboard
        /// </summary>
        public Dictionary<string, object> ObterEstatisticasProgresso(int idFuncionario)
        {
            var estatisticas = new Dictionary<string, object>();
            string query = @"
                SELECT 
                    COUNT(DISTINCT tp.id_tarefa) as total_tarefas,
                    SUM(CASE WHEN tp.concluida = true THEN 1 ELSE 0 END) as tarefas_concluidas,
                    AVG(tp.percentual_concluido) as percentual_medio,
                    SUM(tp.total_paginas_visualizadas) as total_paginas_lidas
                FROM TarefaProgressoLeitura tp
                JOIN Tarefas t ON tp.id_tarefa = t.id_tarefa
                WHERE tp.id_funcionario = @idFuncionario";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                estatisticas["total_tarefas"] = reader.GetInt32("total_tarefas");
                                estatisticas["tarefas_concluidas"] = reader.GetInt32("tarefas_concluidas");
                                estatisticas["percentual_medio"] = reader.IsDBNull(2) ? 0 : reader.GetDecimal("percentual_medio");
                                estatisticas["total_paginas_lidas"] = reader.GetInt32("total_paginas_lidas");
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return estatisticas;
        }

        /// <summary>
        /// Obtém o progresso de todos os membros da equipe para uma tarefa específica
        /// </summary>
        public List<ProgressoLeitura> ObterProgressoEquipeTarefa(int idTarefa, int idEquipe)
        {
            var progressos = new List<ProgressoLeitura>();
            string query = @"
                SELECT 
                    tp.id_funcionario,
                    f.nome,
                    tp.total_paginas_visualizadas,
                    tp.total_paginas,
                    tp.percentual_concluido,
                    tp.concluida
                FROM TarefaProgressoLeitura tp
                JOIN Funcionarios f ON tp.id_funcionario = f.FuncionarioId
                JOIN Equipes_Membros em ON f.FuncionarioId = em.FuncionarioId
                WHERE tp.id_tarefa = @idTarefa AND em.id_equipe = @idEquipe
                ORDER BY tp.percentual_concluido DESC";

            if (abrirConexao())
            {
                try
                {
                    using (var cmd = new MySqlCommand(query, conectar))
                    {
                        cmd.Parameters.AddWithValue("@idTarefa", idTarefa);
                        cmd.Parameters.AddWithValue("@idEquipe", idEquipe);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var progresso = new ProgressoLeitura
                                {
                                    IdFuncionario = reader.GetInt32("id_funcionario"),
                                    TotalPaginasVisualizadas = reader.GetInt32("total_paginas_visualizadas"),
                                    TotalPaginas = reader.GetInt32("total_paginas"),
                                    PercentualConcluido = reader.GetDecimal("percentual_concluido"),
                                    Concluida = reader.GetBoolean("concluida")
                                };
                                progressos.Add(progresso);
                            }
                        }
                    }
                }
                finally
                {
                    fecharConexao();
                }
            }
            return progressos;
        }
    }
}
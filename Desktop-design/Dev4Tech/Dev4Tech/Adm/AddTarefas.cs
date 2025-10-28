using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Dev4Tech
{
    class AddTarefas : conexao
    {
        public string NomeTarefa { get; set; }
        public string Instrucoes { get; set; }
        public string Dificuldade { get; set; }
        public int IdEquipe { get; set; }
        public DateTime DataEntrega { get; set; }
        public string NomeArquivo { get; set; } // ✅ Já receberá o nome com hash
        public byte[] ArquivoBlob { get; set; }
        public int IdEmpresa { get; set; }

        // ✅ MÉTODO DE INSERÇÃO EM LOTE (CORRIGE DUPLICAÇÃO)
        public bool InserirTarefasEmLote(List<int> equipesIds, string nomeTarefa, string instrucoes,
                                       string dificuldade, DateTime dataEntrega, string nomeArquivo, int idEmpresa)
        {
            if (equipesIds == null || equipesIds.Count == 0)
                return false;

            if (abrirConexao())
            {
                MySqlTransaction transaction = null;
                try
                {
                    transaction = conectar.BeginTransaction();

                    string query = @"INSERT INTO Tarefas 
                                    (nomeTarefa, instrucoes, dificuldade, id_equipe, data_entrega, nome_arquivo, arquivo_blob, id_empresa) 
                                    VALUES (@nome, @instr, @dificuldade, @idEq, @data, @nomeArq, @arqBlob, @idEmpresa)";

                    int tarefasInseridas = 0;

                    foreach (int idEquipe in equipesIds)
                    {
                        // ✅ Verificação rápida para evitar duplicação na mesma execução
                        if (VerificarTarefaExistente(nomeTarefa, idEquipe, dataEntrega, idEmpresa))
                        {
                            Console.WriteLine($"Tarefa duplicada ignorada: {nomeTarefa} para equipe {idEquipe}");
                            continue;
                        }

                        MySqlCommand cmd = new MySqlCommand(query, conectar, transaction);
                        cmd.Parameters.AddWithValue("@nome", nomeTarefa);
                        cmd.Parameters.AddWithValue("@instr", instrucoes);
                        cmd.Parameters.AddWithValue("@dificuldade", dificuldade);
                        cmd.Parameters.AddWithValue("@idEq", idEquipe);
                        cmd.Parameters.AddWithValue("@data", dataEntrega);
                        cmd.Parameters.AddWithValue("@nomeArq", nomeArquivo); // ✅ Nome com hash
                        cmd.Parameters.AddWithValue("@arqBlob", DBNull.Value);
                        cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                            tarefasInseridas++;
                    }

                    transaction.Commit();
                    Console.WriteLine($"{tarefasInseridas} tarefas inseridas com sucesso");
                    return tarefasInseridas > 0;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    Console.WriteLine($"Erro ao inserir tarefas em lote: {ex.Message}");
                    return false;
                }
                finally
                {
                    fecharConexao();
                }
            }
            return false;
        }

        // ✅ VERIFICAÇÃO DE TAREFA EXISTENTE
        private bool VerificarTarefaExistente(string nomeTarefa, int idEquipe, DateTime dataEntrega, int idEmpresa)
        {
            string query = @"SELECT COUNT(*) FROM Tarefas 
                            WHERE nomeTarefa = @nome 
                            AND id_equipe = @idEq 
                            AND data_entrega = @data 
                            AND id_empresa = @idEmpresa";

            MySqlCommand cmd = new MySqlCommand(query, conectar);
            cmd.Parameters.AddWithValue("@nome", nomeTarefa);
            cmd.Parameters.AddWithValue("@idEq", idEquipe);
            cmd.Parameters.AddWithValue("@data", dataEntrega);
            cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        public DataTable BuscarEquipes()
        {
            DataTable dt = new DataTable();
            if (abrirConexao())
            {
                try
                {
                    string query = "SELECT id_equipe, nome_equipe FROM Equipes ORDER BY nome_equipe";
                    MySqlCommand cmd = new MySqlCommand(query, conectar);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                finally
                {
                    fecharConexao();
                }
            }
            return dt;
        }
    }
}
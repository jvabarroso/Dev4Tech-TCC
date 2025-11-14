<?php
include_once('conexao.php');

// Função para debug - remover em produção
function debug_log($message) {
    file_put_contents('debug_log.txt', date('Y-m-d H:i:s') . " - " . $message . "\n", FILE_APPEND);
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Log do recebimento
    debug_log("Recebida requisição para processar PDF");
    
    $input = json_decode(file_get_contents('php://input'), true);
    $id_tarefa = $input['id_tarefa'] ?? '';
    $nome_arquivo = $input['nome_arquivo'] ?? '';
    $total_paginas = $input['total_paginas'] ?? 0;
    $hash_arquivo = $input['hash_arquivo'] ?? '';
    
    debug_log("Dados recebidos - id_tarefa: $id_tarefa, nome_arquivo: $nome_arquivo, total_paginas: $total_paginas");
    
    if (empty($id_tarefa) || empty($nome_arquivo)) {
        $response = ['success' => false, 'message' => 'Dados insuficientes'];
        debug_log("Erro: Dados insuficientes");
        echo json_encode($response);
        exit;
    }

    try {
        // Usar a conexão PDO do seu arquivo conexao.php
        global $pdo;
        
        if (!$pdo) {
            $response = ['success' => false, 'message' => 'Erro ao conectar ao banco'];
            debug_log("Erro: Conexão com banco falhou");
            echo json_encode($response);
            exit;
        }
        
        debug_log("Conexão com banco estabelecida");

        // 1. Verificar se a tarefa existe
        $stmt_check = $pdo->prepare("SELECT id_tarefa FROM Tarefas WHERE id_tarefa = ?");
        $stmt_check->execute([$id_tarefa]);
        
        if ($stmt_check->rowCount() === 0) {
            $response = ['success' => false, 'message' => 'Tarefa não encontrada'];
            debug_log("Erro: Tarefa $id_tarefa não encontrada");
            echo json_encode($response);
            exit;
        }
        
        debug_log("Tarefa $id_tarefa encontrada");

        // 2. Inserir ou atualizar na tabela TarefaPdfMetadata
        $stmt_metadata = $pdo->prepare("
            INSERT INTO TarefaPdfMetadata (id_tarefa, nome_arquivo, total_paginas, hash_arquivo) 
            VALUES (?, ?, ?, ?)
            ON DUPLICATE KEY UPDATE 
                nome_arquivo = VALUES(nome_arquivo),
                total_paginas = VALUES(total_paginas),
                hash_arquivo = VALUES(hash_arquivo),
                data_processamento = CURRENT_TIMESTAMP
        ");
        
        if ($stmt_metadata->execute([$id_tarefa, $nome_arquivo, $total_paginas, $hash_arquivo])) {
            debug_log("Tabela TarefaPdfMetadata atualizada com sucesso");
            
            $response = [
                'success' => true, 
                'message' => 'PDF processado com sucesso',
                'total_paginas' => $total_paginas,
                'id_tarefa' => $id_tarefa
            ];
            
            echo json_encode($response);
        } else {
            $error = $stmt_metadata->errorInfo();
            $response = ['success' => false, 'message' => 'Erro ao salvar metadados: ' . $error[2]];
            debug_log("Erro ao salvar metadados: " . $error[2]);
            echo json_encode($response);
        }
        
    } catch (Exception $e) {
        $response = ['success' => false, 'message' => 'Erro: ' . $e->getMessage()];
        debug_log("Exception: " . $e->getMessage());
        echo json_encode($response);
    }
    exit;
} else {
    $response = ['success' => false, 'message' => 'Método não permitido'];
    debug_log("Erro: Método não permitido");
    echo json_encode($response);
    exit;
}
?>
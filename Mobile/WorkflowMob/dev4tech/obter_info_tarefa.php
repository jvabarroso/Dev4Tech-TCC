<?php
include_once('conexao.php');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);
    $idTarefa = $input['id_tarefa'] ?? '';
    
    if (empty($idTarefa)) {
        echo json_encode(['success' => false, 'message' => 'ID da tarefa não fornecido']);
        exit;
    }

    try {
        global $pdo;
        
        // Buscar informações da tarefa e metadados do PDF
        $stmt = $pdo->prepare("
            SELECT 
                t.*, 
                tm.total_paginas, 
                tm.nome_arquivo as nome_arquivo_metadata,
                tm.data_processamento
            FROM Tarefas t 
            LEFT JOIN TarefaPdfMetadata tm ON t.id_tarefa = tm.id_tarefa 
            WHERE t.id_tarefa = ?
        ");
        $stmt->execute([$idTarefa]);
        $tarefa = $stmt->fetch();
        
        if ($tarefa) {
            echo json_encode([
                'success' => true,
                'id_tarefa' => $tarefa['id_tarefa'],
                'nome_arquivo' => $tarefa['nome_arquivo'],
                'total_paginas' => $tarefa['total_paginas'] ?? 0,
                'processada' => !empty($tarefa['total_paginas']) && $tarefa['total_paginas'] > 0,
                'data_processamento' => $tarefa['data_processamento'] ?? null,
                'dados_completos' => $tarefa
            ]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Tarefa não encontrada']);
        }
        
    } catch (Exception $e) {
        echo json_encode(['success' => false, 'message' => 'Erro: ' . $e->getMessage()]);
    }
    exit;
}
?>
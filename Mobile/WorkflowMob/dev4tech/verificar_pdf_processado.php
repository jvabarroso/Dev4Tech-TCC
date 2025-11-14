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
        $conn = conectar();
        
        // Verificar se já existe metadata para esta tarefa
        $stmt = $conn->prepare("
            SELECT tm.*, t.total_paginas 
            FROM TarefaPdfMetadata tm 
            LEFT JOIN Tarefas t ON tm.id_tarefa = t.id_tarefa 
            WHERE tm.id_tarefa = ?
        ");
        $stmt->bind_param("i", $idTarefa);
        $stmt->execute();
        $result = $stmt->get_result();
        $metadata = $result->fetch_assoc();
        
        if ($metadata) {
            // Verificar se as páginas físicas existem
            $pastaPaginas = 'C:/xampp/htdocs/dev4tech/arquivos/' . $idTarefa . '/';
            $paginaExemplo = $pastaPaginas . 'pagina_1.pdf';
            
            if (file_exists($paginaExemplo)) {
                echo json_encode([
                    'success' => true,
                    'processado' => true,
                    'total_paginas' => $metadata['total_paginas'],
                    'metadata' => $metadata
                ]);
            } else {
                echo json_encode([
                    'success' => true,
                    'processado' => false,
                    'message' => 'Metadata existe mas páginas físicas não encontradas'
                ]);
            }
        } else {
            echo json_encode([
                'success' => true,
                'processado' => false
            ]);
        }
        
    } catch (Exception $e) {
        echo json_encode(['success' => false, 'message' => 'Erro: ' . $e->getMessage()]);
    }
    exit;
}
?>
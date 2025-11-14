<?php
include_once('conexao.php');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);
    $idTarefa = $input['id_tarefa'] ?? '';
    $idFuncionario = $input['id_funcionario'] ?? '';
    
    if (empty($idTarefa) || empty($idFuncionario)) {
        echo json_encode(['success' => false, 'message' => 'Dados insuficientes']);
        exit;
    }

    try {
        global $pdo;
        
        // Obter progresso atual da tabela TarefaProgressoLeitura
        $stmt = $pdo->prepare("
            SELECT * FROM TarefaProgressoLeitura 
            WHERE id_tarefa = ? AND id_funcionario = ?
        ");
        $stmt->execute([$idTarefa, $idFuncionario]);
        $progresso = $stmt->fetch();
        
        // Obter páginas visualizadas da tabela TarefaPaginasVisualizadas
        $stmt = $pdo->prepare("
            SELECT numero_pagina 
            FROM TarefaPaginasVisualizadas 
            WHERE id_tarefa = ? AND id_funcionario = ?
            ORDER BY numero_pagina
        ");
        $stmt->execute([$idTarefa, $idFuncionario]);
        
        $paginasVisualizadas = [];
        while ($row = $stmt->fetch()) {
            $paginasVisualizadas[] = $row['numero_pagina'];
        }
        
        // Obter total de páginas da tabela TarefaPdfMetadata
        $stmt = $pdo->prepare("
            SELECT total_paginas FROM TarefaPdfMetadata WHERE id_tarefa = ?
        ");
        $stmt->execute([$idTarefa]);
        $metadata = $stmt->fetch();
        
        $totalPaginas = $metadata['total_paginas'] ?? 0;
        
        // Se não há registro de progresso, criar um com dados básicos
        if (!$progresso) {
            $progresso = [
                'total_paginas_visualizadas' => 0,
                'total_paginas' => $totalPaginas,
                'percentual_concluido' => 0,
                'concluida' => false
            ];
        } else {
            // Garantir que o total_paginas está atualizado
            $progresso['total_paginas'] = $totalPaginas;
        }
        
        $progresso['paginas_visualizadas'] = $paginasVisualizadas;
        
        echo json_encode([
            'success' => true,
            'progresso' => $progresso
        ]);
        
    } catch (Exception $e) {
        echo json_encode(['success' => false, 'message' => 'Erro: ' . $e->getMessage()]);
    }
    exit;
}
?>
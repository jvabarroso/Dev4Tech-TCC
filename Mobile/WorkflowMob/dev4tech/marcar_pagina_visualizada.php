<?php
include_once('conexao.php');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);
    $idTarefa = $input['id_tarefa'] ?? '';
    $idFuncionario = $input['id_funcionario'] ?? '';
    $numeroPagina = $input['numero_pagina'] ?? '';
    
    if (empty($idTarefa) || empty($idFuncionario) || empty($numeroPagina)) {
        echo json_encode(['success' => false, 'message' => 'Dados insuficientes']);
        exit;
    }

    try {
        global $pdo;
        
        // 1. Registrar visualização da página na tabela TarefaPaginasVisualizadas
        $stmt = $pdo->prepare("
            INSERT INTO TarefaPaginasVisualizadas (id_tarefa, id_funcionario, numero_pagina) 
            VALUES (?, ?, ?)
            ON DUPLICATE KEY UPDATE 
                data_visualizacao = CURRENT_TIMESTAMP,
                tempo_visualizacao = tempo_visualizacao + 1
        ");
        $stmt->execute([$idTarefa, $idFuncionario, $numeroPagina]);
        
        // 2. Obter total de páginas da tabela TarefaPdfMetadata
        $stmt = $pdo->prepare("
            SELECT total_paginas FROM TarefaPdfMetadata WHERE id_tarefa = ?
        ");
        $stmt->execute([$idTarefa]);
        $metadata = $stmt->fetch();
        
        $totalPaginas = $metadata['total_paginas'] ?? 0;
        
        // 3. Contar páginas visualizadas deste funcionário para esta tarefa
        $stmt = $pdo->prepare("
            SELECT COUNT(*) as visualizadas 
            FROM TarefaPaginasVisualizadas 
            WHERE id_tarefa = ? AND id_funcionario = ?
        ");
        $stmt->execute([$idTarefa, $idFuncionario]);
        $contagem = $stmt->fetch();
        
        $paginasVisualizadas = $contagem['visualizadas'];
        $percentual = $totalPaginas > 0 ? round(($paginasVisualizadas / $totalPaginas) * 100, 2) : 0;
        $concluida = ($paginasVisualizadas >= $totalPaginas) && ($totalPaginas > 0);
        
        // 4. Atualizar progresso na tabela TarefaProgressoLeitura
        $stmt = $pdo->prepare("
            INSERT INTO TarefaProgressoLeitura (id_tarefa, id_funcionario, total_paginas_visualizadas, total_paginas, percentual_concluido, concluida) 
            VALUES (?, ?, ?, ?, ?, ?)
            ON DUPLICATE KEY UPDATE 
                total_paginas_visualizadas = VALUES(total_paginas_visualizadas),
                total_paginas = VALUES(total_paginas),
                percentual_concluido = VALUES(percentual_concluido),
                concluida = VALUES(concluida),
                data_ultima_atualizacao = CURRENT_TIMESTAMP
        ");
        $stmt->execute([$idTarefa, $idFuncionario, $paginasVisualizadas, $totalPaginas, $percentual, $concluida]);
        
        // 5. Obter lista de páginas visualizadas para retorno
        $stmt = $pdo->prepare("
            SELECT numero_pagina 
            FROM TarefaPaginasVisualizadas 
            WHERE id_tarefa = ? AND id_funcionario = ?
            ORDER BY numero_pagina
        ");
        $stmt->execute([$idTarefa, $idFuncionario]);
        
        $paginasVisualizadasArray = [];
        while ($row = $stmt->fetch()) {
            $paginasVisualizadasArray[] = $row['numero_pagina'];
        }
        
        echo json_encode([
            'success' => true,
            'progresso' => [
                'total_paginas_visualizadas' => $paginasVisualizadas,
                'total_paginas' => $totalPaginas,
                'percentual_concluido' => $percentual,
                'concluida' => $concluida,
                'paginas_visualizadas' => $paginasVisualizadasArray
            ]
        ]);
        
    } catch (Exception $e) {
        echo json_encode(['success' => false, 'message' => 'Erro: ' . $e->getMessage()]);
    }
    exit;
}
?>
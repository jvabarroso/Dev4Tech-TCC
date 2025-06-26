<?php
include_once('conexao.php');

// Obter o ID do funcionário
$id_funcionario = $_GET['id_funcionario'] ?? null;

if (empty($id_funcionario)) {
    error_log("Erro: ID do funcionário não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID do funcionário não fornecido',
        'received_data' => $_GET
    ]);
    exit();
}

try {
    error_log("Buscando tarefas para o funcionário ID: " . $id_funcionario);
    
    // Consulta corrigida usando JOIN com Equipes_Membros
$query = $pdo->prepare("
    SELECT 
        t.id_tarefa,
        t.nomeTarefa,
        t.instrucoes,
        t.data_entrega,
        t.dificuldade,
        e.nome_equipe,
        CASE 
            WHEN et.id_entrega IS NOT NULL THEN 1
            ELSE 0
        END AS entregue
    FROM Tarefas t
    JOIN Equipes e ON t.id_equipe = e.id_equipe
    JOIN Equipes_Membros em ON t.id_equipe = em.id_equipe
    LEFT JOIN EntregasTarefa et 
        ON et.id_tarefa = t.id_tarefa 
        AND et.id_equipe = t.id_equipe 
        AND et.id_equipe = em.id_equipe 
        AND et.id_entrega IN (
            SELECT et2.id_entrega
            FROM EntregasTarefa et2
            JOIN Equipes_Membros em2 ON et2.id_equipe = em2.id_equipe
            WHERE em2.FuncionarioId = :id_funcionario
        )
    WHERE em.FuncionarioId = :id_funcionario
    GROUP BY t.id_tarefa
    ORDER BY t.data_entrega ASC
");

    $query->bindValue(':id_funcionario', $id_funcionario, PDO::PARAM_INT);
    $query->execute();
    $tarefas = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Tarefas encontradas: " . count($tarefas));

foreach ($tarefas as &$tarefa) {
    $tarefa['data_entrega'] = date('d/m/Y', strtotime($tarefa['data_entrega']));
    
    // Converte o "entregue" para booleano se vier como string
    $tarefa['entregue'] = (bool) $tarefa['entregue'];

    switch ($tarefa['dificuldade']) {
        case 1: $tarefa['dificuldade_texto'] = 'Fácil'; $tarefa['dificuldade_icone'] = '⭐'; break;
        case 2: $tarefa['dificuldade_texto'] = 'Média'; $tarefa['dificuldade_icone'] = '⭐⭐'; break;
        case 3: $tarefa['dificuldade_texto'] = 'Difícil'; $tarefa['dificuldade_icone'] = '⭐⭐⭐'; break;
        default: $tarefa['dificuldade_texto'] = 'Fácil'; $tarefa['dificuldade_icone'] = '⭐';
    }
}

    echo json_encode([
        'success' => true,
        'result' => $tarefas
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
}
?>
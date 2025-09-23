<?php
include_once('conexao.php');

$id_equipe = $_GET['id_equipe'] ?? null;

if (empty($id_equipe)) {
    error_log("Erro: ID da Equipe não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID da Equipe não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    $query = $pdo->prepare("SELECT 
        f.FuncionarioId, 
        f.nome, 
        COUNT(et.id_tarefa) as total
    FROM EntregasTarefa et
    INNER JOIN Funcionarios f ON f.FuncionarioId = et.FuncionarioId
    INNER JOIN Tarefas t ON t.id_tarefa = et.id_tarefa
    WHERE t.id_equipe = :id_equipe
    GROUP BY f.FuncionarioId, f.Nome
    ");
    $query->bindValue(':id_equipe', $id_equipe, PDO::PARAM_INT);
    $query->execute();
    
    $tarefas = $query->fetchAll(PDO::FETCH_ASSOC);

    echo json_encode([
        'success' => true,
        'result' => $tarefas
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados'
    ]);
}
?>
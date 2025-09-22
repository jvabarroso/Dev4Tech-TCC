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
    $query = $pdo->prepare("SELECT dificuldade FROM tarefas WHERE id_equipe = :id_equipe");
    $query->bindValue(':id_equipe', $id_equipe, PDO::PARAM_INT);
    $query->execute();
    
    $dificuldade = $query->fetchAll(PDO::FETCH_ASSOC);

    echo json_encode([
        'success' => true,
        'result' => $dificuldade
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados'
    ]);
}
?>
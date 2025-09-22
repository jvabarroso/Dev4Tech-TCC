<?php
include_once('conexao.php');

$id_funcionario = $_GET['id_funcionario'] ?? null;

if (empty($id_funcionario)) {
    error_log("Erro: ID do funcionario não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID da funcionario não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    $query = $pdo->prepare("SELECT pontos FROM pontuacaofuncionario  WHERE id_funcionario = :id_funcionario");
    $query->bindValue(':id_funcionario', $id_funcionario, PDO::PARAM_INT);
    $query->execute();
    
    $pontos = $query->fetch(PDO::FETCH_ASSOC);

    echo json_encode([
        'success' => true,
        'result' => $pontos
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados'
    ]);
}
?>
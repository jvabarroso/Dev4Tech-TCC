<?php
include_once('conexao.php');

$id_tarefa = $_GET['id_tarefa'] ?? null;

error_log("ID do tarefa recebido: " . var_export($id_tarefa, true));

if (empty($id_tarefa)) {
    error_log("Erro: ID do tarefa não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID do tarefa não fornecido',
        'received_data' => $_GET
    ]);
    exit();
}

try {
    error_log("Buscando tarefa: " . $id_tarefa);
    
    $query = $pdo->prepare("DELETE FROM entregastarefa WHERE id_tarefa = :id_tarefa");
    
    $query->bindValue(':id_tarefa', $id_tarefa, PDO::PARAM_INT);
    $query->execute();
    error_log("Deletado com sucesso");

    echo json_encode([
        'success' => true,
    ]);
    

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
}
?>
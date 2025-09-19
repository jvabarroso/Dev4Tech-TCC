<?php
require_once("conexao.php");

$id_tarefa = @$_GET['id_tarefa'];

try {
    $query = $pdo->prepare("SELECT nome_arquivo, descricao FROM entregastarefa WHERE id_tarefa = :id_tarefa");
    $query->bindValue(':id_tarefa', $id_tarefa);
    $query->execute();
    $tarefa = $query->fetch(PDO::FETCH_ASSOC);

    echo json_encode([
        'sucesso' => true,
        'tarefa' => $tarefa
    ]);
} catch (PDOException $e) {
    echo json_encode([
        'sucesso' => false,
        'mensagem' => 'Erro: ' . $e->getMessage()
    ]);
}
?>
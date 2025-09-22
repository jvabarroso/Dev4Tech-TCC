<?php
require_once("conexao.php");

$id_tarefa = @$_GET['id_tarefa'];

try {
    $query = $pdo->prepare("SELECT descricao FROM RelatoProblema WHERE id_tarefa = :id_tarefa");
    $query->bindValue(':id_tarefa', $id_tarefa);
    $query->execute();
    $problemas = $query->fetchAll(PDO::FETCH_COLUMN);

    echo json_encode([
        'sucesso' => true,
        'problemas' => $problemas
    ]);
} catch (PDOException $e) {
    echo json_encode([
        'sucesso' => false,
        'mensagem' => 'Erro: ' . $e->getMessage()
    ]);
}
?>
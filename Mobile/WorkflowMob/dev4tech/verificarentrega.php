<?php
include_once('conexao.php');

$id_tarefa = $_GET['id_tarefa'] ?? null;
$id_funcionario = $_GET['FuncionarioId'] ?? null;

if (!$id_tarefa || !$id_funcionario) {
    echo json_encode(['sucesso' => false, 'mensagem' => 'Dados incompletos']);
    exit;
}

$query = $pdo->prepare("SELECT descricao, nome_arquivo FROM EntregasTarefa 
                        WHERE id_tarefa = :id_tarefa AND FuncionarioId = :id_funcionario LIMIT 1");
$query->bindValue(':id_tarefa', $id_tarefa);
$query->bindValue(':id_funcionario', $id_funcionario);
$query->execute();

$entrega = $query->fetch(PDO::FETCH_ASSOC);

$avaliada = false;
if ($entrega) {
    $queryAvaliacao = $pdo->prepare("SELECT id_avaliacao FROM AvaliacaoTarefa 
                                    WHERE id_tarefa = :id_tarefa LIMIT 1");
    $queryAvaliacao->bindValue(':id_tarefa', $id_tarefa);
    $queryAvaliacao->execute();
    $avaliada = $queryAvaliacao->rowCount() > 0;
}

echo json_encode([
    'sucesso' => true,
    'avaliada' => $avaliada,
    'entregue' => $entrega ? true : false,
    'descricao' => $entrega['descricao'] ?? null,
    'nome_arquivo' => $entrega['nome_arquivo'] ?? null
]);
?>
<?php 
include_once('conexao.php');

$tabela = 'EntregasTarefa';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);


if ($postjson === null) {
    echo json_encode(['success' => false, 'message' => 'Dados inválidos']);
    exit();
}

$id_entrega = $postjson['id_entrega'] ?? null;
$statusAvaliacao = $postjson['statusAvaliacao'] ?? null;
$dificuldade = $postjson['dificuldade'] ?? null;
$id_funcionario = $postjson['id_funcionario'] ?? null;
$pontos = $postjson['pontos'] ?? 0;
$atraso_justificado = $postjson['atraso_justificado'] ?? false;

try {

    $pdo->beginTransaction(); 

    $sqlTarefa = "SELECT id_tarefa FROM EntregasTarefa WHERE id_entrega = :id_entrega";
    $stmtTarefa = $pdo->prepare($sqlTarefa);
    $stmtTarefa->bindValue(':id_entrega', $id_entrega);
    $stmtTarefa->execute();
    $entrega = $stmtTarefa->fetch(PDO::FETCH_ASSOC);
    
    if (!$entrega) {
        throw new Exception('Entrega não encontrada');
    }

    $id_tarefa = $entrega['id_tarefa'];

    $sqlUpdate = "UPDATE $tabela SET entregue = 1 WHERE id_entrega = :id_entrega";
    $stmt = $pdo->prepare($sqlUpdate);
    $stmt->bindValue(':id_entrega', $id_entrega);  
    $stmt->execute();

    $sqlAvaliacao = "INSERT INTO AvaliacaoTarefa 
                    (id_tarefa, aceita, atraso_justificado) 
                    VALUES (:id_tarefa, :aceita, :atraso_justificado)";
    $stmtAvaliacao = $pdo->prepare($sqlAvaliacao);
    $stmtAvaliacao->bindValue(':id_tarefa', $id_tarefa);
    $stmtAvaliacao->bindValue(':aceita', $statusAvaliacao === 'aceito' ? 1 : 0);
    $stmtAvaliacao->bindValue(':atraso_justificado', $atraso_justificado ? 1 : 0);
    $stmtAvaliacao->execute();

    if ($pontos != 0) {
        $res = $pdo->prepare("INSERT INTO pontuacaofuncionario SET 
            id_funcionario = :id_funcionario,
            pontos = :pontos");	

        $res->bindValue(":id_funcionario", $id_funcionario);
        $res->bindValue(":pontos", $pontos);
        $res->execute();
    }

    $pdo->commit();
    
    echo json_encode([
        'success' => true,
        'message' => 'Tarefa avaliada com sucesso',
        'pontos_atribuidos' => $pontos
    ]);
    exit;

}catch (Exception $e) {
    if ($pdo->inTransaction()) {
        $pdo->rollBack();
    }
    echo json_encode(['success' => false, 'message' => 'Erro interno', 'detail' => $e->getMessage()]);
    exit;
}
?>
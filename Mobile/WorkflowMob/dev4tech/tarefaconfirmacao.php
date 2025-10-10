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


try {

    $pdo->beginTransaction(); 

    $sqlUpdate = "UPDATE $tabela SET entregue = 1 WHERE id_entrega = :id_entrega";
    $stmt = $pdo->prepare($sqlUpdate);
    $stmt->bindValue(':id_entrega', $id_entrega);  
    $stmt->execute();

    if($statusAvaliacao === "aceito"){    
        if($dificuldade === "Fácil"){
            $res = $pdo->prepare("INSERT INTO pontuacaofuncionario SET 
                id_funcionario = :id_funcionario,
                pontos = 2");	

            $res->bindValue(":id_funcionario", "$id_funcionario");
            $res->execute();
        }
        elseif($dificuldade === "Médio"){
            $res = $pdo->prepare("INSERT INTO pontuacaofuncionario SET 
                id_funcionario = :id_funcionario,
                pontos = 4");	

            $res->bindValue(":id_funcionario", "$id_funcionario");
            $res->execute();
        }
        elseif($dificuldade === "Difícil"){
            $res = $pdo->prepare("INSERT INTO pontuacaofuncionario SET 
                id_funcionario = :id_funcionario,
                pontos = 6");	

            $res->bindValue(":id_funcionario", "$id_funcionario");
            $res->execute();
        }
    }
    elseif($statusAvaliacao === "negado"){
        $res = $pdo->prepare("INSERT INTO pontuacaofuncionario SET 
            id_funcionario = :id_funcionario,
            pontos = -3");	

        $res->bindValue(":id_funcionario", "$id_funcionario");
        $res->execute();
    }

    $pdo->commit();
    
    echo json_encode([
        'success' => true,
        'message' => 'Tarefa avaliada com sucesso',
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
<?php
require_once("conexao.php");

$input = json_decode(file_get_contents("php://input"), true);
$FuncionarioId = $input['FuncionarioId'] ?? null;

try {
    $stmt = $pdo->prepare("UPDATE mensagenschat 
        SET status = 'lido' 
        WHERE FuncionarioId != ? 
        AND status = 'entregue'")
        ->execute([$FuncionarioId]);
     $stmt->execute([$FuncionarioId]);
     
    echo json_encode(["sucesso" => true]);
} catch (Exception $e) {
    echo json_encode(["erro" => "Erro ao atualizar mensagens: " . $e->getMessage()]);
    exit;
}

?>
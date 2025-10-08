<?php
require_once("conexao.php");

$input = json_decode(file_get_contents("php://input"), true);

$Texto = $input['Texto'];
$data_envio = $input['data_envio'];
$id_equipe = $input['id_equipe'];
$FuncionarioId = $input['FuncionarioId'];
$AdminId = $input['AdminId'];
$id_empresa = $input['id_empresa'];

try{
    $stmt = $pdo->prepare("INSERT INTO mensagenschat 
        (Texto, data_envio, id_equipe, FuncionarioId, AdminId, id_empresa, status) 
        VALUES (:Texto, NOW(), :id_equipe, :FuncionarioId, :AdminId, :id_empresa, 'enviada')");

    $stmt->execute([
        ':Texto' => $Texto,
        ':id_equipe' => $id_equipe,
        ':FuncionarioId' => $FuncionarioId,
        ':AdminId' => $AdminId,
        ':id_empresa' => $id_empresa
    ]);

    echo json_encode(["sucesso" => true, "mensagem" => "Mensagem enviada com sucesso!"]);
} catch (Exception $e) {
    echo json_encode(["erro" => "Erro ao salvar mensagem: " . $e->getMessage()]);
}

?>
<?php
require_once("conexao.php");

$id_equipe = $_GET['id_equipe'] ?? null;

if (!$id_equipe) {
  echo json_encode(["erro" => "id_equipe não informado"]);
  exit;
}

$pdo->prepare("UPDATE mensagenschat SET status = 'entregue' WHERE id_equipe = ? AND status = 'enviada'")
    ->execute([$id_equipe]);

$stmt = $pdo->prepare("SELECT 
    id_mensagem,
    Texto, 
    data_envio, 
    id_equipe, 
    FuncionarioId, 
    AdminId, 
    id_empresa,  
    status 
    FROM mensagenschat 
    WHERE id_equipe = ?
    ORDER BY id_mensagem DESC");

$stmt->execute([$id_equipe]);
$mensagens = $stmt->fetchAll();

echo json_encode($mensagens);

?>
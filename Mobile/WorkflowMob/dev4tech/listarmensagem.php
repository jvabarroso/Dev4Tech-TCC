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
    m.id_mensagem,
    m.Texto, 
    m.data_envio, 
    m.id_equipe, 
    m.FuncionarioId, 
    m.AdminId, 
    m.id_empresa,  
    f.Nome AS FuncionarioNome,
    a.Nome AS AdminNome,
    status 
    FROM mensagenschat m
    LEFT JOIN funcionarios f ON m.FuncionarioId = f.FuncionarioId
    LEFT JOIN administradores a ON m.AdminId = a.AdminId
    WHERE m.id_equipe = ?
    ORDER BY m.id_mensagem DESC");

$stmt->execute([$id_equipe]);
$mensagens = $stmt->fetchAll();

echo json_encode($mensagens);

?>
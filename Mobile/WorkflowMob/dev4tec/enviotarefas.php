<?php 
require_once("conexao.php");

// Recebe os dados JSON
$tabela = 'EntregasTarefa';

$postjson = json_decode(file_get_contents('php://input'), true);

$id_tarefa = @$postjson['id_tarefa'];
$id_equipe = @$postjson['id_equipe'];
// $descricao = @$postjson['descricao'];
// $nome_arquivo = @$postjson['nome_arquivo'];
$FuncionarioId = @$postjson['FuncionarioId'];

try{
    $res = $pdo->prepare("INSERT INTO $tabela SET 
    id_tarefa = :id_tarefa,
    id_equipe = :id_equipe,
    -- descricao = :descricao, 
    -- nome_arquivo = :nome_arquivo, 
    FuncionarioId = :FuncionarioId
    ");	

    $res->bindValue(":id_tarefa","$id_tarefa");
    $res->bindValue(":id_equipe","$id_equipe");
    // $res->bindValue(":descricao","$descricao");
    // $res->bindValue(":nome_arquivo","$nome_arquivo");
    $res->bindValue(":FuncionarioId","$FuncionarioId");

    if($res->execute()){
        $result = json_encode(array('mensagem'=>'Entregue com sucesso!', 'sucesso'=>true));
    } 
    else{
        $result = json_encode(array('mensagem'=>'Erro ao Salvar', 'sucesso'=>false));
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
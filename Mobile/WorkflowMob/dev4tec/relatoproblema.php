<?php 
require_once("conexao.php");

// Recebe os dados JSON
$tabela = 'relatoproblema';

$postjson = json_decode(file_get_contents('php://input'), true);

$id_tarefa = @$postjson['id_tarefa'];
$id_equipe = @$postjson['id_equipe'];
$descricao = @$postjson['descricao'];
$id_empresa = @$postjson['id_empresa'];

try{
    $res = $pdo->prepare("INSERT INTO $tabela SET 
    id_tarefa = :id_tarefa, 
    id_equipe = :id_equipe, 
    descricao = :descricao, 
    id_empresa = :id_empresa");	

    $res->bindValue(":id_tarefa", "$id_tarefa");
    $res->bindValue(":id_equipe", "$id_equipe");
    $res->bindValue(":descricao", "$descricao");
    $res->bindValue(":id_empresa", "$id_empresa");

    if($res->execute()){
        $result = json_encode(array('mensagem'=>'Salvo problema com sucesso!', 'sucesso'=>true));
    } 
    else{
        $result = json_encode(array('mensagem'=>'Erro ao Salvar o problema', 'sucesso'=>false));
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
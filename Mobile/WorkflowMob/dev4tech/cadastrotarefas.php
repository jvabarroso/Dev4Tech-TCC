<?php 
require_once("conexao.php");

$tabela = 'Tarefas';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$nomeTarefa = @$postjson['nomeTarefa'];
$instrucoes = @$postjson['instrucoes'];
$id_equipe = @$postjson['id_equipe'];
$data_entrega = @$postjson['data_entrega'];
$nome_arquivo = @$postjson['nome_arquivo'];
$dificuldade = @$postjson['dificuldade'];
$id_empresa = @$postjson['id_empresa'];

try {

    $res = $pdo->prepare("INSERT INTO $tabela SET 
        nomeTarefa = :nomeTarefa, 
        instrucoes = :instrucoes, 
        id_equipe = :id_equipe,
        data_entrega = :data_entrega,
        nome_arquivo = :nome_arquivo,
        dificuldade = :dificuldade,
        id_empresa = :id_empresa");	

    $res->bindValue(":nomeTarefa", "$nomeTarefa");
    $res->bindValue(":instrucoes", "$instrucoes");
    $res->bindValue(":id_equipe", "$id_equipe");
    $res->bindValue(":data_entrega", "$data_entrega");
    $res->bindValue(":nome_arquivo", "$nome_arquivo");
    $res->bindValue(":dificuldade", "$dificuldade");
    $res->bindValue(":id_empresa", "$id_empresa");
    
    if ($res->execute()) {
        $result = json_encode(['mensagem'=>'Salvo com sucesso!', 'sucesso'=>true]);
    } else {
        $result = json_encode(['mensagem'=>'Erro ao Salvar', 'sucesso'=>false]);
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
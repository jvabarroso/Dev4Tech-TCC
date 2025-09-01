<?php 
require_once("conexao.php");
include_once("upload_equipe.php");

$tabela = 'Equipes';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$nome_equipe = @$postjson['nome_equipe'];
$id_categoria = @$postjson['id_categoria'];
$AdminId = @$postjson['AdminId'];
$foto_equipe = @$postjson['foto_equipe'];
$id_empresa = @$postjson['id_empresa'];

try {
    $query = $pdo->prepare("SELECT * FROM categorias  WHERE id_categoria = :id_categoria");
    $query->bindValue(':id_categoria', $id_categoria, PDO::PARAM_INT);
    $query->execute();

    if (!$query->fetch(PDO::FETCH_ASSOC)) {
        echo json_encode(['success' => false, 'message' => 'Categoria inválida']);
        exit();
    }

    $res = $pdo->prepare("INSERT INTO $tabela SET 
        nome_equipe = :nome_equipe, 
        id_categoria = :id_categoria, 
        AdminId = :AdminId,
        id_empresa = :id_empresa,
        foto_equipe = :foto_equipe");	

    $res->bindValue(":nome_equipe", "$nome_equipe");
    $res->bindValue(":id_categoria", "$id_categoria");
    $res->bindValue(":AdminId", "$AdminId");
    $res->bindValue(":id_empresa", "$id_empresa");
    $res->bindValue(":foto_equipe", "$foto_equipe");

    if ($res->execute()) {
        $idEquipe = $pdo->lastInsertId();
        $result = json_encode(['mensagem'=>'Salvo com sucesso!', 'sucesso'=>true, 'id_equipe'=>$idEquipe, 'foto'=>$foto_equipe]);
    } else {
        $result = json_encode(['mensagem'=>'Erro ao Salvar', 'sucesso'=>false]);
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
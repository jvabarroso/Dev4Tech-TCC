<?php 
require_once("conexao.php");

$tabela = 'Equipe';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$nome_equipe = @$postjson['nome_equipe'];
$id_categoria = @$postjson['id_categoria'];
$data_criacao = @$postjson['data_criacao'];
$foto_equipe = @$postjson['foto_equipe'];

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
    data_criacao = :data_criacao,
    foto_equipe = :foto_equipe");	

    $res->bindValue(":nome_equipe", "$nome_equipe");
    $res->bindValue(":id_categoria", "$id_categoria");
    $res->bindValue(":data_criacao", "$data_criacao");
    $res->bindValue(":foto_equipe", "$foto_equipe");

    if($res->execute()){
        $result = json_encode(array('mensagem'=>'Salvo com sucesso!', 'sucesso'=>true));
    } 
    else{
        $result = json_encode(array('mensagem'=>'Erro ao Salvar', 'sucesso'=>false));
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
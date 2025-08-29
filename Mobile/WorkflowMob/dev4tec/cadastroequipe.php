<?php 
require_once("conexao.php");
include_once("upload_equipe.php");

$tabela = 'Equipes';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$nome_equipe = @$postjson['nome_equipe'];
$id_categoria = @$postjson['id_categoria'];
$AdminId = @$postjson['AdminId'];
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
        id_empresa = :id_empresa");	

    $res->bindValue(":nome_equipe", "$nome_equipe");
    $res->bindValue(":id_categoria", "$id_categoria");
    $res->bindValue(":AdminId", "$AdminId");
    $res->bindValue(":id_empresa", "$id_empresa");


    if ($res->execute()) {
        $idEquipe = $pdo->lastInsertId();

        if ($photo_tmp_name && $upload_path) {
            if (move_uploaded_file($photo_tmp_name, $upload_path)) {
                $stmt = $pdo->prepare("UPDATE $tabela SET foto_equipe = :foto WHERE id_equipe = :id");
                $stmt->bindValue(':foto', $random_name);
                $stmt->bindValue(':id', $idEquipe, PDO::PARAM_INT); 
                $stmt->execute();
            } else {
                echo json_encode(['success' => false, 'message' => 'Erro ao mover arquivo']);
                exit();
            }
        }

        $result = json_encode(['mensagem'=>'Salvo com sucesso!', 'sucesso'=>true, 'id_equipe'=>$idEquipe, 'foto'=>$random_name]);
    } else {
        $result = json_encode(['mensagem'=>'Erro ao Salvar', 'sucesso'=>false]);
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
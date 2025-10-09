<?php 
require_once("conexao.php");
include_once("upload_equipe.php");

$tabela = 'Equipes';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$nome_equipe = @$postjson['nome_equipe'];
$id_categoria = @$postjson['id_categoria'];
$nome_categoria = @$postjson['nome_categoria'];
$AdminId = @$postjson['AdminId'];
$foto_equipe = @$postjson['foto_equipe'];
$id_empresa = @$postjson['id_empresa'];

try {
    if($id_categoria == null) {
        $res = $pdo->prepare("INSERT INTO categorias SET 
            nome_categoria = :nome_categoria, 
            id_empresa = :id_empresa");	

        $res->bindValue(":nome_categoria", "$nome_categoria");
        $res->execute();
        $id_categoria = $pdo->lastInsertId();
    }

    $query = $pdo->prepare("SELECT * FROM categorias  WHERE nome_categoria = :nome_categoria");
    $query->bindValue(':nome_categoria', $nome_categoria, PDO::PARAM_STR);
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
        
        if (!empty($postjson['funcionarios']) && is_array($postjson['funcionarios'])) {
            foreach ($postjson['funcionarios'] as $funcId) {
                $res2 = $pdo->prepare("INSERT INTO equipes_membros SET 
                    FuncionarioId = :FuncionarioId,
                    id_equipe = :id_equipe
                ");
                $res2->bindValue(":FuncionarioId", $funcId);
                $res2->bindValue(":id_equipe", $idEquipe);
                $res2->execute();
            }
        }

        $result = json_encode(['mensagem'=>'Salvo com sucesso!', 'sucesso'=>true, 'id_equipe'=>$idEquipe, 'foto'=>$foto_equipe]);
    } else {
        $result = json_encode(['mensagem'=>'Erro ao Salvar', 'sucesso'=>false]);
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
<?php 
include_once('conexao.php');

$tabela = 'equipes_membros';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);



if ($postjson === null) {
    echo json_encode(['success' => false, 'message' => 'Dados inválidos']);
    exit();
}

if (empty($postjson['nome_equipe']) || empty($postjson['id_categoria'])) {
    echo json_encode(['success' => false, 'message' => 'Preencha todos os campos obrigatórios']);
    exit();
}

try {
    $query = $pdo->prepare("UPDATE equipes SET
        nome_equipe = :nome_equipe,
        id_categoria = :id_categoria,
        foto_equipe = :foto_equipe
        WHERE id_equipe = :id"); 

        $query->bindValue(":nome_equipe", $postjson['nome_equipe']);
        $query->bindValue(":id_categoria", $postjson['id_categoria']);
        $query->bindValue(":foto_equipe", $postjson['foto_equipe']);
        $query->bindValue(":id", $postjson['id']);
        $query->execute();

        foreach ($postjson['funcionarios'] as $f) {
            $id = $f['FuncionarioId'];
            $res = $pdo->prepare("INSERT INTO $tabela SET id_equipe = :id_equipe, FuncionarioId  = :id");
            $res->bindValue(":id_equipe", $postjson['id']);
            $res->bindValue(":id", $id);
            $res->execute();
        }

        if ($query->rowCount() > 0) {
            // Busca os dados atualizados
            $query2 = $pdo->prepare("SELECT * FROM equipes WHERE id_equipe = :id");
            $query2->bindValue(":id", $postjson['id']);
            $query2->execute();
            $equipe = $query2->fetch(PDO::FETCH_ASSOC);
            
            echo json_encode([
                'success' => true,
                'usuario' => [
                    'id' => $equipe['id_equipe'],
                    'nome_equipe' => $equipe['nome_equipe'],
                    'id_categoria' => $equipe['id_categoria'],
                    'foto_equipe' => $equipe['foto_equipe']
                ]
            ]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Nenhum dado foi alterado']);
        }
    }
    catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: '
    ]);
}

?>
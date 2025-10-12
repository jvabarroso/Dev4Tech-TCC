<?php 
include_once('conexao.php');

$tabela = 'equipes_membros';

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);


if ($postjson === null) {
    echo json_encode(['success' => false, 'message' => 'Dados inválidos']);
    exit();
}

$id = $postjson['id'] ?? null;
$nome_equipe = $postjson['nome_equipe'] ?? null;
$id_categoria = $postjson['id_categoria'] ?? null;
$foto_equipe = $postjson['foto_equipe'] ?? null; 
$funcionarios = $postjson['funcionarios'] ?? [];

if (empty($id) || empty($nome_equipe) || empty($id_categoria)) {
    echo json_encode(['success' => false, 'message' => 'Preencha todos os campos obrigatórios: id, nome_equipe, id_categoria']);
    exit;
}

try {

    // Inicia transação
    $pdo->beginTransaction();
    // Atualiza dados da equipe
    $sqlUpdate = "UPDATE equipes SET
                    nome_equipe = :nome_equipe,
                    id_categoria = :id_categoria,
                    foto_equipe = :foto_equipe
                  WHERE id_equipe = :id_equipe";
    $stmt = $pdo->prepare($sqlUpdate);
    $stmt->bindValue(':nome_equipe', $nome_equipe);
    $stmt->bindValue(':id_categoria', $id_categoria);
    $stmt->bindValue(':foto_equipe', $foto_equipe);
    $stmt->bindValue(':id_equipe', $id);    
    $stmt->execute();

    if (!empty($funcionarios) && is_array($funcionarios)) {
        foreach ($postjson['funcionarios'] as $funcId) {
            $res2 = $pdo->prepare("INSERT INTO equipes_membros SET 
                FuncionarioId = :FuncionarioId,
                id_equipe = :id_equipe
            ");
            $res2->bindValue(":FuncionarioId", $funcId);
            $res2->bindValue(":id_equipe", $id);
            $res2->execute();
        }
    }

    // Confirma transação
    $pdo->commit();

    // Busca dados atualizados para retornar (incluindo foto_equipe)
    $q = $pdo->prepare("SELECT id_equipe, nome_equipe, id_categoria, foto_equipe FROM equipes WHERE id_equipe = :id");
    $q->bindValue(':id', $id);
    $q->execute();
    $equipe = $q->fetch(PDO::FETCH_ASSOC);

    echo json_encode([
        'success' => true,
        'message' => 'Equipe atualizada com sucesso',
        'equipe' => $equipe
    ]);
    exit;

} catch (PDOException $e) {
    // desfaz transação em caso de erro
    if ($pdo->inTransaction()) {
        $pdo->rollBack();
    }
    error_log("Erro editarequipe.php: " . $e->getMessage());
    echo json_encode(['success' => false, 'message' => 'Erro no banco de dados', 'detail' => $e->getMessage()]);
    exit;
} catch (Exception $e) {
    if ($pdo->inTransaction()) {
        $pdo->rollBack();
    }
    error_log("Erro editarequipe.php (geral): " . $e->getMessage());
    echo json_encode(['success' => false, 'message' => 'Erro interno', 'detail' => $e->getMessage()]);
    exit;
}
?>
<?php
include_once('conexao.php');

$id_equipe = $_GET['id_equipe'] ?? null;

if (empty($id_equipe)) {
    error_log("Erro: ID da Equipe não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID da Equipe não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    
    $query = $pdo->prepare("SELECT 
        f.FuncionarioId,
        f.nome, 
        f.cargo, 
        f.foto_perfil
    FROM funcionarios f
    INNER JOIN Equipes_Membros em ON f.FuncionarioId = em.FuncionarioId
    WHERE em.id_equipe = :id_equipe");

    
    $query->bindValue(':id_equipe', $id_equipe, PDO::PARAM_INT);
    $query->execute();
    $funcionarios = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Funcionarios encontradas: " . count($funcionarios));

    echo json_encode([
        'success' => true,
        'result' => $funcionarios
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados'
    ]);
}
?>
<?php
include_once('conexao.php');

$id_empresa = $_GET['id_empresa'] ?? null;
$id_equipe = $_GET['id_equipe'] ?? null;

if (empty($id_empresa)) {
    error_log("Erro: ID da empresa não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID da empresa não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    
    $query = $pdo->prepare("SELECT f.FuncionarioId, f.nome 
        FROM funcionarios f
        WHERE f.id_empresa = :id_empresa
        AND f.FuncionarioId NOT IN (
            SELECT FuncionarioId 
            FROM Equipes_Membros 
            WHERE id_equipe = :id_equipe)");
    
    $query->bindValue(':id_empresa', $id_empresa, PDO::PARAM_INT);
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
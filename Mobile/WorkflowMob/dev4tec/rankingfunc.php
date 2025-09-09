<?php
include_once('conexao.php');

$id_equipe= $_GET['id_equipe'] ?? null;

if (empty($id_equipe)) {
    error_log("Erro: ID da equipe não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID da equipe não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    error_log("Buscando equipes " . $id_equipe);
    
    $query = $pdo->prepare("SELECT 
        f.FuncionarioId, 
        f.Nome, 
        f.Cargo, 
        f.foto_perfil,
        COALESCE(pf.pontos, 0) AS pontos
    FROM Equipes_Membros em
    INNER JOIN Funcionarios f ON f.FuncionarioId = em.FuncionarioId
    LEFT JOIN PontuacaoFuncionario pf ON f.FuncionarioId = pf.id_funcionario
    WHERE em.id_equipe = :id_equipe
    ORDER BY pf.pontos DESC");
    
    $query->bindValue(':id_empresa', $id_empresa, PDO::PARAM_INT);
    $query->execute();
    $equipes = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Equipes encontradas: " . count($equipes));

    echo json_encode([
        'success' => true,
        'result' => $equipes
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados'
    ]);
}
?>
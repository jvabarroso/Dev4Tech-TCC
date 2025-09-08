<?php
include_once('conexao.php');

// Obter o ID do funcionário
$id_funcionario = $_GET['id_funcionario'] ?? null;

if (empty($id_funcionario)) {
    error_log("Erro: ID do funcionário não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID do funcionário não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    error_log("Buscando equipes para o funcionário ID: " . $id_funcionario);
    
    $query = $pdo->prepare("SELECT 
        e.id_equipe, 
        e.nome_equipe, 
        e.id_categoria, 
        e.data_criacao,
        e.foto_equipe,
        c.nome_categoria
    FROM Equipes e
    JOIN Equipes_Membros em ON e.id_equipe = em.id_equipe
    JOIN Categorias c ON e.id_categoria = c.id_categoria
    WHERE em.FuncionarioId = :id_funcionario");
    
    $query->bindValue(':id_funcionario', $id_funcionario, PDO::PARAM_INT);
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
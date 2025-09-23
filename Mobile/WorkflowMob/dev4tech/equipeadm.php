<?php
include_once('conexao.php');

// Obter o ID do funcionário
$id_administrador = $_GET['id_administrador'] ?? null;

if (empty($id_administrador)) {
    error_log("Erro: ID do Administrador não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID do Administrador não fornecido',
        'received_data' => $_GET 
    ]);
    exit();
}

try {
    error_log("Buscando equipes para o Administrador ID: " . $id_administrador);
    
    $query = $pdo->prepare("SELECT 
        e.id_equipe, 
        e.nome_equipe, 
        e.id_categoria, 
        e.data_criacao,
        CONCAT('http://10.239.0.125/dev4tech/img/', foto_equipe) AS foto_url,
        c.nome_categoria
    FROM Equipes e
    JOIN Categorias c ON e.id_categoria = c.id_categoria
    WHERE e.AdminId = :id_administrador");
    
    $query->bindValue(':id_administrador', $id_administrador, PDO::PARAM_INT);
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
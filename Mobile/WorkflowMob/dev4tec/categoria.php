<?php
include_once('conexao.php');

$id_empresa = $_GET['id_empresa'] ?? null;

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
    
    $query = $pdo->prepare("SELECT * FROM categorias  WHERE id_empresa = :id_empresa");
    
    $query->bindValue(':id_empresa', $id_empresa, PDO::PARAM_INT);
    $query->execute();
    $categorias = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Categorias encontradas: " . count($categorias));

    echo json_encode([
        'success' => true,
        'result' => $categorias
    ]);

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados'
    ]);
}
?>
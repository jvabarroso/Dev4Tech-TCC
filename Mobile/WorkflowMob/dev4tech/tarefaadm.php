<?php
include_once('conexao.php');

// Obter o ID do funcionário
$id_equipe = $_GET['id_equipe'] ?? null;
error_log("ID da equipe recebido: " . var_export($id_equipe, true));

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
    error_log("Buscando tarefas para o funcionário ID: " . $id_equipe);
    
    // Consulta corrigida usando JOIN com Equipes_Membros
    $query = $pdo->prepare("SELECT 
            t.id_tarefa,
            t.nomeTarefa,
            t.instrucoes,
            t.dificuldade,
            DATE_FORMAT(t.data_entrega, '%Y-%m-%d') AS data_entrega,
            EXISTS (
                SELECT *
                FROM EntregasTarefa et
                WHERE et.id_tarefa = t.id_tarefa
                    AND et.id_equipe = t.id_equipe
            ) AS entregue
        FROM Tarefas t
        WHERE t.id_equipe= :id_equipe
        ORDER BY t.data_entrega ASC");
    
    error_log("Consulta preparada com sucesso");
    
    $query->bindValue(':id_equipe', $id_equipe, PDO::PARAM_INT);
    $query->execute();
    error_log("Consulta executada com sucesso");
    
    $tarefas = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Tarefas retornadas: " . print_r($tarefas, true));


    echo json_encode([
        'success' => true,
        'result' => $tarefas
    ]);
    

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
}
?>
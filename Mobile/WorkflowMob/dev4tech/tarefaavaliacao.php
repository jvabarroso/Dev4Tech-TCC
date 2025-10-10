<?php
include_once('conexao.php');

// Obter o ID do funcionário
$AdminId = $_GET['AdminId'] ?? null;
error_log("ID do admiministrador recebido: " . var_export($AdminId, true));

if (empty($AdminId)) {
    error_log("Erro: ID do administrador não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID do funcionário não fornecido',
        'received_data' => $_GET
    ]);
    exit();
}

try {
    error_log("Buscando tarefas enviadas " . $AdminId);
    
    // Consulta corrigida usando JOIN com Equipes_Membros
    $query = $pdo->prepare("SELECT 
        f.FuncionarioId,
        t.nomeTarefa,
        e.nome_equipe,
        t.dificuldade,
        t.id_tarefa,
        et.nome_arquivo,
        DATE_FORMAT(t.data_entrega, '%Y-%m-%d') AS data_entrega,
        DATE_FORMAT(t.data_criacao, '%Y-%m-%d') AS data_criacao
        FROM Funcionarios f
        JOIN entregastarefa et ON f.FuncionarioId = et.FuncionarioId
        JOIN Tarefas t ON et.id_tarefa = t.id_tarefa    
        JOIN equipes e ON t.id_equipe = e.id_equipe
        WHERE e.AdminId = :AdminId
        AND et.entregue = 0
        ORDER BY t.data_entrega ASC");
        
    error_log("Consulta preparada com sucesso");
    
    $query->bindValue(':AdminId', $AdminId, PDO::PARAM_INT);
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
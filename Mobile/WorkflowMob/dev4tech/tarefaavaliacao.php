<?php
include_once('conexao.php');

// Obter o ID do funcionário
$AdminId = $_GET['AdminId'] ?? null;
$id_equipe = $_GET['id_equipe'] ?? null;
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
        et.id_entrega,
        DATE_FORMAT(t.data_entrega, '%Y-%m-%d') AS data_limite_entrega,
        DATE_FORMAT(et.data_entrega, '%Y-%m-%d') AS data_envio_entrega,
        DATE_FORMAT(t.data_criacao, '%Y-%m-%d') AS data_criacao
        FROM Funcionarios f
        JOIN entregastarefa et ON f.FuncionarioId = et.FuncionarioId
        JOIN Tarefas t ON et.id_tarefa = t.id_tarefa    
        JOIN equipes e ON t.id_equipe = e.id_equipe
        WHERE e.AdminId = :AdminId
        AND et.entregue = 0
        AND e.id_equipe = :id_equipe
        ORDER BY t.data_entrega ASC");
    
    $query->bindValue(':AdminId', $AdminId, PDO::PARAM_INT);
    $query->bindValue(':id_equipe', $id_equipe, PDO::PARAM_INT);
    $query->execute();
    $tarefas = $query->fetchAll(PDO::FETCH_ASSOC);
    $tarefasComProblemas = [];

    foreach ($tarefas as $tarefa) {
        $id_tarefa = $tarefa['id_tarefa'];
        
        $queryProblemas = $pdo->prepare("SELECT 
            descricao, idProblema
            FROM RelatoProblema 
            WHERE id_tarefa = :id_tarefa
            ORDER BY idProblema ASC");
            
        $queryProblemas->bindValue(':id_tarefa', $id_tarefa, PDO::PARAM_INT);
        $queryProblemas->execute();
        $problemas = $queryProblemas->fetchAll(PDO::FETCH_ASSOC);
        
        $tarefa['problemas'] = $problemas;
        $tarefa['problema_relatado'] = !empty($problemas);
        
        $tarefasComProblemas[] = $tarefa;
    }

    error_log("Tarefas retornadas: " . print_r($tarefas, true));


    echo json_encode([
        'success' => true,
        'result' => $tarefasComProblemas
    ]);
    

} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
}
?>
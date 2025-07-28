<?php
include_once('conexao.php');

// Obter o ID do funcionário
$id_funcionario = $_GET['id_funcionario'] ?? null;
error_log("ID do funcionário recebido: " . var_export($id_funcionario, true));

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
    error_log("Buscando tarefas para o funcionário ID: " . $id_funcionario);
    
    // Consulta corrigida usando JOIN com Equipes_Membros
    $query = $pdo->prepare("SELECT 
            t.id_tarefa,
            t.nomeTarefa,
            t.instrucoes,
            DATE_FORMAT(t.data_entrega, '%Y-%m-%d') as data_entrega,
            t.dificuldade,
            e.nome_equipe,
            EXISTS (
                SELECT * FROM EntregasTarefa et
                JOIN Equipes_Membros em_sub ON et.id_equipe = em_sub.id_equipe
                WHERE et.id_tarefa = t.id_tarefa
                AND em_sub.FuncionarioId = :id_funcionario_exist
            ) as entregue
        FROM Tarefas t
        JOIN Equipes e ON t.id_equipe = e.id_equipe
        JOIN Equipes_Membros em ON t.id_equipe = em.id_equipe
        WHERE em.FuncionarioId = 1
      ORDER BY t.data_entrega ASC");
    
    error_log("Consulta preparada com sucesso");

    $query->bindValue(':id_funcionario', $id_funcionario, PDO::PARAM_INT);
    $query->bindValue(':id_funcionario_exist', $id_funcionario, PDO::PARAM_INT);
    $query->execute();
    error_log("Consulta executada com sucesso");
    
    $tarefas = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Tarefas retornadas: " . print_r($tarefas, true));

    foreach ($tarefas as &$tarefa) {
        $tarefa['data_entrega'] = date('Y-m-d', strtotime($tarefa['data_entrega']));
        
        $tarefa['entregue'] = (bool) $tarefa['entregue'];

        switch (strtolower($tarefa['dificuldade'])) {
            case 'fácil': 
                $tarefa['dificuldade_texto'] = 'Fácil'; 
                $tarefa['dificuldade_icone'] = '⭐'; 
                break;
            case 'média': 
                $tarefa['dificuldade_texto'] = 'Média'; 
                $tarefa['dificuldade_icone'] = '⭐⭐'; 
                break;
            case 'difícil': 
                $tarefa['dificuldade_texto'] = 'Difícil'; 
                $tarefa['dificuldade_icone'] = '⭐⭐⭐'; 
                break;
            default: 
                $tarefa['dificuldade_texto'] = $tarefa['dificuldade']; 
                $tarefa['dificuldade_icone'] = '⭐';
        }
    }
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
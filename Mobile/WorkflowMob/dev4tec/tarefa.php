<?php
include_once('conexao.php');

// Obter o ID do funcionário
$id_funcionario = $_GET['id_funcionario'] ?? null;

if (empty($id_funcionario)) {
    error_log("Erro: ID do funcionário não fornecido");
    echo json_encode([
        'success' => false,
        'message' => 'ID do funcionário não fornecido',
        'received_data' => $_GET // Para depuração
    ]);
    exit();
}

try {
    error_log("Buscando tarefas para o funcionário ID: " . $id_funcionario);
    
    $query = $pdo->prepare("SELECT 
        t.id_tarefa,
        t.nomeTarefa,
        t.instrucoes,
        t.data_entrega,
        t.dificuldade,
        e.nome_equipe,
        at.status as status_tarefa,
        at.data_atribuicao
    FROM Tarefas t
    JOIN Equipes e ON t.id_equipe = e.id_equipe
    JOIN AtribuicoesTarefa at ON t.id_tarefa = at.id_tarefa
    WHERE at.id_funcionario = :id_funcionario
    ORDER BY t.data_entrega ASC");
    
    $query->bindValue(':id_funcionario', $id_funcionario, PDO::PARAM_INT);
    $query->execute();
    $tarefas = $query->fetchAll(PDO::FETCH_ASSOC);

    error_log("Tarefas encontradas: " . count($tarefas));

    foreach ($tarefas as &$tarefa) {
        $tarefa['data_entrega'] = date('d/m/Y', strtotime($tarefa['data_entrega']));
        $tarefa['data_atribuicao'] = date('d/m/Y H:i', strtotime($tarefa['data_atribuicao']));

        switch ($tarefa['dificuldade']) {
            case 'Fácil':
                $tarefa['dificuldade_icone'] = '⭐';
                break;
            case 'Média':
                $tarefa['dificuldade_icone'] = '⭐⭐';
                break;
            case 'Difícil':
                $tarefa['dificuldade_icone'] = '⭐⭐⭐';
                break;
            default:
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
        'message' => 'Erro no banco de dados'
    ]);
}
?>
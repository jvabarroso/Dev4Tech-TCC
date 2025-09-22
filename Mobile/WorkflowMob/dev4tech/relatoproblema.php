<?php 
require_once("conexao.php");

// Recebe os dados JSON
$tabela = 'RelatoProblema';

$postjson = json_decode(file_get_contents('php://input'), true);

$id_tarefa = @$postjson['id_tarefa'];
$id_equipe = @$postjson['id_equipe'];
$descricao = @$postjson['descricao'];
$id_empresa = @$postjson['id_empresa'];

try{
    $res = $pdo->prepare("INSERT INTO $tabela SET 
    id_tarefa = :id_tarefa, 
    id_equipe = :id_equipe, 
    descricao = :descricao, 
    id_empresa = :id_empresa");	

    $res->bindValue(":id_tarefa", "$id_tarefa");
    $res->bindValue(":id_equipe", "$id_equipe");
    $res->bindValue(":descricao", "$descricao");
    $res->bindValue(":id_empresa", "$id_empresa");


    if($res->execute()){
        // Buscar tarefa atualizada
        $queryProblemas = $pdo->prepare("SELECT descricao FROM RelatoProblema WHERE id_tarefa = :id_tarefa");
        $queryProblemas->bindValue(':id_tarefa', $id_tarefa);
        $queryProblemas->execute();
        $problemas = $queryProblemas->fetchAll(PDO::FETCH_COLUMN);
        
        // Buscar tarefa atualizada
        $query = $pdo->prepare("SELECT *, 
            CASE WHEN EXISTS 
            (SELECT 1 FROM RelatoProblema WHERE id_tarefa = :id_tarefa) 
            THEN 1 ELSE 0 END AS selproblema 
        FROM Tarefas 
        WHERE id_tarefa = :id_tarefa");
            
        $query->bindValue(':id_tarefa', $id_tarefa ?: null, PDO::PARAM_INT);
        $query->execute();
        $tarefa = $query->fetch(PDO::FETCH_ASSOC);

        echo json_encode([
            'mensagem' => 'Problema salvo com sucesso!',
            'sucesso' => true,
            'tarefa' => $tarefa,
            'problemas' => $problemas  // ← RETORNA TODOS OS PROBLEMAS
        ]);
    } else {
        echo json_encode([
            'mensagem' => 'Erro ao salvar o problema',
            'sucesso' => false
        ]);
    }

} catch (PDOException $e) {
    echo json_encode([
        'mensagem' => 'Erro: ' . $e->getMessage(),
        'sucesso' => false
    ]);
}
?>
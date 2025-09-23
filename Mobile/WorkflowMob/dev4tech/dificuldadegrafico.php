<?php
include_once('conexao.php');

$id_equipe = $_GET['id_equipe'] ?? null;

if (empty($id_equipe)) {
    echo json_encode([
        'success' => false,
        'message' => 'ID da Equipe não fornecido'
    ]);
    exit();
}

try {
    $query = $pdo->prepare("SELECT 
        t.dificuldade, COUNT(*) as total
        FROM tarefas t
        INNER JOIN entregastarefa et ON t.id_tarefa = et.id_tarefa
        WHERE t.id_equipe = :id_equipe
        GROUP BY t.dificuldade
    ");
    $query->bindValue(':id_equipe', $id_equipe, PDO::PARAM_INT);
    $query->execute();
    
    $dadosBrutos = $query->fetchAll(PDO::FETCH_ASSOC);
    
    $dificuldadeNormalizada = array_map(function($item) {
        $map = [
            'Fácil' => 'facil',
            'Médio' => 'media',
            'Difícil' => 'dificil'
        ];
        
        return [
            'dificuldade' => $map[$item['dificuldade']] ?? strtolower($item['dificuldade']),
            'total' => $item['total']
        ];
    }, $dadosBrutos);

    echo json_encode([
        'success' => true,
        'result' => $dificuldadeNormalizada
    ]);

} catch (PDOException $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
}
?>
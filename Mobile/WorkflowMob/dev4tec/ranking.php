<?php
include_once('conexao.php');

$id_empresa= $_GET['id_empresa'] ?? null;

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
    error_log("Buscando equipes com o id da empresa " . $id_empresa);
    
    $query = $pdo->prepare("SELECT 
        e.id_equipe, 
        e.nome_equipe, 
        e.id_categoria, 
        c.nome_categoria,
        e.foto_equipe,
        COALESCE(SUM(pf.pontos), 0) AS pontuacao_total
    FROM Equipes e
    JOIN Equipes_Membros em ON e.id_equipe = em.id_equipe
    JOIN Funcionarios f ON em.FuncionarioId = f.FuncionarioId
    LEFT JOIN PontuacaoFuncionario pf ON f.FuncionarioId = pf.id_funcionario
    JOIN Categorias c ON e.id_categoria = c.id_categoria
    WHERE e.id_empresa = :id_empresa
    GROUP BY e.id_equipe, e.nome_equipe, e.id_categoria, e.data_criacao, c.nome_categoria
    ORDER BY pontuacao_total DESC");
    
    $query->bindValue(':id_empresa', $id_empresa, PDO::PARAM_INT);
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
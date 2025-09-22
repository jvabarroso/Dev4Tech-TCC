<?php
include_once('conexao.php');

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$role = $postjson['role'] ?? null; // Use null coalescing para evitar warnings
$id = $postjson['id'] ?? null;

if (!$role || !$id) {
    echo json_encode([
        'success' => false, 
        'message' => 'Role ou ID não fornecido']);
    exit();
}

if ($postjson === null) {
    echo json_encode([
        'success' => false, 
        'message' => 'Dados inválidos']);
    exit();
}

try {
    if ($postjson['role'] === 'funcionario'){
        $query = $pdo->prepare("SELECT foto_perfil FROM Funcionarios WHERE FuncionarioId = :id");
        $query->bindValue(':id', $id, PDO::PARAM_INT);
    }
    else if ($postjson['role'] === 'administrador'){
        $query = $pdo->prepare("SELECT foto_perfil FROM Administradores WHERE AdminId = :id");
        $query->bindValue(':id', $id, PDO::PARAM_INT);
    }       
    else{
        echo json_encode(['success' => false, 'message' => 'Role inválida']);
        exit();
    } 
    
    $query->execute();
    $resultado = $query->fetch(PDO::FETCH_ASSOC);

    if ($resultado && !empty($resultado['foto_perfil'])) {
        $diretorio = 'img/';
        $foto = $resultado['foto_perfil'];
        $caminhoCompleto = $diretorio . $foto;

        if (file_exists($caminhoCompleto)) {
            $urlCompleta = "http://10.239.0.125/dev4tech/img/" . $foto;
            echo json_encode(['success' => true, 'imagem' => $urlCompleta], JSON_UNESCAPED_UNICODE);
        } else {
            echo json_encode([]);
        }
    } 

} catch (Exception $e) {
    echo json_encode(['error' => $e->getMessage()]);
}


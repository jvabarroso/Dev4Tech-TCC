<?php
include_once('conexao.php');

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$id = $postjson['id'] ?? null;

if (!$id) {
    echo json_encode([
        'success' => false, 
        'message' => 'ID não fornecido']);
    exit();
}

if ($postjson === null) {
    echo json_encode([
        'success' => false, 
        'message' => 'Dados inválidos']);
    exit();
}

try {

    $query = $pdo->prepare("SELECT foto_equipe FROM equipes WHERE id_equipe = :id");
    $query->bindValue(':id', $id, PDO::PARAM_INT);

    $query->execute();
    $resultado = $query->fetch(PDO::FETCH_ASSOC);

    if ($resultado && !empty($resultado['foto_equipe'])) {
        $diretorio = 'img/';
        $foto = $resultado['foto_equipe'];
        $caminhoCompleto = $diretorio . $foto;

        if (file_exists($caminhoCompleto)) {
            $urlCompleta = "http://10.239.0.125/dev4tec/img/" . $foto;
            echo json_encode(['success' => true, 'imagem' => $urlCompleta], JSON_UNESCAPED_UNICODE);
        } else {
            echo json_encode(['success' => false, 'message' => 'Imagem não encontrada']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Equipe sem imagem']);
    }

} catch (Exception $e) {
    echo json_encode(['error' => $e->getMessage()]);
}


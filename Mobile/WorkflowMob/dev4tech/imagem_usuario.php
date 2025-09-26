<?php
include_once('conexao.php');

$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$role = $postjson['role'] ?? null;
$id   = $postjson['id'] ?? null;

if (!$role || !$id) {
    echo json_encode(['success' => false, 'message' => 'Role ou ID não fornecido']);
    exit();
}

try {
    if ($role === 'funcionario'){
        $query = $pdo->prepare("SELECT foto_perfil FROM Funcionarios WHERE FuncionarioId = :id");
        $query->bindValue(':id', $id, PDO::PARAM_INT);
    }
    else if ($role === 'administrador'){
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
        $raw = trim($resultado['foto_perfil']);

        // normaliza barras e remove espaços
        $raw = str_replace('\\', '/', $raw);

        // se contiver caminho absoluto (começa com / ou com letra e : no Windows) trate como arquivo já completo
        $isAbsolute = (strpos($raw, '/') === 0) || preg_match('/^[A-Za-z]:\//', $raw);

        // remove prefixo "img/" ou "/img/" se existir
        if (!$isAbsolute) {
            if (stripos($raw, 'img/') === 0) {
                $raw = substr($raw, 4);
            }
            if (stripos($raw, '/img/') === 0) {
                $raw = substr($raw, 5);
            }
        }

        // caminho físico no servidor
        $fileOnDisk = __DIR__ . '/img/' . $raw;

        if ($isAbsolute) {
            // se veio absoluto, sobrescreve
            $fileOnDisk = $raw;
        }

        if (file_exists($fileOnDisk)) {
            // monta URL base dinamicamente (esquema + host + pasta do script)
            $scheme = (!empty($_SERVER['HTTPS']) && $_SERVER['HTTPS'] !== 'off') ? 'https' : 'http';
            $host = $_SERVER['HTTP_HOST'];
            $scriptDir = rtrim(dirname($_SERVER['SCRIPT_NAME']), '/\\');

            // monta URL final para a imagem (ex: http://10.239.20.68/dev4tech/img/arquivo.jpg)
            $urlImagem = $scheme . '://' . $host . $scriptDir . '/img/' . $raw;

            echo json_encode(['success' => true, 'imagem' => $urlImagem], JSON_UNESCAPED_UNICODE);
            exit();
        } else {
            // arquivo não existe
            echo json_encode(['success' => false, 'message' => 'Arquivo não encontrado', 'file' => $fileOnDisk]);
            exit();
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Nenhuma foto cadastrada']);
        exit();
    }

} catch (Exception $e) {
    echo json_encode(['success' => false, 'error' => $e->getMessage()]);
}
?>

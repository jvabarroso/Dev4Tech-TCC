<?php
include_once('conexao.php');

$uploadDir = __DIR__ . "/img/";

// cria a pasta se necessário
if (!is_dir($uploadDir)) {
    if (!mkdir($uploadDir, 0777, true)) {
        echo json_encode(['success' => false, 'message' => 'Falha ao criar diretório de upload']);
        exit;
    }
}

try {
    if (!empty($_FILES['photo']) && $_FILES['photo']['error'] === UPLOAD_ERR_OK) {
        $origName = basename($_FILES['photo']['name']);
        $random_name = uniqid() . "-" . $origName;
        $random_name = preg_replace('/\s+/', '-', $random_name); // corrigido

        $upload_path = $uploadDir . $random_name;

        if (move_uploaded_file($_FILES['photo']['tmp_name'], $upload_path)) {
            $file_url = (isset($_SERVER['HTTP_HOST']) ? (isset($_SERVER['REQUEST_SCHEME']) ? $_SERVER['REQUEST_SCHEME'] : 'http') . '://' . $_SERVER['HTTP_HOST'] : 'http://10.239.0.125') 
                        . dirname($_SERVER['SCRIPT_NAME']) . "/img/" . $random_name;
            // Normalmente você pode montar a URL fixa:
            // $file_url = "http://10.239.20.68/dev4tec/img/" . $random_name;
            echo json_encode([
                'success' => true,
                'message' => 'Upload realizado com sucesso!',
                'file' => $random_name,
                'url' => $file_url
            ]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Erro ao mover arquivo para o diretório. Verifique permissões.']);
        }
    } else {
        $errorMsg = 'Nenhum arquivo enviado';
        if (!empty($_FILES['photo']['error'])) {
            $errorMsg .= ' - error code: ' . $_FILES['photo']['error'];
        }
        echo json_encode(['success' => false, 'message' => $errorMsg]);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>

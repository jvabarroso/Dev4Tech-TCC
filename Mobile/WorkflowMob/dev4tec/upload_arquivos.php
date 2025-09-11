<?php
include_once('conexao.php');
  
try {
    if(isset($_FILES['file'])){
        $file_name = $_FILES["file"]["name"];
        $file_tmp_name = $_FILES["file"]["tmp_name"];

        //MOVE FILE TO SERVER
        $random_name = rand(1000,1000000)."-".$file_name;
        $random_name = preg_replace('/s+/', '-', $random_name);

        // Caminho de upload (na pasta img/)
        $upload_path = __DIR__ . "/arquivos/" . $random_name;

        // Tenta mover o arquivo
        if (move_uploaded_file($file_tmp_name, $upload_path)) {
            echo json_encode(['success' => true, 'message' => 'Upload realizado com sucesso!', 'file' => $random_name]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Erro ao mover arquivo']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Nenhum arquivo enviado']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>
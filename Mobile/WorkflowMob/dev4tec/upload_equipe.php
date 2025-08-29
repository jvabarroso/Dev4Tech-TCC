<?php
include_once('conexao.php');
  
try {
    if(isset($_FILES['photo'])){
        $photo_name = $_FILES["photo"]["name"];
        $photo_tmp_name = $_FILES["photo"]["tmp_name"];

        //MOVE FILE TO SERVER
        $random_name = rand(1000,1000000)."-".$photo_name;
        $random_name = preg_replace('/s+/', '-', $random_name);

        // Caminho de upload (na pasta img/)
        $upload_path = __DIR__ . "/img/" . $random_name;

    } else {
        $random_name = null;
        $upload_path = null;
        $photo_tmp_name = null;
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>
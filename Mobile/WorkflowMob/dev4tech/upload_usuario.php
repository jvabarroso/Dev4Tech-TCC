<?php
include_once('conexao.php');

$role =  $_POST['role'] ?? null; 
$id =  $_POST['id'] ?? null;

if (!$role || !$id) {
    echo json_encode([
        'success' => false, 
        'message' => 'Role ou ID não fornecido']);
    exit();
}
  
try {
    if(isset($_FILES['photo'])){
        $photo_name = $_FILES["photo"]["name"];
        $photo_tmp_name = $_FILES["photo"]["tmp_name"];

        //MOVE FILE TO SERVER
        $random_name = rand(1000,1000000)."-".$photo_name;
        $random_name = preg_replace('/s+/', '-', $random_name);

        // Caminho de upload (na pasta img/)
        $upload_path = __DIR__ . "/img/" . $random_name;

        if(move_uploaded_file($photo_tmp_name, $upload_path)) {  
            if ($role === 'funcionario'){
                $stmt = $pdo->prepare("UPDATE Funcionarios SET foto_perfil = :foto WHERE FuncionarioId = :id");
            }
            else if ($role === 'administrador'){
                $stmt = $pdo->prepare("UPDATE Administradores SET foto_perfil = :foto WHERE AdminId = :id");
            }       
            else{
                echo json_encode(['success' => false, 'message' => 'Role inválida']);
                exit();
            } 
            $stmt->bindValue(':foto', $random_name);
            $stmt->bindValue(':id', $id, PDO::PARAM_INT);
            
            if ($stmt->execute()) {
                echo json_encode(['success' => true, 'file' => $random_name]);
            } else {
                echo json_encode(['success' => false, 'message' => 'Erro ao atualizar no banco']);
            }
        } else {
            echo json_encode(['success' => false, 'message' => 'Erro ao mover arquivo']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Nenhuma imagem recebida']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>
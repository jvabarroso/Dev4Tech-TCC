<?php
include_once('conexao.php');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);
    $idTarefa = $input['id_tarefa'] ?? '';
    $status = $input['status'] ?? '';
    
    if (empty($idTarefa) || empty($status)) {
        echo json_encode(['success' => false, 'message' => 'Dados insuficientes']);
        exit;
    }

    try {
        global $pdo;
        
        // Primeiro verificar se a coluna status_tarefa existe
        $checkColumn = $pdo->query("SHOW COLUMNS FROM Tarefas LIKE 'status_tarefa'");
        if ($checkColumn->rowCount() == 0) {
            // Se a coluna não existe, adicionar
            $pdo->query("ALTER TABLE Tarefas ADD COLUMN status_tarefa VARCHAR(20) DEFAULT 'pendente'");
        }
        
        // Atualizar status da tarefa
        $stmt = $pdo->prepare("UPDATE Tarefas SET status_tarefa = ? WHERE id_tarefa = ?");
        
        if ($stmt->execute([$status, $idTarefa])) {
            echo json_encode(['success' => true, 'message' => 'Status atualizado com sucesso']);
        } else {
            $error = $stmt->errorInfo();
            echo json_encode(['success' => false, 'message' => 'Erro ao atualizar status: ' . $error[2]]);
        }
        
    } catch (Exception $e) {
        echo json_encode(['success' => false, 'message' => 'Erro: ' . $e->getMessage()]);
    }
    exit;
}
?>
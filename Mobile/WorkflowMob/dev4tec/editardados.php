<?php 
include_once('conexao.php');

function limparTelefone($telefone) {
    return preg_replace('/[^0-9]/', '', $telefone);
}

// Recebe os dados JSON
$input = file_get_contents('php://input');
$postjson = json_decode($input, true);

$role = $postjson['role'] ?? null; // Use null coalescing para evitar warnings

if (!$role) {
    echo json_encode(['success' => false, 'message' => 'Role não definida']);
    exit();
}


// Verifica se o JSON foi decodificado corretamente
if ($postjson === null) {
    echo json_encode(['success' => false, 'message' => 'Dados inválidos']);
    exit();
}

if (!isset($postjson['role']) || empty($postjson['role'])) {
    echo json_encode(['success' => false, 'message' => 'Tipo de usuário não especificado']);
    exit();
}

if (empty($postjson['DataNascimento']) || empty($postjson['Telefone']) || empty($postjson['endereco'])) {
    echo json_encode(['success' => false, 'message' => 'Preencha todos os campos obrigatórios']);
    exit();
}

try {
    if($postjson['role'] === 'funcionario'){
    $telefoneLimpo = limparTelefone($postjson['Telefone']);
    $query = $pdo->prepare("UPDATE Funcionarios SET
        DataNascimento = :DataNascimento,
        Telefone = :Telefone,
        endereco = :endereco
        WHERE FuncionarioId = :id"); 

        $query->bindValue(":DataNascimento", $postjson['DataNascimento']);
        $query->bindValue(":Telefone", $telefoneLimpo);
        $query->bindValue(":endereco", $postjson['endereco']);
        $query->bindValue(":id", $postjson['id']);
        $query->execute();
    
        if ($query->rowCount() > 0) {
            // Busca os dados atualizados
            $query2 = $pdo->prepare("SELECT * FROM Funcionarios WHERE FuncionarioId = :id");
            $query2->bindValue(":id", $postjson['id']);
            $query2->execute();
            $user = $query2->fetch(PDO::FETCH_ASSOC);
            
            echo json_encode([
                'success' => true,
                'usuario' => [
                    'id' => $user['FuncionarioId'],
                    'nome' => $user['Nome'],
                    'email' => $user['Email'],
                    'cargo' => $user['Cargo'],
                    'telefone' => $user['Telefone'],
                    'cpf' => $user['CPF'],
                    'dataNascimento' => $user['DataNascimento'],
                    'endereco' => $user['endereco'],
                    'role' => 'funcionario'
                ]
            ]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Nenhum dado foi alterado']);
        }
    }
    else if ($postjson['role'] === 'administrador'){
        $telefoneLimpo = limparTelefone($postjson['Telefone']);
        $query = $pdo->prepare("UPDATE Administradores SET
            DataNascimento = :DataNascimento,
            Telefone = :Telefone,
            endereco = :endereco
            WHERE AdminId = :id");
        
        $query->bindValue(":DataNascimento", $postjson['DataNascimento']);
        $query->bindValue(":Telefone", $telefoneLimpo);
        $query->bindValue(":endereco", $postjson['endereco']);
        $query->bindValue(":id", $postjson['id']);
        $query->execute();


        if ($query->rowCount() > 0) {
            // Busca os dados atualizados
            $query2 = $pdo->prepare("SELECT * FROM Administradores WHERE AdminId = :id");
            $query2->bindValue(":id", $postjson['id']);
            $query2->execute();
            $user = $query2->fetch(PDO::FETCH_ASSOC);
            
            echo json_encode([
                'success' => true,
                'usuario' => [
                    'id' => $user['AdminId'],
                    'nome' => $user['Nome'],
                    'email' => $user['Email'],
                    'cargo' => $user['Cargo'],
                    'telefone' => $user['Telefone'],
                    'cpf' => $user['CPF'],
                    'dataNascimento' => $user['DataNascimento'],
                    'endereco' => $user['endereco'],
                    'role' => 'administrador'
                ]
            ]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Nenhum registro foi atualizado']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Tipo de usuário não reconhecido']);
    }
} catch (PDOException $e) {
    error_log("Erro no banco de dados: " . $e->getMessage());
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: '
    ]);
}

?>
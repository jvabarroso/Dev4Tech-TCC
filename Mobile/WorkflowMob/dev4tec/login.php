<?php 
include_once('conexao.php');

$postjson = json_decode(file_get_contents('php://input'), true);

$Email = $postjson['Email'] ?? '';
$Senha = $postjson['Senha'] ?? '';

try {
    $query = $pdo->prepare("SELECT 
        FuncionarioId AS id,
        Nome,
        Cargo,
        CPF,
        DataNascimento,
        Telefone,
        Email,
        Senha,
        endereco,
        numero
        FROM Funcionarios WHERE Email = :Email");
    
    $query->bindValue(':Email', $Email);
    $query->execute();
    $userfuncionario = $query->fetch(PDO::FETCH_ASSOC);

    $query2 = $pdo->prepare("SELECT     
        AdminId AS id,
        Nome,
        Cargo,
        CPF,
        DataNascimento,
        Telefone,
        Email,
        Senha,
        data_cadAdmin,
        endereco,
        num
        FROM Administradores WHERE Email = :Email");
    
    $query2->bindValue(':Email', $Email);
    $query2->execute();
    $useradministrador = $query2->fetch(PDO::FETCH_ASSOC);

} catch (PDOException $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
    exit();
}

if ($userfuncionario && $Senha === $userfuncionario['Senha']) {
    $result = [
        'success' => true,
        'role' => 'funcionario', 
        'usuario' => [
            'id' => $userfuncionario['id'],
            'nome' => $userfuncionario['Nome'],
            'email' => $userfuncionario['Email'],
            'cargo' => $userfuncionario['Cargo'],
            'telefone' => $userfuncionario['Telefone'],
            'cpf' => $userfuncionario['CPF'],
            'dataNascimento' => $userfuncionario['DataNascimento'],
            'endereco' => $userfuncionario['endereco'],
            'numero' => $userfuncionario['numero']
        ],
        'message' => 'Login realizado com sucesso!'
    ];
} else if ($useradministrador && $Senha === $useradministrador['Senha']) {
    $result = [
        'success' => true,
        'role' => 'administrador', 
        'usuario' => [
            'id' => $useradministrador['id'],
            'nome' => $useradministrador['Nome'],
            'email' => $useradministrador['Email'],
            'cargo' => $useradministrador['Cargo'],
            'data_cadAdmin' => $useradministrador['data_cadAdmin'],
            'telefone' => $useradministrador['Telefone'],
            'cpf' => $useradministrador['CPF'],
            'dataNascimento' => $useradministrador['DataNascimento'],
            'endereco' => $useradministrador['endereco'],
            'num' => $useradministrador['num']
        ],                   
        'message' => 'Login realizado com sucesso!' 
    ];
} else {
    $result = [
        'success' => false, 
        'message' => 'Email ou senha inválidos'
    ];
}

echo json_encode($result);
?>
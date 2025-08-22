<?php 
include_once('conexao.php');

$postjson = json_decode(file_get_contents('php://input'), true);

$Email = $postjson['Email'] ?? '';
$Senha = $postjson['Senha'] ?? '';

try {
    $query = $pdo->prepare("SELECT 
        f.*, 
        f.FuncionarioId AS id, 
        'funcionario' AS role 
    FROM Funcionarios f 
    WHERE f.Email = :email AND f.Senha = :senha");
    
    $query->bindValue(':email', $Email);
    $query->bindValue(':senha', $Senha);
    $query->execute();
    $userfuncionario = $query->fetch(PDO::FETCH_ASSOC);

    $query2 = $pdo->prepare("SELECT 
        a.*, 
        a.AdminId AS id, 
        'administrador' AS role 
    FROM Administradores a 
    WHERE a.Email = :email AND a.Senha = :senha");
    
    $query2->bindValue(':email', $Email);
    $query2->bindValue(':senha', $Senha);
    $query2->execute();
    $useradministrador = $query2->fetch(PDO::FETCH_ASSOC);

} catch (PDOException $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
    exit();
}

$diretorioImg = 'http://10.239.0.125/dev4tec/img/';
if ($userfuncionario) {
    $fotoUrl = $userfuncionario['foto_perfil'] 
               ? $diretorioImg . $userfuncionario['foto_perfil'] 
               : null;

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
            'numero' => $userfuncionario['numero'],
            'id_empresa' => $userfuncionario['id_empresa'],
            'AdminId' => $userfuncionario['AdminId'],
            'foto_perfil' => $fotoUrl,
            'role' => 'funcionario'
        ],
        'message' => 'Login realizado com sucesso!'
    ];
} else if ($useradministrador) {
    $fotoUrl = $useradministrador['foto_perfil'] 
               ? $diretorioImg . $useradministrador['foto_perfil'] 
               : null;
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
            'num' => $useradministrador['num'],
            'id_empresa' => $useradministrador['id_empresa'],
            'foto_perfil' => $fotoUrl,
            'role' => 'administrador', 
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
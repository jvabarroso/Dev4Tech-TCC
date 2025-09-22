<?php 
include_once('conexao.php');

$postjson = json_decode(file_get_contents('php://input'), true);

$Email = $postjson['Email'] ?? '';
$Senha = $postjson['Senha'] ?? '';

try {
    $query = $pdo->prepare("SELECT 
        f.FuncionarioId,
        f.Nome,
        f.Cargo,
        f.CPF,
        f.DataNascimento,
        f.Telefone,
        f.Email,
        f.endereco,
        f.numero,
        f.id_empresa,
        f.foto_perfil,
        f.senha,
        'funcionario' AS role 
    FROM Funcionarios f 
    WHERE f.Email = :email AND f.Senha = :senha");
    
    $query->bindValue(':email', $Email);
    $query->bindValue(':senha', $Senha);
    $query->execute();
    $userfuncionario = $query->fetch(PDO::FETCH_ASSOC);

    $query2 = $pdo->prepare("SELECT 
        a.AdminId,
        a.Nome,
        a.Cargo,
        a.CPF,
        a.DataNascimento,
        a.Telefone,
        a.Email,
        a.endereco,
        a.num,
        a.foto_perfil,
        a.id_empresa,
        a.senha,
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

$diretorioImg = 'http://26.205.151.98/dev4tec/img/';
if ($userfuncionario) {
    $fotoUrl = $userfuncionario['foto_perfil'] 
               ? $diretorioImg . $userfuncionario['foto_perfil'] 
               : null;

    $result = [
        'success' => true,
        'role' => 'funcionario',
        'usuario' => [
            'FuncionarioId' => $userfuncionario['FuncionarioId'],
            'nome' => $userfuncionario['Nome'],
            'email' => $userfuncionario['Email'],
            'cargo' => $userfuncionario['Cargo'],
            'telefone' => $userfuncionario['Telefone'],
            'cpf' => $userfuncionario['CPF'],
            'dataNascimento' => $userfuncionario['DataNascimento'],
            'endereco' => $userfuncionario['endereco'],
            'numero' => $userfuncionario['numero'],
            'id_empresa' => $userfuncionario['id_empresa'],
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
            'AdminId' => $useradministrador['AdminId'],
            'nome' => $useradministrador['Nome'],
            'email' => $useradministrador['Email'],
            'cargo' => $useradministrador['Cargo'],
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
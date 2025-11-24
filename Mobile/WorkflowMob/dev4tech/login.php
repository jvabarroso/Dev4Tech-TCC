<?php 
include_once('conexao.php');
include_once('senhasHash.php'); // Inclui as funções de hash

$postjson = json_decode(file_get_contents('php://input'), true);

$Email = $postjson['Email'] ?? '';
$Senha = $postjson['Senha'] ?? '';

try {
    // Busca primeiro no funcionário
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
    WHERE f.Email = :email");
    
    $query->bindValue(':email', $Email);
    $query->execute();
    $userfuncionario = $query->fetch(PDO::FETCH_ASSOC);

    // Se encontrou funcionário, verifica a senha
    if ($userfuncionario) {
        $senhaValida = SenhasHash::verificarSenha($Senha, $userfuncionario['senha']);
        if (!$senhaValida) {
            $userfuncionario = false; // Invalida o usuário se senha não conferir
        }
    }

    // Se não encontrou funcionário ou senha inválida, busca no administrador
    if (!$userfuncionario) {
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
        WHERE a.Email = :email");
        
        $query2->bindValue(':email', $Email);
        $query2->execute();
        $useradministrador = $query2->fetch(PDO::FETCH_ASSOC);

        // Verifica senha do administrador
        if ($useradministrador) {
            $senhaValida = SenhasHash::verificarSenha($Senha, $useradministrador['senha']);
            if (!$senhaValida) {
                $useradministrador = false;
            }
        }
    }

} catch (PDOException $e) {
    echo json_encode([
        'success' => false,
        'message' => 'Erro no banco de dados: ' . $e->getMessage()
    ]);
    exit();
}

$diretorioImg = 'http://10.239.20.68/dev4tec/img/';
if ($userfuncionario && $senhaValida) {
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
} else if ($useradministrador && $senhaValida) {
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
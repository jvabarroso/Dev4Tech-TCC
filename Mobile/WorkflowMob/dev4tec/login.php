<?php 
include_once('conexao.php');

$postjson = json_decode(file_get_contents('php://input'), true);

$Email = $postjson['Email'] ?? '';
$Senha = $postjson['Senha'] ?? '';

$query = $pdo->prepare("SELECT 
    FuncionarioId AS id,
    Nome,
    Cargo,
    CPF,
    DataNascimento,
    Telefone,
    Email,
    Senha,
    endereço AS endereco,  
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
    Senha
    FROM Administradores WHERE Email = :Email");

$query2->bindValue(':Email', $Email);
$query2->execute();
$useradministrador = $query2->fetch(PDO::FETCH_ASSOC);

    if ($userfuncionario && $Senha === $userfuncionario['Senha']) {
           $result = json_encode([
            'success'=>true,
            'role' => 'funcionario', 
            'usuario' =>[
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
        ]);
    }
    else if($useradministrador && $Senha === $useradministrador['Senha']){
           $result = json_encode([
            'success'=>true,
            'role' => 'administrador', 
            'usuario' =>[
                'id' => $useradministrador['id'],
                'nome' => $useradministrador['Nome'],
                'email' => $useradministrador['Email'],
                'cargo' => $useradministrador['Cargo'],
                'telefone' => $useradministrador['Telefone'],
                'cpf' => $useradministrador['CPF'],
                'dataNascimento' => $useradministrador['DataNascimento']
            ],                   
            'message' => 'Login realizado com sucesso!' 
        ]);
    }
    else{
        $result = json_encode(['success'=>false, 'message' => 'Email ou senha inválidos', 'result'=>'0']);
    }
echo $result;
?>
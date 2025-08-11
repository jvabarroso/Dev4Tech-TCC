<?php 
require_once("conexao.php");

// Recebe os dados JSON
$tabela = 'Funcionarios';

$postjson = json_decode(file_get_contents('php://input'), true);

$Nome = @$postjson['Nome'];
$Cargo = @$postjson['Cargo'];
$DataNascimento = @$postjson['DataNascimento'];
$Telefone = @$postjson['Telefone'];
$Email = @$postjson['Email'];
$Senha = @$postjson['Senha'];
$endereco = @$postjson['endereco'];
$id_administradores = $postjson['id_administradores'] ?? null;
$id_empresa = $postjson['id_empresa'] ?? null;
$numero = @$postjson['numero'];

try{
    $res = $pdo->prepare("INSERT INTO $tabela SET 
    Nome = :Nome, 
    Cargo = :Cargo, 
    DataNascimento = :DataNascimento, 
    Telefone = :Telefone, 
    Email = :Email, 
    Senha = :Senha, 
    data_cadFunc  = NOW(), 
    endereco = :endereco, 
    numero = :numero,
    id_empresa = :id_empresa,
    AdminId = :AdminId");	

    $res->bindValue(":Nome", "$Nome");
    $res->bindValue(":Cargo", "$Cargo");
    $res->bindValue(":DataNascimento", "$DataNascimento");
    $res->bindValue(":Telefone", "$Telefone");
    $res->bindValue(":Email", "$Email");
    $res->bindValue(":Senha", "$Senha");
    $res->bindValue(":endereco", "$endereco");
    $res->bindValue(":numero", "$numero"); 
    $res->bindValue(":id_empresa", "$id_empresa"); 
    $res->bindValue(":AdminId", "$id_administradores"); 

    if($res->execute()){
        $result = json_encode(array('mensagem'=>'Salvo com sucesso!', 'sucesso'=>true));
    } 
    else{
        $result = json_encode(array('mensagem'=>'Erro ao Salvar', 'sucesso'=>false));
    }
} catch (PDOException $e) {
    $result = json_encode(['mensagem'=>'Erro: ' . $e->getMessage(), 'sucesso'=>false]);
}

echo $result;

?>
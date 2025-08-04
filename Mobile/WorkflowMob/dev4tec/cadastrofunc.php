<?php 
require_once("conexao.php");

// Recebe os dados JSON
$tabela = 'Funcionarios';

$postjson = json_decode(file_get_contents('php://input'), true);


$Nome = @$postjson['Nome'];
$Cargo = @$postjson['Cargo'];
$DataNascimento = @$postjson['DataNascimento'];
$Telefone = $postjson['Telefone'];
$Email = $postjson['Email'];
$Senha = $postjson['Senha'];
$endereco = $postjson['endereco'];
$numero = $postjson['numero'];

$res = $pdo->prepare("INSERT INTO $tabela SET Nome = :Nome, Cargo = :Cargo, 
DataNascimento = :DataNascimento, Telefone = :Telefone, Email = :Email, 
Senha = :Senha, data_cadFunc  = NOW(), endereco = :endereco, numero = :numero");	


$res->bindValue(":Nome", "$Nome");
$res->bindValue(":Cargo", "$Cargo");
$res->bindValue(":DataNascimento", "$DataNascimento");
$res->bindValue(":Telefone", "$Telefone");
$res->bindValue(":Email", "$Email");
$res->bindValue(":Senha", "$Senha");
$res->bindValue(":endereco", "$endereco");
$res->bindValue(":numero", "$numero");

$res->execute();

$result = json_encode(array('mensagem'=>'Salvo com sucesso!', 'sucesso'=>true));

echo $result;

?>
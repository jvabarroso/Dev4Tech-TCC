<?php 

header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Credentials: true');
header('Access-Control-Allow-Methods: POST, GET, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Authorization, X-Requested-With'); 
header('Content-Type: application/json; charset=utf-8');  

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    // Responde OK para pré-flight e sai
    http_response_code(200);
    exit();
}

date_default_timezone_set('America/Sao_Paulo');


$usuario = 'root';
$senha = '';
$host = 'localhost';
$banco = 'Dev4Tech';
$SERVER_IP = '10.239.0.127';

try {
	$pdo = new PDO("mysql:host=$host;dbname=$banco;charset=utf8mb4", $usuario, $senha, [
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8mb4"
    ]);

	$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	
} catch (Exception $e) {
	echo 'Erro ao conectar com o banco!!' .$e;
}

?>
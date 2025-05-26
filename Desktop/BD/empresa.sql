create database empresa;
use empresa;

create table dadosEmpresa(
nomeEmpresa varchar(255) not null,
CNPJ int(18) primary key,
setorEmpresarial varchar(255) not null,
logradouro varchar(255) not null,
numResidencia int not null,
bairro varchar(255) not null,
complemento varchar(255) not null
);

CREATE TABLE Administradores (
    AdminId INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    CPF CHAR(11) NOT NULL UNIQUE,
    DataNascimento DATE NOT NULL,
    Telefone VARCHAR(20),
    Email VARCHAR(100) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL);
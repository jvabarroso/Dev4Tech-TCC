create database empresa;
use empresa;

<<<<<<< HEAD
CREATE TABLE dadosempresa (
    codigoID INT AUTO_INCREMENT PRIMARY KEY,
    nomeEmpresa VARCHAR(255) NOT NULL,
    CNPJ CHAR(14) NOT NULL UNIQUE,
    setorEmpresarial VARCHAR(255) NOT NULL,
    logradouro VARCHAR(255) NOT NULL,
    numResidencia INT NOT NULL,
    bairro VARCHAR(255) NOT NULL,
    complemento VARCHAR(255) NOT NULL
=======
create table dadosEmpresa(
nomeEmpresa varchar(255) not null,
CNPJ int(18) primary key,
setorEmpresarial varchar(255) not null,
logradouro varchar(255) not null,
numResidencia int not null,
bairro varchar(255) not null,
complemento varchar(255) not null
>>>>>>> d7e4b7cac9d95fca2c7419bcad2738b99344b662
);

CREATE TABLE Administradores (
    AdminId INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    CPF CHAR(11) NOT NULL UNIQUE,
    DataNascimento DATE NOT NULL,
    Telefone VARCHAR(20),
    Email VARCHAR(100) NOT NULL UNIQUE,
<<<<<<< HEAD
    Senha VARCHAR(255) NOT NULL
);

	
	DROP DATABASE empresa;
=======
    Senha VARCHAR(255) NOT NULL);
>>>>>>> d7e4b7cac9d95fca2c7419bcad2738b99344b662

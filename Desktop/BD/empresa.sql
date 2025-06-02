create database empresa;
use empresa;


CREATE TABLE dadosempresa (
    codigoID INT AUTO_INCREMENT PRIMARY KEY,
    nomeEmpresa VARCHAR(255) NOT NULL,
    CNPJ CHAR(14) NOT NULL UNIQUE,
    setorEmpresarial VARCHAR(255) NOT NULL,
    logradouro VARCHAR(255) NOT NULL,
    numResidencia INT NOT NULL,
    bairro VARCHAR(255) NOT NULL,
    complemento VARCHAR(255) NOT NULL);

CREATE TABLE Administradores (
    AdminId INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    CPF CHAR(11) NOT NULL UNIQUE,
    DataNascimento DATE NOT NULL,
    Telefone VARCHAR(20),
    Email VARCHAR(100) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL
);

create table funcionario (
    FuncionarioId int auto_increment primary key,
    Nome VARCHAR(100),
    Cargo VARCHAR(255),
    CPF CHAR(11) UNIQUE,
    DataNascimento DATE,
    Telefone VARCHAR(20),
    Email VARCHAR(255)  UNIQUE,
    Senha VARCHAR(255) 
);

create table equipes(
equipeId int auto_increment primary key,
nome varchar(255),
Email varchar(255),
foreign key (Email) references Funcionario(Email),
img varchar(255)
);

create table categorias(
nome varchar(255),
categoriaId int auto_increment primary key
);

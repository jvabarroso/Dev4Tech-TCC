-- Criação do banco de dados
CREATE DATABASE Dev4Tech;
USE Dev4Tech;

-- Tabela de Empresas
CREATE TABLE Empresas (
    id_empresa INT PRIMARY KEY auto_increment,
    nome_empresa VARCHAR(100) NOT NULL,
    cnpj VARCHAR(14) UNIQUE NOT NULL,
    logradouro varchar(255),
    email VARCHAR(100) NOT NULL,
    telefone VARCHAR(15),
    numResidencia VARCHAR(200),
    bairro varchar(255),
    complemento varchar(255)
);
-- Tabela de Administradores
CREATE TABLE Administradores (
    AdminId INT PRIMARY KEY auto_increment,
    Nome VARCHAR(100),
    Cargo VARCHAR(50),
    CPF CHAR(11) UNIQUE,
    DataNascimento DATE,
    Telefone VARCHAR(20),
    Email VARCHAR(100) unique,
    Senha VARCHAR(255)
);


-- Tabela de Funcionários
CREATE TABLE Funcionarios (
    FuncionarioId INT PRIMARY KEY auto_increment,
    Nome VARCHAR(100),
    Cargo VARCHAR(255),
    CPF CHAR(11) UNIQUE,
    DataNascimento DATE,
    Telefone VARCHAR(20),
    Email VARCHAR(255) UNIQUE,
    Senha VARCHAR(255)
);

CREATE TABLE MensagensChat (
    id_mensagem INT PRIMARY KEY auto_increment,
    texto varchar(255) NOT NULL,
    data_envio DATETIME
);

-- Tabela Categorias
CREATE TABLE Categorias (
    id_categoria INT AUTO_INCREMENT PRIMARY KEY,
    nome_categoria VARCHAR(255) NOT NULL UNIQUE
);

-- Tabela Equipes
CREATE TABLE Equipes (
    id_equipe INT AUTO_INCREMENT PRIMARY KEY,
    nome_equipe VARCHAR(255) NOT NULL,
    id_categoria INT NOT NULL,
    FOREIGN KEY (id_categoria) REFERENCES Categorias(id_categoria) ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE Equipes_Membros (
    id_equipe INT NOT NULL,
    FuncionarioId INT NOT NULL,
    responsavel BOOLEAN DEFAULT FALSE,
    PRIMARY KEY (id_equipe, FuncionarioId),
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (FuncionarioId) REFERENCES Funcionarios(FuncionarioId) ON DELETE CASCADE ON UPDATE CASCADE
);
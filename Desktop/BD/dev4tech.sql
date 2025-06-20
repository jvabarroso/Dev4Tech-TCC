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
    complemento varchar(255),
    data_cadEm DATETIME
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
    Senha VARCHAR(255),
    data_cadAdmin DATETIME
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
    Senha VARCHAR(255),
    data_cadFunc DATETIME
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

CREATE TABLE Tarefas (
    id_tarefa INT AUTO_INCREMENT PRIMARY KEY,
    nomeTarefa VARCHAR(255) NOT NULL,
    instrucoes VARCHAR(255) NOT NULL,
    id_equipe INT NOT NULL,
    data_entrega DATE NOT NULL,
    nome_arquivo VARCHAR(255),
    arquivo_blob LONGBLOB,
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe) ON DELETE CASCADE ON UPDATE CASCADE,
    nomeTarefa VARCHAR(255) NOT NULL
);

CREATE TABLE EntregasTarefa (
    id_entrega INT AUTO_INCREMENT PRIMARY KEY,
    id_tarefa INT NOT NULL,
    id_equipe INT NOT NULL,
    descricao TEXT,
    nome_arquivo VARCHAR(255),
    arquivo_blob LONGBLOB,
    data_entrega DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_tarefa) REFERENCES Tarefas(id_tarefa) ON DELETE CASCADE,
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe) ON DELETE CASCADE
);

CREATE TABLE RelatoProblema (
idProblema INT PRIMARY KEY AUTO_INCREMENT,
descrição TEXT NOT NULL
);

-- Tabela de Integrantes das Equipes
CREATE TABLE IntegrantesEquipe (
    id_integrante INT PRIMARY KEY auto_increment,
    id_equipe INT,
    id_funcionario INT,
    data_entrada DATETIME,
    cargo_equipe VARCHAR(50),
    status BIT DEFAULT 1,
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe),
    FOREIGN KEY (id_funcionario) REFERENCES Funcionarios(FuncionarioId)
);

-- Tabela de Tarefas
CREATE TABLE Tarefas (
    id_tarefa INT PRIMARY KEY auto_increment,
    id_equipe INT,
    id_criador INT,
    titulo VARCHAR(100) NOT NULL,
    descricao TEXT,
    data_criacao DATETIME,
    data_limite DATETIME,
    data_conclusao DATETIME,
    prioridade VARCHAR(10) DEFAULT 'media',
    status VARCHAR(20) DEFAULT 'pendente',
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe),
    FOREIGN KEY (id_criador) REFERENCES Administradores(AdminId)
);

-- Tabela de Atribuições de Tarefas
CREATE TABLE AtribuicoesTarefa (
    id_atribuicao INT PRIMARY KEY auto_increment,
    id_tarefa INT,
    id_funcionario INT,
    data_atribuicao DATETIME,
    status VARCHAR(20) DEFAULT 'pendente',
    FOREIGN KEY (id_tarefa) REFERENCES Tarefas(id_tarefa),
    FOREIGN KEY (id_funcionario) REFERENCES Funcionarios(FuncionarioId)
);

-- Tabela de Comentários nas Tarefas
CREATE TABLE ComentariosTarefa (
    id_comentario INT PRIMARY KEY auto_increment,
    id_tarefa INT,
    id_usuario INT,
    comentario TEXT NOT NULL,
    data_comentario DATETIME,
    FOREIGN KEY (id_tarefa) REFERENCES Tarefas(id_tarefa),
    FOREIGN KEY (id_usuario) REFERENCES Funcionarios(FuncionarioId)
);

-- Tabela de Ranking de Equipes
CREATE TABLE RankingEquipes (
    id_ranking INT PRIMARY KEY auto_increment,
    id_equipe INT,
    pontuacao INT DEFAULT 0,
    data_atualizacao DATETIME,
    periodo VARCHAR(20),
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe)
);

-- Tabela de Relatórios de Problemas
CREATE TABLE RelatoriosProblema (
    id_relatorio INT PRIMARY KEY auto_increment,
    id_usuario INT,
    id_tarefa INT,
    titulo VARCHAR(100) NOT NULL,
    descricao TEXT NOT NULL,
    data_relatorio DATETIME,
    status VARCHAR(20) DEFAULT 'aberto',
    FOREIGN KEY (id_usuario) REFERENCES Funcionarios(FuncionarioId),
    FOREIGN KEY (id_tarefa) REFERENCES Tarefas(id_tarefa)
);

-- Tabela de Chat


-- Tabela de Configurações
CREATE TABLE Configuracoes (
    id_configuracao INT PRIMARY KEY auto_increment,
    id_empresa INT,
    chave VARCHAR(50) NOT NULL,
    valor TEXT,
    data_atualizacao DATETIME,
    FOREIGN KEY (id_empresa) REFERENCES Empresas(id_empresa)
);

-- Índices
CREATE INDEX idx_administradores_email ON Administradores(Email);
CREATE INDEX idx_funcionarios_email ON Funcionarios(Email);
CREATE INDEX idx_tarefas_equipe ON Tarefas(id_equipe);
CREATE INDEX idx_tarefas_status ON Tarefas(status);
CREATE INDEX idx_ranking_periodo ON RankingEquipes(periodo);
CREATE INDEX idx_mensagens_equipe ON MensagensChat(id_equipe);
>>>>>>> 84c1efa37aa2833043764843579cbc8d64b56f55

-- Criação do banco de dados
CREATE DATABASE Dev4Tech;
USE Dev4Tech;

-- Tabela de Empresas
CREATE TABLE Empresas (
    id_empresa INT PRIMARY KEY auto_increment,
    nome_empresa VARCHAR(100) NOT NULL,
    cnpj VARCHAR(14) UNIQUE NOT NULL,
    email VARCHAR(100) NOT NULL,
    telefone VARCHAR(15),
    endereco VARCHAR(200),
    data_cadastro DATETIME,
    status BIT DEFAULT 1
);

-- Tabela de Administradores
CREATE TABLE Administradores (
    AdminId INT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    CPF CHAR(11) NOT NULL UNIQUE,
    DataNascimento DATE NOT NULL,
    Telefone VARCHAR(20),
    Email VARCHAR(100) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL,
    tipo_usuario VARCHAR(20) NOT NULL DEFAULT 'admin'
);

-- Tabela de Funcionários
CREATE TABLE Funcionarios (
    FuncionarioId INT PRIMARY KEY auto_increment,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(255),
    CPF CHAR(11) UNIQUE,
    DataNascimento DATE,
    Telefone VARCHAR(20),
    Email VARCHAR(255) UNIQUE,
    Senha VARCHAR(255),
    status BIT DEFAULT 1
);

-- Tabela de Tipos de Usuário
CREATE TABLE TiposUsuario (
    id_tipo INT PRIMARY KEY auto_increment,
    nome VARCHAR(50) NOT NULL,
    descricao TEXT
);

-- Tabela de Permissões
CREATE TABLE Permissoes (
    id_permissao INT PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    descricao TEXT
);

-- Tabela de Permissões por Tipo de Usuário
CREATE TABLE PermissoesTipoUsuario (
    id_tipo INT,
    id_permissao INT,
    PRIMARY KEY (id_tipo, id_permissao),
    FOREIGN KEY (id_tipo) REFERENCES TiposUsuario(id_tipo),
    FOREIGN KEY (id_permissao) REFERENCES Permissoes(id_permissao)
);

-- Tabela de Usuários
CREATE TABLE Usuarios (
    id_usuario INT PRIMARY KEY auto_increment,
    id_empresa INT,
    id_tipo INT,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    senha VARCHAR(255) NOT NULL,
    cargo VARCHAR(50),
    data_cadastro DATETIME,
    ultimo_acesso DATETIME,
    status BIT DEFAULT 1,
    FOREIGN KEY (id_empresa) REFERENCES Empresas(id_empresa),
    FOREIGN KEY (id_tipo) REFERENCES TiposUsuario(id_tipo)
);

-- Tabela de Equipes
CREATE TABLE Equipes (
    id_equipe INT PRIMARY KEY auto_increment,
    id_empresa INT,
    nome_equipe VARCHAR(100) NOT NULL,
    descricao TEXT,
    data_criacao DATETIME,
    status BIT DEFAULT 1,
    FOREIGN KEY (id_empresa) REFERENCES Empresas(id_empresa)
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
CREATE TABLE MensagensChat (
    id_mensagem INT PRIMARY KEY auto_increment,
    id_equipe INT,
    id_usuario INT,
    mensagem TEXT NOT NULL,
    data_envio DATETIME,
    FOREIGN KEY (id_equipe) REFERENCES Equipes(id_equipe),
    FOREIGN KEY (id_usuario) REFERENCES Funcionarios(FuncionarioId)
);

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

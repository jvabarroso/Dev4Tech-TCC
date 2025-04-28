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
create table tbReceita(
	receitaId    integer       not null,
	receitaDesc  varchar(80)   not null,
	receitaValor decimal(10,2) not null,
	receitaData  date          not null,
	
	primary key	( receitaId )
);
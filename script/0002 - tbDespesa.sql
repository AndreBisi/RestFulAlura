create table tbDespesa(
	
	despesaId    integer       not null,
	despesaDesc  varchar(80)   not null,
	despesaValor decimal(10,2) not null,
	despesaData  date          not null,
	
	primary key (despesaId)

);
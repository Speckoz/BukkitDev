create database if not exists BukkitDev;
drop database if exists BukkitDev;
use BukkitDev;

##########################
######### testes #########
##########################
#====================================================================================

show tables;

select * from PluginList;# where NomePlugin like '%eco%';
select * from LicencaList;
select count(LicencaList) from LicencaList;

truncate table PluginList;
truncate table LicencaList;

delete from PluginList where ID = 999991; 
select NomePlugin from PluginList where ID = 999991;

select * from LicencaList where (LicencaKey = @a || ClienteID = 99687671) && DataCriacao = '2019-07-25';

insert into PluginList values (999999, "RC_Economy", "Logikoz", "1.0.0", "Gratuito", "0", "Plugin de economia para seu servidor", false);
insert into LicencaList values (99999999, 312909, "xxxxx-xxxxx-xxxxx-xxxxx-xxxxx", false, "2019-06-18", "06:00:00", false);

#=====================================================================================

drop table if exists PluginList;
create table if not exists PluginList(
ID 								MediumInt not null primary key, 
NomePlugin 						tinytext not null,
AutorPlugin 					tinytext not null,
VersaoPlugin 					char(5) not null,
TipoPlugin 						char(10) not null,
PrecoPlugin 					char(5),
DescricaoPlugin 				text(1024) not null,
ImagemPadraoPersonalizada 		boolean not null);

drop table if exists LicencaList;
create table if not exists licencaList(
ClienteID 						int unsigned not null,
#cliente_nome 					varchar(30) not null,
PluginID 						MediumInt not null,
LicencaKey 						tinytext not null,
LicencaGlobal 					boolean not null,
DataCriacao 					date not null,
HorarioCriacao 					time not null,
LicencaSuspensa 				bool,
IPPermitido						tinytext not null);

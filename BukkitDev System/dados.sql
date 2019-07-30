create database if not exists bukkitdev;
drop database if exists bukkitdev;
use bukkitdev;

##########################
######### testes #########
##########################
#====================================================================================

show tables;

select * from pluginlist;# where nome_plugin like '%eco%';
select * from licencalist;
select count(licencalist) from licencalist;

truncate table pluginlist;
truncate table licencalist;

delete from pluginlist where id = 999991; 
select nome_plugin from pluginlist where id = 999991;

select * from licencalist where (licenca_key = @a || cliente_id = 99687671) && data_criacao = '2019-07-25';

insert into pluginlist values (999999, "RC_Economy", "Logikoz", "1.0.0", "Gratuito", "0", "Plugin de economia para seu servidor", false);
insert into licencalist values (99999999, 312909, "xxxxx-xxxxx-xxxxx-xxxxx-xxxxx", false, "2019-06-18", "06:00:00", false);

#=====================================================================================

drop table if exists pluginlist;
create table if not exists pluginlist(
id MediumInt not null primary key,
nome_plugin tinytext not null,
autor_plugin tinytext not null,
versao_plugin char(5) not null,
tipo_plugin char(10) not null,
preco_plugin char(5),
descricao_plugin text(1024) not null,
imagem_padrao_personalizada boolean not null);

drop table if exists licencalist;
create table if not exists licencaList(
cliente_id int unsigned not null,
#cliente_nome varchar(30) not null,
plugin_id MediumInt not null,
licenca_key tinytext not null,
licenca_global boolean not null,
data_criacao date not null,
horario_criacao time not null,
licenca_suspenso bool);
create database if not exists bukkitdev;
drop database if exists bukkitdev;
use bukkitdev;

##########################
######### testes #########
##########################
#====================================================================================

show tables;

select * from pluginlist;# where nome_plugin like '%eco%';
select licence_key from licencelist;
select count(licence_key) from licencelist;

truncate table pluginlist;
truncate table licencelist;

delete from pluginlist where id = 999991; 
select nome_plugin from pluginlist where id = 999991;

update licencelist set plugin_suspenso = true where licence_key = "fkffb-jafsi-e952v-r1qtc-vb0xt";

insert into pluginlist values (999999, "RC_Economy", "Logikoz", "1.0.0", "Gratuito", "0", "Plugin de economia para seu servidor", true);
insert into licencelist values (99999999, 312909, "xxxxx-xxxxx-xxxxx-xxxxx-xxxxx", false, "2019-06-18", "06:00:00", false);

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

drop table if exists licencelist;
create table if not exists licenceList(
cliente_id int unsigned not null,
#cliente_nome varchar(30) not null,
plugin_id MediumInt not null,
licence_key tinytext not null,
licence_global boolean not null,
data_criacao date not null,
horario_criacao time not null,
plugin_suspenso bool);
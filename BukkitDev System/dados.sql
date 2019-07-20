create database bukkitdev;
use bukkitdev;

##########################
######### testes #########
##########################
#====================================================================================

show tables;

select * from pluginlist;# where id like '%%';
select * from licencelist;

truncate table pluginlist;
truncate table licencelist;

insert into pluginlist values (666666, "RC_Economy", "Logikoz", "1.0", "Gratuito", "0", "Plugin de economia para seu servidor", "1", "http://localhost/bukkitdev/images/666666.png");
insert into licencelist values (76547845, 666666, "gfds5-gjvc4-hgfl6-cczx5-kkf4a", 0);

#=====================================================================================

drop table pluginlist;
create table if not exists pluginlist(
id int unsigned not null primary key,
nome_plugin varchar(50) not null,
autor_plugin varchar(50) not null,
versao_plugin varchar(5) not null,
tipo_plugin varchar(10) not null,
preco_plugin varchar(5),
descricao_plugin varchar(300) not null,
imagem_padrao_personalizada boolean not null);

drop table licencelist;
create table if not exists licenceList(
cliente_id int(8) not null,
#cliente_nome varchar(30) not null,
plugin_id int(8),
licence_key tinytext not null,
licence_global boolean not null,
data_criacao varchar(30) not null);
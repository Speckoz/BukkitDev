using BukkitDev_System.Principal;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.MySQL
{
	internal class CriarTabela
	{
		private const string CmdText =
			@"create table if not exists pluginlist(id int unsigned not null primary key,nome_plugin varchar(50) not null,autor_plugin varchar(50) not null,versao_plugin varchar(5) not null,tipo_plugin varchar(10) not null,preco_plugin varchar(5),descricao_plugin varchar(300) not null,imagem_padrao_personalizada boolean not null); 
			create table if not exists licenceList(cliente_id int(8) primary key,plugin_id int(8) not null,licence_key tinytext,licence_global boolean);";

		public async Task CriarAsync()
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand createTable = new MySqlCommand(CmdText, con))
					{
						_ = await createTable.ExecuteNonQueryAsync();
					}
				}
				catch (Exception)
				{
					TelaInicial.EnviarMensagemDialogHostAsync("Nao consegui gerar a tabela no mysql\nSe for sua primeira inicializaçao, por favor, insira as credenciais na configuraçao\ne selecione o tipo de banco 'Local/Externo'\n\nDepois reinicie o software!");
				}
			}
		}
	}
}

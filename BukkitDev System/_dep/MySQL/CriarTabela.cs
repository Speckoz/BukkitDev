using BukkitDev_System.Principal;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.MySQL
{
	internal class CriarTabela
	{
		private const string query = @"create table if not exists pluginlist(id MediumInt not null primary key,nome_plugin tinytext not null,autor_plugin tinytext not null,versao_plugin char(5) not null,tipo_plugin char(10) not null,preco_plugin char(5),descricao_plugin text(1024) not null,imagem_padrao_personalizada boolean not null);
			create table if not exists licenceList(cliente_id int unsigned not null,plugin_id MediumInt,licence_key tinytext not null,licence_global boolean not null,data_criacao date not null,horario_criacao time not null,plugin_suspenso bool);";
		/// <summary>
		/// Cria as tabelas no banco de dados caso elas nao existam. (PluginList e LicenceList).
		/// </summary>
		/// <returns>Retorna a conclusao da tarefa</returns>
		public async Task CriarAsync()
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand createTable = new MySqlCommand(query, con))
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

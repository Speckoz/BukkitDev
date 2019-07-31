using Logikoz.BukkitDevSystem.Principal;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Logikoz.BukkitDevSystem._dep.MySQL
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="CriarTabela"/>.
	/// </summary>
	internal class CriarTabela
	{
		private const string query = @"create table if not exists PluginList(ID MediumInt not null primary key,NomePlugin tinytext not null,AutorPlugin tinytext not null,VersaoPlugin char(5) not null,TipoPlugin char(10) not null,preco_plugin char(5),DescricaoPlugin text(1024) not null,ImagemPadraoPersonalizada boolean not null);
			create table if not exists LicencaList(ClienteID int unsigned not null,PluginID MediumInt,LicencaKey tinytext not null,LicencaGlobal boolean not null,DataCriacao date not null,HorarioCriacao time not null,LicencaSuspensa bool);";
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

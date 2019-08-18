using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace Logikoz.BukkitDev._dep.SQLite
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="CriarBanco"/>.
	/// </summary>
	internal class CriarBanco
	{
		#region queries
		private const string TabelasExistem = "select count(*) from sqlite_master where type='table' and (name = 'mysql' or name = 'ftp')";
		private const string CriarTabelaInserirDados = @"create table ftp(host varchar(30), usuario varchar(100), senha varchar(100), porta varchar(5), tipo varchar(10));
			insert into ftp(host, usuario, senha, porta, tipo) values ('localhost', 'root', 'root', '21', 'Externo');
			insert into ftp(host, usuario, senha, porta, tipo) values ('localhost', 'root', 'root', '21', 'Local');

			create table mysql(servidor varchar(30), usuario varchar(100), senha varchar(100), porta varchar(5), banco varchar(30), tipo varchar(10));
			insert into mysql(servidor, usuario, senha, porta, banco, tipo) values ('localhost', 'root', 'root', '3306', 'default', 'Externo'); 
			insert into mysql(servidor, usuario, senha, porta, banco, tipo) values ('localhost', 'root', 'root', '3306', 'default', 'Local');";
		#endregion
		/// <summary>
		/// Cria o novo arquivo SQLite que guardará as conexoes com mysql e ftp.
		/// </summary>
		/// <param name="nome">Nome que será dado ao arquivo, por padrao é uma propriedade <see cref="PegarInfos.NomeArquivoSQLite"/>.</param>
		public void CriarArquivo(string nome)
		{
			string path = "./" + nome;

			if (!File.Exists(path))
			{
				SQLiteConnection.CreateFile(nome);
			}
		}
		/// <summary>
		/// Verifica se as tabelas (ftp e mysql) ja estao criadas dentro do arquivo sqlite.
		/// </summary>
		/// <param name="nome">Nome do arquivo SQLite, por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoSQLite"/>.</param>
		/// <returns>Retorna a tarefa com um bool informando se a operaçao foi um sucesso.</returns>
		public async Task<bool> TabelaExisteAsync(string nome)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source = {nome}; Version = 3;"))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand ver = new SQLiteCommand(TabelasExistem, con))
					{
						byte valid = byte.Parse((await ver.ExecuteScalarAsync()).ToString());

						return valid > 0;
					}
				}
				catch (SQLiteException e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return false;
				}
			}
		}
		/// <summary>
		/// Cria as tabelas dentro do arquivo SQLite.
		/// </summary>
		/// <param name="nome">Nome do arquivo SQLite, por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoSQLite"/>.</param>
		/// <returns>Retorna a tarefa com um bool informando se a operaçao foi um sucesso.</returns>
		public async Task<bool> CriarTabelaAsync(string nome)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source = {nome}; Version=3;"))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand criarTabela = new SQLiteCommand(CriarTabelaInserirDados, con))
					{
						_ = await criarTabela.ExecuteNonQueryAsync();

						return true;
					}
				}
				catch (SQLiteException e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return false;
				}
			}
		}
	}
}

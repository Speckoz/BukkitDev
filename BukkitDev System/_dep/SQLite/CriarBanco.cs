using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.SQLite
{
	internal class CriarBanco : PegarConexaoMySQL_FTP
	{
		private const string TabelasExistem = "select count(*) from sqlite_master where type='table' and (name = 'mysql' or name = 'ftp')";
		//query ftp
		private const string CriarTabelaInserirDados = @"create table ftp(host varchar(30), usuario varchar(100), senha varchar(100), porta varchar(5), tipo varchar(10));
			insert into ftp(host, usuario, senha, porta, tipo) values ('localhost', 'root', 'root', '21', 'Externo');
			insert into ftp(host, usuario, senha, porta, tipo) values ('localhost', 'root', 'root', '21', 'Local');

			create table mysql(servidor varchar(30), usuario varchar(100), senha varchar(100), porta varchar(5), banco varchar(30), tipo varchar(10));
			insert into mysql(servidor, usuario, senha, porta, banco, tipo) values ('localhost', 'root', 'root', '3306', 'default', 'Externo'); 
			insert into mysql(servidor, usuario, senha, porta, banco, tipo) values ('localhost', 'root', 'root', '3306', 'default', 'Local');";

		public void CriarArquivo(string nome)
		{
			string path = "./" + nome;

			if (!File.Exists(path))
			{
				SQLiteConnection.CreateFile(nome);
			}
		}

		//metodo nao utilizado..
		public async Task<bool> TabelaExisteAsync(string nome/*, string tipo1, string tipo2*/)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source = {nome}; Version = 3;"))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand ver = new SQLiteCommand(TabelasExistem, con))
					{
						//_ = ver.Parameters.Add(new SQLiteParameter("@a", tipo1));
						//_ = ver.Parameters.Add(new SQLiteParameter("@b", tipo2));

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

		public async Task CriarTabelaAsync(string nome)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source = {nome}; Version=3;"))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand criarTabela = new SQLiteCommand(CriarTabelaInserirDados, con))
					{
						_ = await criarTabela.ExecuteNonQueryAsync();
					}
				}
				catch (SQLiteException e)
				{
					MetodosConstantes.MostrarExceptions(e);
				}
			}
		}
	}
}

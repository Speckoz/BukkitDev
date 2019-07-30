using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Logikoz.BukkitDevSystem._dep.SQLite
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="PegarConexaoMySQL_FTP"/>.
	/// </summary>
	internal class PegarConexaoMySQL_FTP
	{
		/// <summary>
		/// Pegar a conexao do FTP ou MySQL de dentro do Arquivo SQLite.
		/// </summary>
		/// <param name="nome">Nome do arquivo SQLite, Por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoSQLite"/></param>
		/// <param name="tipo">Tipo de conexao (Local ou Externo), por padrao é uma propriedade em <see cref="PegarInfos.ConfigFTP"/> ou <see cref="PegarInfos.ConfigMySQL"/>.</param>
		/// <param name="tabela">Nome da tabela dentro do arquivo SQLite (FTP ou MySQL). OBS: nao é binary!</param>
		/// <returns>Retorna uma Lista com a conexao do tipo requisitado!</returns>
		public async Task<List<string>> PegarAsync(string nome, string tipo, string tabela)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source={nome};Version=3;"))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand pegar = new SQLiteCommand($"select * from {tabela} where tipo = @a", con))
					{
						_ = pegar.Parameters.Add(new SQLiteParameter("@a", tipo));

						using (SQLiteDataReader ler = (SQLiteDataReader)await pegar.ExecuteReaderAsync())
						{
							List<string> d = new List<string>();
							if (await ler.ReadAsync())
							{
								for (byte i = 0; i < ler.FieldCount; i++)
								{
									d.Add(ler.GetString(i));
								}
							}
							return d;
						}
					}
				}
				catch (SQLiteException e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return null;
				}
			}
		}
	}
}

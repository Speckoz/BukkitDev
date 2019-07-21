using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.SQLite
{
	internal class PegarConexaoMySQL_FTP
	{
		/// <summary>
		/// Pegar a conexao do FTP ou MySQL de dentro do Arquivo SQLite.
		/// </summary>
		/// <param name="nome">Nome do arquivo SQLite, Por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoXML"/></param>
		/// <param name="tipo">Tipo de conexao (Local ou Externo), por padrao é uma propriedade em <see cref="PegarInfos.ConfigFTP"/> ou <see cref="PegarInfos.ConfigMySQL"/>.</param>
		/// <param name="tabela">Nome da tabela dentro do arquivo SQLite (FTP ou MySQL). OBS: nao é binary!</param>
		/// <returns>Retorna uma Lista com a conexao do tipo requisitado!</returns>
		public async Task<List<string>> PegarAsync(string nome, string tipo, string tabela)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source={nome};Version=3;"))
			{
				try
				{
					return await AbrirConexaoAndGetValues(tipo, con, tabela);
				}
				catch (SQLiteException e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return null;
				}
			}
		}

		private static async Task<List<string>> AbrirConexaoAndGetValues(string tipo, SQLiteConnection con, string tabela)
		{
			await con.OpenAsync();

			using (SQLiteCommand pegar = new SQLiteCommand($"select * from {tabela} where tipo = @a", con))
			{
				return await ParametroAndExecutarCmdAsync(tipo, pegar);
			}
		}

		private static async Task<List<string>> ParametroAndExecutarCmdAsync(string tipo, SQLiteCommand pegar)
		{
			_ = pegar.Parameters.Add(new SQLiteParameter("@a", tipo));

			using (SQLiteDataReader ler = (SQLiteDataReader)await pegar.ExecuteReaderAsync())
			{
				List<string> d = new List<string>();
				return await RetornarVetorConexaoAsync(ler, d);
			}
		}

		private static async Task<List<string>> RetornarVetorConexaoAsync(SQLiteDataReader ler, List<string> d)
		{
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

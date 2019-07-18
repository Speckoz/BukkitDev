using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.SQLite
{
	internal class PegarConexaoMySQL_FTP
	{
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

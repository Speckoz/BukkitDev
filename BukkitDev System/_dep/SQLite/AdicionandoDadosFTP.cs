using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.SQLite
{
	internal class AdicionandoDadosFTP
	{
		private const string CommandText = "update ftp set host = @a, usuario = @b, senha = @c, porta = @d where tipo = @e";
		private const string V = "data source = ";
		private const string V1 = "; Version = 3;";

		public async Task<bool> AtualizarAsync(string nome, string[] dados, string tipo)
		{
			using (SQLiteConnection con = new SQLiteConnection(V + nome + V1))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand add = new SQLiteCommand(CommandText, con))
					{
						_ = add.Parameters.Add(new SQLiteParameter("@a", dados[0]));
						_ = add.Parameters.Add(new SQLiteParameter("@b", dados[1]));
						_ = add.Parameters.Add(new SQLiteParameter("@c", dados[2]));
						_ = add.Parameters.Add(new SQLiteParameter("@d", dados[3]));
						_ = add.Parameters.Add(new SQLiteParameter("@e", tipo));

						_ = await add.ExecuteNonQueryAsync();
						return true;
					}
				}
				catch (Exception e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return false;
				}
			}
		}
	}
}

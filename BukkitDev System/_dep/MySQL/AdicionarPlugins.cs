using BukkitDev_System._dep.SQLite;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.MySQL
{
	internal class AdicionarPlugins : CriarBanco
	{
		public async Task<bool> AdicionarDadosAsync(uint id, List<string> dados)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					return await AbrirConexaoAndSetParameters(id, dados, con);
				}
				catch (MySqlException e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return false;
				}
			}
		}

		private static async Task<bool> AbrirConexaoAndSetParameters(uint id, List<string> dados, MySqlConnection con)
		{
			await con.OpenAsync();

			using (MySqlCommand add = new MySqlCommand("insert into pluginlist values (@a, @b, @c, @d, @f, @g, @h, @i)", con))
			{
				return await RetornarBoolResult(id, dados, add);
			}
		}

		private static async Task<bool> RetornarBoolResult(uint id, List<string> dados, MySqlCommand add)
		{
			Parametros(id, dados, add);

			_ = await add.ExecuteNonQueryAsync();
			return true;
		}

		private static void Parametros(uint id, List<string> dados, MySqlCommand add)
		{
			_ = add.Parameters.Add(new MySqlParameter("@a", id));
			_ = add.Parameters.Add(new MySqlParameter("@b", dados[0]));
			_ = add.Parameters.Add(new MySqlParameter("@c", dados[1]));
			_ = add.Parameters.Add(new MySqlParameter("@d", dados[2]));
			_ = add.Parameters.Add(new MySqlParameter("@f", dados[3]));
			_ = add.Parameters.Add(new MySqlParameter("@g", dados[4]));
			_ = add.Parameters.Add(new MySqlParameter("@h", dados[5]));
			_ = add.Parameters.Add(new MySqlParameter("@i", dados[6]));
		}
	}
}

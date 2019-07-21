using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.MySQL
{
	internal class AdicionarLicenca
	{
		private const string CmdText = "insert into licencelist values (@a, @b, @c, @d, @e, @f);";

		public async Task<bool> AdicionarAsync(uint user, uint pluginCod, string key, bool global)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					return await AbrirConexaoAndSetParameters(user, pluginCod, key, global, con);
				}
				catch (MySqlException erro)
				{
					MetodosConstantes.MostrarExceptions(erro);
					return false;
				}
			}
		}

		private static async Task<bool> AbrirConexaoAndSetParameters(uint user, uint pluginCod, string key, bool global, MySqlConnection con)
		{
			await con.OpenAsync();

			using (MySqlCommand add = new MySqlCommand(CmdText, con))
			{
				return await RetornarBoolResult(user, pluginCod, key, global, add);
			}
		}

		private static async Task<bool> RetornarBoolResult(uint user, uint pluginCod, string key, bool global, MySqlCommand add)
		{
			Parametros(user, pluginCod, key, global, add);

			_ = await add.ExecuteNonQueryAsync();

			return true;
		}

		private static void Parametros(uint user, uint pluginCod, string key, bool global, MySqlCommand add)
		{
			_ = add.Parameters.Add(new MySqlParameter("@a", user));
			_ = add.Parameters.Add(new MySqlParameter("@b", global ? null : (object)pluginCod));
			_ = add.Parameters.Add(new MySqlParameter("@c", key));
			_ = add.Parameters.Add(new MySqlParameter("@d", global));
			_ = add.Parameters.Add(new MySqlParameter("@e", System.DateTime.Now.ToString("yyyy-MM-dd")));
			_ = add.Parameters.Add(new MySqlParameter("@f", System.DateTime.Now.ToString("HH:mm:ss")));
		}
	}
}

using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BukkitDev.System._dep.MySQL
{
	internal class SuspencaoLicenca
	{
		private const string QueryChecar = "select licenca_suspenso from licencalist where binary licenca_key = @a";
		private const string QuerySuspender = "update licencalist set licenca_suspenso = @b where binary licenca_key = @a; " + QueryChecar;

		public async Task<bool> SuspensoAsync(bool suspender, string lic, bool valor)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					string query = !suspender ? QueryChecar : QuerySuspender;

					using (MySqlCommand ver = new MySqlCommand(query, con))
					{
						_ = ver.Parameters.Add(new MySqlParameter("@a", lic));
						if (suspender)
						{
							_ = ver.Parameters.Add(new MySqlParameter("@b", valor));
						}

						using (MySqlDataReader reader = await ver.ExecuteReaderAsync())
						{
							bool retorno = false;
							if (await reader.ReadAsync())
							{
								retorno = reader.GetBoolean(0);
							}
							return retorno;
						}
					}
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

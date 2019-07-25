using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace BukkitDev.System._dep.MySQL
{
	internal class PegarLicenca
	{
		public async Task<List<string>> PegarAsync(string @ref)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand get = new MySqlCommand("select * from licencalist where licenca_key = @a || cliente_id = @a", con))
					{
						_ = get.Parameters.Add(new MySqlParameter("@a", @ref));

						using (MySqlDataReader dataReader = await get.ExecuteReaderAsync())
						{
							List<string> dados = new List<string>();

							if (await dataReader.ReadAsync())
							{
								for (byte i = 0; i < dataReader.FieldCount; i++)
								{
									dados.Add(dataReader.GetString(i));
								}
							}

							return dados;
						}
					}
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return null;
			}
		}
		public async Task<List<string>> PegarAsync()
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand get = new MySqlCommand("select licenca_key from licencalist", con))
					{
						using(MySqlDataReader reader = await get.ExecuteReaderAsync())
						{
							List<string> dados = new List<string>();
							DataTable data = new DataTable();
							data.Load(reader);
							foreach (DataRow row in data.Rows)
							{
								dados.Add(row["licenca_key"].ToString());
							}
							return dados;
						}
					}
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return null;
			}
		}
	}
}

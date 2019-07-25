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
		public async Task<List<string>> PegarAsync()
		{
			return await PegarVazio();
		}
		public async Task<List<string>> PegarAsync(string lic)
		{
			return await PegarParam(lic, null);
		}
		public async Task<List<string>> PegarAsync(string @ref, string data)
		{
			return await PegarParam(@ref, data);
		}
		private async Task<List<string>> PegarParam(string @ref, string dataPesquisar)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand get = new MySqlCommand($"select * from licencalist where (licenca_key = @a || cliente_id = @a) {(dataPesquisar != null ? "&& data_criacao = @b" : string.Empty)}", con))
					{
						_ = get.Parameters.Add(new MySqlParameter("@a", @ref));
						//
						if (dataPesquisar != null)
						{
							_ = get.Parameters.Add(new MySqlParameter("@b", dataPesquisar));
						}
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
		private async Task<List<string>> PegarVazio()
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand get = new MySqlCommand("select licenca_key from licencalist", con))
					{
						using (MySqlDataReader reader = await get.ExecuteReaderAsync())
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

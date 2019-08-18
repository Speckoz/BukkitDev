using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logikoz.BukkitDev._dep.MySQL
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="LicencaInfo"/>.
	/// </summary>
	internal class LicencaInfo
	{
		#region queries
		private const string QueryChecar = "select LicencaSuspensa from LicencaList where binary LicencaKey = @a";
		private const string QuerySuspender = "update LicencaList set LicencaSuspensa = @b where binary LicencaKey = @a; " + QueryChecar;
		#endregion

		/// <summary>
		/// Adiciona uma nova licença ao Banco de Dados.
		/// </summary>
		/// <param name="user">Codigo do usuario que terá acesso a esse key.</param>
		/// <param name="pluginCod">codigo do plugin que a licença fará referencia.</param>
		/// <param name="key">string contendo a key desejada!</param>
		/// <param name="global">true se a licença pode ser usada para todos os plugins.</param>
		/// <returns>Retorna true se a operaçao foi um sucesso.</returns>
		public async Task<bool> AdicionarAsync(uint user, uint pluginCod, string key, bool global)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand add = new MySqlCommand("insert into LicencaList values (@a, @b, @c, @d, @e, @f, @g)", con))
					{
						_ = add.Parameters.Add(new MySqlParameter("@a", user));
						_ = add.Parameters.Add(new MySqlParameter("@b", global ? 0 : pluginCod));
						_ = add.Parameters.Add(new MySqlParameter("@c", key));
						_ = add.Parameters.Add(new MySqlParameter("@d", global));
						_ = add.Parameters.Add(new MySqlParameter("@e", System.DateTime.Now.ToString("yyyy-MM-dd")));
						_ = add.Parameters.Add(new MySqlParameter("@f", System.DateTime.Now.ToString("HH:mm:ss")));
						_ = add.Parameters.Add(new MySqlParameter("@g", false));

						_ = await add.ExecuteNonQueryAsync();

						return true;
					}
				}
				catch (MySqlException erro)
				{
					MetodosConstantes.MostrarExceptions(erro);
					return false;
				}
			}
		}
		/// <summary>
		/// Pega a key de todas as licenças do banco.
		/// </summary>
		/// <returns>Retorna a tarefa com a lista contendo todas as keys.</returns>
		public async Task<List<string>> PegarAsync()
		{
			return await PegarVazio();
		}
		/// <summary>
		/// Pega informaçoes de uma determinada licença.
		/// </summary>
		/// <param name="ref">key ou codigo do cliente que deseja pegar as informaçoes da(s) licença(s).</param>
		/// <returns>Retorna a tarefa com a lista contendo todas informaçoes da licença.</returns>
		public async Task<List<string>> PegarAsync(string @ref)
		{
			return await PegarParam(@ref, null);
		}
		/// <summary>
		/// Pega informaçoes de uma ou mais licenças em determinada data de criaçao.
		/// </summary>
		/// <param name="ref">key ou codigo de um cliente para ser procurado no banco.</param>
		/// <param name="data">data que deseja procurar no formado (9999-12-31) ou <see cref="Nullable"/> caso nao queira usar esse filtro.</param>
		/// <returns>Retorna a tarefa com a lista contendo todas informaçoes da licença.</returns>
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

					using (MySqlCommand get = new MySqlCommand($"select * from LicencaList where (LicencaKey = @a || ClienteID = @a) {(dataPesquisar != null ? "&& DataCriacao = @b" : string.Empty)}", con))
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

					using (MySqlCommand get = new MySqlCommand("select LicencaKey from LicencaList", con))
					{
						using (MySqlDataReader reader = await get.ExecuteReaderAsync())
						{
							List<string> dados = new List<string>();

							DataTable data = new DataTable();
							data.Load(reader);
							foreach (DataRow row in data.Rows)
							{
								dados.Add(row["LicencaKey"].ToString());
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
		/// <summary>
		/// Remover uma licença do banco de dados.
		/// </summary>
		/// <param name="lic">A key da licença que deseja remover.</param>
		/// <returns>Retorna a tarefa com bool informando se ouve ou nao sucesso na operaçao.</returns>
		public async Task<bool> ApagarAsync(string lic)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand rem = new MySqlCommand("delete from LicencaList where binary LicencaKey = @a;", con))
					{
						_ = rem.Parameters.Add(new MySqlParameter("@a", lic));

						_ = await rem.ExecuteNonQueryAsync();

						return true;
					}
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return false;
			}
		}
		/// <summary>
		/// Da suporte a suspensao da licença no banco.
		/// </summary>
		/// <param name="suspender">true para suspender a licença, false para apenas fazer a checagem no banco.</param>
		/// <param name="lic">A key da licença que deseja suspender.</param>
		/// <param name="valor">true para suspender a licença, false para realoca-la</param>
		/// <returns>Retorna a tarefa com bool informando se ouve ou nao sucesso na operaçao.</returns>
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

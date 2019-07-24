using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.MySQL
{
	class RemoverItem
	{
		/// <summary>
		/// Remover plugin do banco de dados.
		/// </summary>
		/// <param name="pluginID">Codigo do plugin que dejesa apagar.</param>
		/// <returns>Retorna a tarefa contendo um tupla com os retornos da consulta.</returns>
		public async Task<(bool @bool, string nomePlugin, bool img)> ApagarAsync(uint pluginID)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand rem = new MySqlCommand("select nome_plugin, imagem_padrao_personalizada from pluginlist where id = @a; delete from pluginlist where id = @a;", con))
					{
						_ = rem.Parameters.Add(new MySqlParameter("@a", pluginID));

						using (MySqlDataReader dataReader = await rem.ExecuteReaderAsync())
						{
							string nome = null;
							bool img = false;
							if (await dataReader.ReadAsync())
							{
								nome = dataReader.GetString(0);
								img = dataReader.GetBoolean(1);
							}
							return (true, nome, img);
						}
					}
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return (false, null, false);
			}
		}
		/// <summary>
		/// Remover uma licença do banco de dados.
		/// </summary>
		/// <param name="lic">A key da licença que deseja remover.</param>
		/// <returns>Retorna o resultado da tarefa em bool.</returns>
		public async Task<bool> ApagarAsync(string lic)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand rem = new MySqlCommand("delete from licencelist where binary licence_key = @a;", con))
					{
						_ = rem.Parameters.Add(new MySqlParameter("@a", lic));

						return (true);
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

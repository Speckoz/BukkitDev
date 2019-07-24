using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
//como o tipo byte representa apenas valores de 0-255, pode ocorrer de ter mais de 255 plugin na busca kkkk entao basta trocar o tipo da variavel aqui
using @var = System.Byte;

namespace BukkitDev_System._dep.MySQL
{
	internal class PegarPlugins
	{
		//queries
		//referente ao primeiro metodo
		private const string CmdText2 = "select * from pluginlist";
		private const string CmdText3 = "select count(*) from pluginlist";
		//referente ao segundo metodo
		private const string CmdText = "select * from pluginlist where id like @a or nome_plugin like @a";
		private const string CmdText1 = "select count(*) from pluginlist where id like @a or nome_plugin like @a";

		public DataTable DataTable { get; set; }
		public List<string> TooltipInfo { get; set; }

		/// <summary>
		/// Pega informaçoes dos plugins.
		/// </summary>
		/// <param name="dataGrid">se true, adiciona tabela na propriedade <see cref="DataTable"/>. senao, adiciona informaçoes de um determinado plugin na propriedade <see cref="TooltipInfo"/>.</param>
		/// <param name="itemProcurarID">se dataGrid for false, deve informar o codigo do plugin para ser informado. senao, <see cref="Nullable"/></param>
		/// <returns>Retorna o resultado da tarefa em bool.</returns>
		public async Task<bool> InformacoesAsync(bool dataGrid, string itemProcurarID)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand get = new MySqlCommand(CmdText2, con))
					{
						if (dataGrid)
						{
							using (MySqlDataAdapter mySqlData = new MySqlDataAdapter())
							{
								mySqlData.SelectCommand = get;

								DataTable dataTable = new DataTable();
								_ = await mySqlData.FillAsync(dataTable);

								DataTable = dataTable;
								NomeColunas();

								using (MySqlCommand num = new MySqlCommand(CmdText3, con))
								{
									MetodosConstantes.EnviarMenssagem(@var.Parse((await num.ExecuteScalarAsync()).ToString()) + " Plugins encontrados!");
								}

								return true;
							}
						}
						else
						{
							TooltipInfo = new List<string>();
							get.CommandText = $"{CmdText2} where id = @id";
							_ = get.Parameters.Add(new MySqlParameter("@id", itemProcurarID));
							using (MySqlDataReader a = await get.ExecuteReaderAsync())
							{
								if (await a.ReadAsync())
								{
									for (byte i = 1; i <= 4; i++)
									{
										TooltipInfo.Add(a.GetString(i));
									}
								}
								return true;
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

		public async Task<bool> InformacoesAsync(string itemProcurar)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand get = new MySqlCommand(CmdText, con))
					{
						_ = get.Parameters.Add(new MySqlParameter("@a", "%" + itemProcurar + "%"));

						using (MySqlDataAdapter mySqlData = new MySqlDataAdapter())
						{
							mySqlData.SelectCommand = get;

							DataTable dataTable = new DataTable();
							_ = await mySqlData.FillAsync(dataTable);

							DataTable = dataTable;
							NomeColunas();

							using (MySqlCommand num = new MySqlCommand(CmdText1, con))
							{
								_ = num.Parameters.Add(new MySqlParameter("@a", "%" + itemProcurar + "%"));

								MetodosConstantes.EnviarMenssagem(@var.Parse((await num.ExecuteScalarAsync()).ToString()) + " Plugins encontrados!");
							}

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
		}

		private void NomeColunas()
		{
			DataTable.Columns[0].ColumnName = "ID";
			DataTable.Columns[1].ColumnName = "Nome";
			DataTable.Columns[2].ColumnName = "Autor";
			DataTable.Columns[3].ColumnName = "Versao";
			DataTable.Columns[4].ColumnName = "Tipo";
			DataTable.Columns[5].ColumnName = "Preço";
			DataTable.Columns[6].ColumnName = "Descriçao";
			DataTable.Columns[7].ColumnName = "Imagem";
		}
	}
}

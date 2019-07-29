using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
//como o tipo byte representa apenas valores de 0-255, pode ocorrer de ter mais de 255 plugin na busca kkkk entao basta trocar o tipo da variavel aqui
using @var = System.Byte;

namespace Logikoz.BukkitDevSystem._dep.MySQL
{
	internal class PluginInfo
	{
		#region queries
		//referente ao primeiro metodo de Read
		private const string CmdText2 = "select * from pluginlist";
		private const string CmdText3 = "select count(*) from pluginlist";
		//referente ao segundo metodo de Read
		private const string CmdText = "select * from pluginlist where id like @a or nome_plugin like @a";
		private const string CmdText1 = "select count(*) from pluginlist where id like @a or nome_plugin like @a";
		//query de atualizaçao
		private const string Atualizar = "uptade pluginlist set nome_plugin = @a, autor_plugin = @b, versao_plugin = @c, tipo_plugin = @d, preco_plugin = @e, descricao_plugin = @f, imagem_padrao_personalizada = @g where id = @id";
		#endregion
		//guarda a DataTable com informaçoes do plugin.
		public DataTable DataTable { get; set; }
		//usada para mostrar informaçoes nos chips antes de excluir um plugin.
		public List<string> TooltipInfo { get; set; }
		/// <summary>
		/// Adiciona um novo plugin ao banco
		/// </summary>
		/// <param name="id">Codigo do plugin</param>
		/// <param name="dados">Lista contendo informaçoes a serem adicionadas (Nome, Autor, Versao, Tipo, Preço, Descriçao, Imagem(0, 1)) respectivamente.</param>
		/// <returns></returns>
		public async Task<bool> AdicionarDadosAsync(uint id, List<string> dados)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand add = new MySqlCommand("insert into pluginlist values (@a, @b, @c, @d, @f, @g, @h, @i)", con))
					{
						_ = add.Parameters.Add(new MySqlParameter("@a", id));
						_ = add.Parameters.Add(new MySqlParameter("@b", dados[0]));
						_ = add.Parameters.Add(new MySqlParameter("@c", dados[1]));
						_ = add.Parameters.Add(new MySqlParameter("@d", dados[2]));
						_ = add.Parameters.Add(new MySqlParameter("@f", dados[3]));
						_ = add.Parameters.Add(new MySqlParameter("@g", dados[4]));
						_ = add.Parameters.Add(new MySqlParameter("@h", dados[5]));
						_ = add.Parameters.Add(new MySqlParameter("@i", dados[6]));

						_ = await add.ExecuteNonQueryAsync();
						return true;
					}
				}
				catch (MySqlException e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return false;
				}
			}
		}
		/// <summary>
		/// Pega as informaçoes de todos plugins e guarda na propriedade <see cref="DataTable"/>
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
		/// <summary>
		/// Pega informaçoes de um determinado plugin e guarda na propriedade <see cref="DataTable"/>.
		/// </summary>
		/// <param name="itemProcurar">Index (ID ou Nome do plugin) do plugin que deseja as informaçoes.</param>
		/// <param name="messageReturn">true, se deseja informar informar a mensagem no canto da tela mostrando a quantidade de resultados encontrados.</param>
		/// <returns>Retorna o resultado da tarefa em bool.</returns>
		public async Task<bool> InformacoesAsync(string itemProcurar, bool messageReturn)
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

							if (messageReturn)
							{
								using (MySqlCommand num = new MySqlCommand(CmdText1, con))
								{
									_ = num.Parameters.Add(new MySqlParameter("@a", "%" + itemProcurar + "%"));

									MetodosConstantes.EnviarMenssagem(@var.Parse((await num.ExecuteScalarAsync()).ToString()) + " Plugins encontrados!");
								}
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
		//alterando nome das colunas do DataGrid.
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
		/// <summary>
		/// Atualiza informaçoes do plugin.
		/// </summary>
		/// <param name="pluginID">Identificador do plugin que deseja atualizar</param>
		/// <param name="dados"><see cref="string[]"/> contendo informaçoes do plugin (nome, autor, versao, tipo, preço, descriçao e configImagem) respectivamente.</param>
		/// <returns>Retorna a tarefa informando se foi concluido ou nao (true, false).</returns>
		public async Task<bool> AtualizarAsync(string pluginID, string[] dados)
		{
			try
			{
				using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
				{
					await con.OpenAsync();

					using (MySqlCommand att = new MySqlCommand(Atualizar, con))
					{
						_ = att.Parameters.Add(new MySqlParameter("@id", pluginID));
						_ = att.Parameters.Add(new MySqlParameter("@a", dados[0]));
						_ = att.Parameters.Add(new MySqlParameter("@b", dados[0]));
						_ = att.Parameters.Add(new MySqlParameter("@c", dados[0]));
						_ = att.Parameters.Add(new MySqlParameter("@d", dados[0]));
						_ = att.Parameters.Add(new MySqlParameter("@e", dados[0]));
						_ = att.Parameters.Add(new MySqlParameter("@f", dados[0]));
						_ = att.Parameters.Add(new MySqlParameter("@g", dados[0]));

						_ = await att.ExecuteNonQueryAsync();
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
	}
}

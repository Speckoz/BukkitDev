using Logikoz.BukkitDevSystem._dep.SQLite;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logikoz.BukkitDevSystem._dep.MySQL
{
	internal class AdicionarPlugins : CriarBanco
	{
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
	}
}

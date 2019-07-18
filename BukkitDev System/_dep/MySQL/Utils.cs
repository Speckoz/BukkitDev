using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace BukkitDev_System._dep.MySQL
{
	internal class Utils
	{
		/// <summary>
		/// Procurar por um determinado item dentro do banco de dados
		/// </summary>
		/// <param name="itemProcurado">valor do item a ser procurado</param>
		/// <param name="tabela">Nome da tabela no banco na qual será feito a busca.</param>
		/// <param name="coluna">Index para a busca.</param>
		public async Task<bool> VerificarExisteAsync(string itemProcurado, string tabela, string coluna)
		{
			using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
			{
				try
				{
					await con.OpenAsync();

					using (MySqlCommand ver = new MySqlCommand($"select count(*) from {tabela} where {coluna} = @a", con))
					{
						_ = ver.Parameters.Add(new MySqlParameter("@a", itemProcurado));

						byte valid = byte.Parse((await ver.ExecuteScalarAsync()).ToString());

						return valid == 1;
					}
				}
				catch (Exception e)
				{
					MetodosConstantes.MostrarExceptions(e);
					return false;
				}
			}
		}

		public async Task<uint> GerarIdAsync(int min, int max, string tabela, string coluna)
		{
			uint idGerado;
			try
			{
				Random ale = new Random();
				do
				{
					idGerado = (uint)ale.Next(min, max);
				}
				while (await VerificarExisteAsync(idGerado.ToString(), tabela, coluna));

				return idGerado;
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return 0;
			}

		}
	}
}

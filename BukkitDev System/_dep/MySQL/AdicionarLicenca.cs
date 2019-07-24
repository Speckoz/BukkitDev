using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Logikoz.BukkitDevSystem._dep.MySQL
{
	internal class AdicionarLicenca
	{
		private const string CmdText = "insert into licencelist values (@a, @b, @c, @d, @e, @f, @g);";
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

					using (MySqlCommand add = new MySqlCommand(CmdText, con))
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
	}
}

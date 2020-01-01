using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Speckoz.BukkitDev._dep.SQLite
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="AtualizarDadosFTP"/>.
	/// </summary>
	internal class AtualizarDadosFTP
	{
		private const string CommandText = "update ftp set host = @a, usuario = @b, senha = @c, porta = @d where tipo = @e";
		/// <summary>
		/// Atualiza os dados de conexao do ftp.
		/// </summary>
		/// <param name="nome">Nome do arquivo SQLite, por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoSQLite"/>.</param>
		/// <param name="dados">Array contendo novos dados.</param>
		/// <param name="tipo">Tipo de conexao (Local ou Externa) OBS: É binary</param>
		/// <returns>Retorna a tarefa com um bool informando se a operaçao foi um sucesso.</returns>
		public async Task<bool> AtualizarAsync(string nome, string[] dados, string tipo)
		{
			using (SQLiteConnection con = new SQLiteConnection($"data source = {nome}; Version = 3;"))
			{
				try
				{
					await con.OpenAsync();

					using (SQLiteCommand add = new SQLiteCommand(CommandText, con))
					{
						_ = add.Parameters.Add(new SQLiteParameter("@a", dados[0]));
						_ = add.Parameters.Add(new SQLiteParameter("@b", dados[1]));
						_ = add.Parameters.Add(new SQLiteParameter("@c", dados[2]));
						_ = add.Parameters.Add(new SQLiteParameter("@d", dados[3]));
						_ = add.Parameters.Add(new SQLiteParameter("@e", tipo));

						_ = await add.ExecuteNonQueryAsync();
						return true;
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
}

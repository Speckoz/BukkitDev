using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;

namespace Logikoz.BukkitDev._dep.SQLite
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="AtualizarDadosMySQL"/>.
	/// </summary>
	internal class AtualizarDadosMySQL
	{
		private const string Query = "update mysql set servidor = @a, usuario = @b, senha = @c, porta = @d, banco = @e where tipo = @f";
		/// <summary>
		/// Atualiza os dados referente a conexao com o banco mysql.
		/// </summary>
		/// <param name="nome">Nome do arquivo SQLite, por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoSQLite"/>.</param>
		/// <param name="dados">Array contendo os novos dados.</param>
		/// <param name="tipo">Tipo de conexao (Local ou Externa). OBS: É binary.</param>
		/// <returns>Retorna a tarefa com um bool informando se a operaçao foi um sucesso.</returns>
		public async Task<bool> AtualizarAsync(string nome, string[] dados, string tipo)
		{
			using (SQLiteConnection con = new SQLiteConnection($"Data Source={nome};Version=3;"))
			{
				try
				{
					return await AbrirConexaoAndSetParameters(dados, tipo, con);
				}
				catch (Exception e)
				{
					_ = MessageBox.Show($"Problema ao atualizar dados na tabela!\nErro: {e}");
					return false;
				}
			}
		}

		private static async Task<bool> AbrirConexaoAndSetParameters(string[] dados, string tipo, SQLiteConnection con)
		{
			await con.OpenAsync();

			using (SQLiteCommand adicionarDados = new SQLiteCommand(Query, con))
			{
				return await RetornarBoolResult(dados, tipo, adicionarDados);
			}
		}

		private static async Task<bool> RetornarBoolResult(string[] dados, string tipo, SQLiteCommand adicionarDados)
		{
			Parametros(dados, tipo, adicionarDados);

			_ = await adicionarDados.ExecuteNonQueryAsync();
			return true;
		}

		private static void Parametros(string[] dados, string tipo, SQLiteCommand adicionarDados)
		{
			_ = adicionarDados.Parameters.Add(new SQLiteParameter("@a", dados[0]));
			_ = adicionarDados.Parameters.Add(new SQLiteParameter("@b", dados[1]));
			_ = adicionarDados.Parameters.Add(new SQLiteParameter("@c", dados[2]));
			_ = adicionarDados.Parameters.Add(new SQLiteParameter("@d", dados[3]));
			_ = adicionarDados.Parameters.Add(new SQLiteParameter("@e", dados[4]));
			_ = adicionarDados.Parameters.Add(new SQLiteParameter("@f", tipo));
		}
	}
}

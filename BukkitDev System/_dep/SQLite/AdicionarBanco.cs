using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;

namespace Logikoz.BukkitDevSystem._dep.SQLite
{
	internal class AdicionarBanco
	{
		private const string CommandText = "update mysql set servidor = @a, usuario = @b, senha = @c, porta = @d, banco = @e where tipo = @f";

		public async Task<bool> EnviarDadosAsync(string nome, string[] dados, string tipo)
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

			using (SQLiteCommand adicionarDados = new SQLiteCommand(CommandText, con))
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

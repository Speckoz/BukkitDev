using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Logikoz.BukkitDevSystem.Controles.Config
{
	/// <summary>
	/// Crian uma nova instancia de <see cref="ConfiguracaoCamposFTP"/>.
	/// </summary>
	internal class ConfiguracaoCamposFTP
	{
		public async void BotaoAsync(string[] dados, string tipo)
		{
			foreach (string v in dados)
			{
				if (string.IsNullOrEmpty(v))
				{
					MessageBox.Show("Voce precisa preencher todos os campos");
					return;
				}
			}

			if (await new AtualizarDadosFTP().AtualizarAsync(PegarInfos.NomeArquivoSQLite, dados, tipo))
			{
				MetodosConstantes.EnviarMenssagem("Dados Gravados com sucesso!");
			}
		}

		public async Task<List<string>> CamposCarregadosAsync(string tipo, UserControl uS)
		{
			if (uS.IsLoaded)
			{
				return await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, tipo, "ftp");
			}
			return null;
		}
	}
	/// <summary>
	/// Cria uma nova instancia de <see cref="ConfiguracaoCamposMySQL"/>.
	/// </summary>
	internal class ConfiguracaoCamposMySQL
	{
		public async void BotaoAsync(string[] dados, string tipo)
		{
			foreach (string v in dados)
			{
				if (string.IsNullOrEmpty(v))
				{
					MessageBox.Show("Voce precisa preencher todos os campos");
					return;
				}
			}
			try
			{
				AtualizarDadosMySQL add = new AtualizarDadosMySQL();
				if (await add.EnviarDadosAsync(PegarInfos.NomeArquivoSQLite, dados, tipo))
				{
					MetodosConstantes.EnviarMenssagem("Dados Gravados com sucesso!");
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
			}
		}

		public async Task<List<string>> CamposCarregadosAsync(string tipo, UserControl uS)
		{
			if (uS.IsLoaded)
			{
				PegarConexaoMySQL_FTP get = new PegarConexaoMySQL_FTP();

				return await get.PegarAsync(PegarInfos.NomeArquivoSQLite, tipo, "mysql");
			}
			return null;
		}
	}
}

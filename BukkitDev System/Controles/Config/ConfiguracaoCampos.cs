using BukkitDev_System._dep;
using BukkitDev_System._dep.SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BukkitDev_System.Controles.Config
{
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

			AdicionandoDadosFTP add = new AdicionandoDadosFTP();
			if (await add.AtualizarAsync(PegarInfos.NomeArquivoSQLite, dados, tipo))
			{
				MetodosConstantes.EnviarMenssagem("Dados Gravados com sucesso!");
			}
		}

		public async Task<List<string>> CamposCarregadosAsync(string tipo, UserControl uS)
		{
			if (uS.IsLoaded)
			{
				PegarConexaoMySQL_FTP get = new PegarConexaoMySQL_FTP();

				List<string> d = await get.PegarAsync(PegarInfos.NomeArquivoSQLite, tipo, "ftp");

				return d;
			}
			return null;
		}
	}

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
				AdicionarBanco add = new AdicionarBanco();
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

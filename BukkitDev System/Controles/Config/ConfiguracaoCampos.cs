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
	/// Crian uma nova instancia de <see cref="ConfiguracaoCampos"/>.
	/// </summary>
	internal class ConfiguracaoCampos
	{
		public async void BotaoAsync(string[] dados, string conexao, string tipoConexao, object nomeClass)
		{
			foreach (string v in dados)
			{
				if (string.IsNullOrEmpty(v))
				{
					MessageBox.Show("Voce precisa preencher todos os campos");
					return;
				}
			}

			if (tipoConexao == "mysql")
			{
				await ((AtualizarDadosMySQL)nomeClass).AtualizarAsync(PegarInfos.NomeArquivoSQLite, dados, conexao);
			}
			else
			{
				await ((AtualizarDadosFTP)nomeClass).AtualizarAsync(PegarInfos.NomeArquivoSQLite, dados, conexao);
			}
			MetodosConstantes.EnviarMenssagem("Dados Gravados com sucesso!");
		}
		public async Task<List<string>> CamposCarregadosAsync(UserControl uS, string conexao, string tipoConexao)
		{
			if (uS.IsLoaded)
			{
				return await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, conexao, tipoConexao);
			}
			return null;
		}
	}
}

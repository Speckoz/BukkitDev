using Logikoz.BukkitDevSystem._dep.XML;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows;

namespace Logikoz.BukkitDevSystem._dep
{
	internal class MudarCor
	{
		[System.Obsolete]
		public static async Task MudarAsync(string cor)
		{
			try
			{
				await AlterarAsync(cor);
			}
			catch
			{
				MetodosConstantes.EnviarMenssagem("Erro ao alterar a cor!");
			}
		}

		[System.Obsolete]
		private static async Task AlterarAsync(string cor)
		{
			PaletteHelper a = new PaletteHelper();
			a.ReplacePrimaryColor(cor);
			a.ReplaceAccentColor(cor);
			await AtualizarEConcluirAsync(cor);
		}

		private static async Task AtualizarEConcluirAsync(string cor)
		{
			new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, "Cor", cor);
			await MetodosConstantes.LerXMLAsync();

			MetodosConstantes.EnviarMenssagem($"Cor do programa foi alterada para {cor}!");
		}
	}
}

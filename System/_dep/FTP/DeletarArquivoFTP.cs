using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Logikoz.BukkitDev._dep.FTP
{
	internal class DeletarArquivoFTP
	{
		/// <summary>
		/// Remove arquivo do servidor pelo protocolo FTP.
		/// </summary>
		/// <param name="tipo">tipo do arquivo (Plugin ou Images), exatamente desta forma com exceçao do PascalCase!</param>
		/// <param name="ftpArquivo">Nome do arquivo a ser deletado incluindo a extensao.</param>
		/// <param name="conexaoFTP">Credenciais para conexao com FTP (host, porta, login, senha) respectivamente.</param>
		public async Task<bool> DeletarAsync(string tipo, string ftpArquivo, List<string> conexaoFTP)
		{
			try
			{
				FtpWebRequest delete = Iniando(tipo, ftpArquivo, conexaoFTP);
				DadosDelete(conexaoFTP, delete);
				//obtendo resposta da requisiçao de forma assincrona
				FtpWebResponse ftpWebResponse = (FtpWebResponse)await delete.GetResponseAsync();
				//liberando os recursos da requisiçao
				ftpWebResponse.Close();
				return true;
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return false;
			}
		}
		private static void DadosDelete(List<string> conexaoFTP, FtpWebRequest delete)
		{
			delete.Credentials = CriandoCredenciais(conexaoFTP);
			delete.Method = WebRequestMethods.Ftp.DeleteFile;
			delete.KeepAlive = false;
		}
		private static NetworkCredential CriandoCredenciais(List<string> conexaoFTP)
		{
			return new NetworkCredential(conexaoFTP[1], conexaoFTP[2]);
		}
		private static FtpWebRequest Iniando(string tipo, string ftpArquivo, List<string> conexaoFTP)
		{
			return (FtpWebRequest)WebRequest.Create(CriandoUri(tipo, ftpArquivo, conexaoFTP[0], conexaoFTP[3]));
		}
		private static Uri CriandoUri(string tipo, string ftpArquivo, string host, string porta)
		{
			return new Uri($"ftp://{host}:{porta}/assets/{tipo}/{ftpArquivo}");
		}
	}
}

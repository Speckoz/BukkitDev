using System;
using System.Collections.Generic;
using System.Net;

namespace BukkitDev_System._dep.FTP
{
	internal class DeletarArquivoFTP
	{
		public async void Deletar(string tipo, string ftpArquivo, List<string> conexaoFTP)
		{
			try
			{
				FtpWebRequest delete = Iniando(tipo, ftpArquivo, conexaoFTP);
				DadosDelete(conexaoFTP, delete);
				//obtendo resposta da requisiçao de forma assincrona
				FtpWebResponse r = (FtpWebResponse)await delete.GetResponseAsync();
				//liberando os recursos da requisiçao
				r.Close();
				//testing, remover caso eu esqueça
				MetodosConstantes.EnviarMenssagem("restos removidos!");
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
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
		private FtpWebRequest Iniando(string tipo, string ftpArquivo, List<string> conexaoFTP)
		{
			return (FtpWebRequest)WebRequest.Create(CriandoUri(tipo, ftpArquivo, conexaoFTP[0], conexaoFTP[3]));
		}
		private Uri CriandoUri(string tipo, string ftpArquivo, string host, string porta)
		{
			return new Uri($"ftp://{host}:{porta}/assets/{tipo}/{ftpArquivo}");
		}
	}
}

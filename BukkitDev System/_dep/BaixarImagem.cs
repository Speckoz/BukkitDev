using Logikoz.BukkitDevSystem._dep.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Logikoz.BukkitDevSystem._dep
{
	class BaixarImagem
	{
		public async Task<BitmapImage> BaixarAsync(string nomeImg)
		{
			try
			{
				List<string> credenciaisFTP = await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, PegarInfos.ConfigFTP, "ftp");

				HttpWebRequest w = (HttpWebRequest)WebRequest.Create($"http://{credenciaisFTP[0]}/bukkitdev/assets/Images/{nomeImg}.png");
				w.AllowWriteStreamBuffering = true;
				w.Timeout = 30000;

				WebResponse webResponse = await w.GetResponseAsync();
				Stream stream = webResponse.GetResponseStream();
				BitmapImage img = new BitmapImage();

				img.BeginInit();
				img.StreamSource = stream;
				img.CacheOption = BitmapCacheOption.OnLoad;
				img.EndInit();
				return img;
			}
			catch (ArgumentOutOfRangeException)
			{
				MetodosConstantes.EnviarMenssagem("TimeOut: Nao foi possivel baixar a imagem padrao!");
				return null;
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return null;
			}
		}
	}
}

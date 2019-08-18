using Logikoz.BukkitDev._dep.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Logikoz.BukkitDev._dep
{
    /// <summary>
    /// Cria uma nova instancia de <see cref="BaixarImagem"/>.
    /// </summary>
    internal class BaixarImagem
    {
        /// <summary>
        /// Baixa uma determinada imagem do servidor e armazena em um <see cref="BitmapImage"/>.
        /// </summary>
        /// <param name="nomeImg">Nome da imagem a ser baixada, normalmente é um codigo de plugin (999999) ou 'default', sem o uso de extensao.</param>
        /// <returns>Retorna a tarefa com uma <see cref="BitmapImage"/> contendo a imagem desejada.</returns>
        public async Task<BitmapImage> BaixarAsync(string nomeImg)
        {
            try
            {
                List<string> credenciaisFTP = await PegarFTPConAsync();

                HttpWebRequest w = (HttpWebRequest)WebRequest.Create($"http://{credenciaisFTP[0]}/BukkitDev/assets/Images/{nomeImg}.png");
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

        private static async Task<List<string>> PegarFTPConAsync()
        {
            return await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, PegarInfos.ConfigFTP, "ftp");
        }
    }
}

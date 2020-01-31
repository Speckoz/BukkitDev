using Microsoft.Win32;

using Speckoz.BukkitDev._dep;

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Speckoz.BukkitDev.Controles.Plugins.Plugin
{
    /// <summary>
    /// Cria uma nova instancia de <see cref="InformacoesAddPlugins"/>.
    /// </summary>
    internal class InformacoesAddPlugins
    {
        /// <summary>
        /// Abre um explorer para procurar uma imagem.
        /// </summary>
        /// <returns>Retorna uma tupla contento o path da imagem selecionada e um <see cref="BitmapImage"/> com a propria imagem.</returns>
        public (string, BitmapImage) ProcurarImagem()
        {
            var procurarImg = new OpenFileDialog
            {
                //por enquanto so tem suporte a .png, mas se achar melhor pode
                Filter = "PNG Files (*.png)|*.png",
                Title = "Procurar Imagem"
            };
            if (procurarImg.ShowDialog().Equals(true))
            {
                //colocando caminho do imagem na var
                string caminhoImagem = procurarImg.FileName;
                //
                if (new FileInfo(caminhoImagem).Length / 1024 > PegarInfos.TamanhoLimitePluginImg)
                {
                    MetodosConstantes.EnviarMenssagem($"Voce precisa escolher uma imagem de no max {Math.Round(PegarInfos.TamanhoLimitePluginImg / 1024.0, 2)}MiB");
                    return (null, null);
                }
                //colocando imagem selecionada no campo.
                return (caminhoImagem, new BitmapImage(new Uri(caminhoImagem)));
            }
            return (null, null);
        }

        /// <summary>
        /// Abre um explorer para procurar um plugin.
        /// </summary>
        /// <returns>Retorna uma tupla contento o path do plugin selecionadioe e o nome do proprio plugin.</returns>
        public (string, string) ProcurarPlugin()
        {
            var pluginProcurar = new OpenFileDialog
            {
                Title = "Procurar Plugin de Minerafiti",
                Filter = "JAR Files (*.jar)|*.jar"
            };
            if (pluginProcurar.ShowDialog() == true)
            {
                if (new FileInfo(pluginProcurar.FileName).Length / 1024 > PegarInfos.TamanhoLimitePluginImg)
                {
                    MetodosConstantes.EnviarMenssagem($"Seu plugin possui mais que o tamanho permitido ({Math.Round(PegarInfos.TamanhoLimitePluginImg / 1024.0, 2)}MiB)");
                    return (null, null);
                }
                return (pluginProcurar.FileName, Path.GetFileNameWithoutExtension(pluginProcurar.FileName));
            }
            return (null, null);
        }
    }
}
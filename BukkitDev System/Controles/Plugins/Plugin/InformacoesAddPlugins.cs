using Logikoz.BukkitDevSystem._dep;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Logikoz.BukkitDevSystem.Controles.Plugins.Plugin
{
	internal class InformacoesAddPlugins
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public (string, BitmapImage) ProcurarImagem()
		{
			OpenFileDialog procurarImg = new OpenFileDialog
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

		public (string, string) ProcurarPlugin()
		{
			OpenFileDialog pluginProcurar = new OpenFileDialog
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

using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace BukkitDev_System._dep.XML
{
	internal static class CriandoArquivoXML
	{
		public static async Task<bool> VerificarECriarAsync(string nome)
		{
			return !File.Exists(nome) ? await CriarArquivo(nome) : true;
		}

		private static async Task<bool> CriarArquivo(string nome)
		{
			using (XmlWriter doc = XmlWriter.Create(nome, Configuracao()))
			{
				try
				{
					return await InserirDadosArquivoAsync(doc);
				}
				catch (Exception erro)
				{
					MetodosConstantes.MostrarExceptions(erro);
					return false;
				}
			}
		}

		private static XmlWriterSettings Configuracao()
		{
			return new XmlWriterSettings
			{
				Async = true,
			};
		}

		private static async Task<bool> InserirDadosArquivoAsync(XmlWriter doc)
		{
			try
			{
				await doc.WriteStartDocumentAsync();
				await doc.WriteStartElementAsync(null, "Config", null);
				await SubTagsAsync(doc);
				await doc.WriteEndElementAsync();
				await doc.WriteEndDocumentAsync();
				await doc.FlushAsync();
				return true;
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return false;
			}
		}
		private static async Task SubTagsAsync(XmlWriter doc)
		{
			try
			{
				await doc.WriteElementStringAsync(null, "MySQL", null, "Local");
				await doc.WriteElementStringAsync(null, "FTP", null, "Local");
				await doc.WriteElementStringAsync(null, "TamanhoMaxPlugin", null, "5120");
				await doc.WriteElementStringAsync(null, "Tema", null, "Dark");
				await doc.WriteElementStringAsync(null, "Cor", null, "Purple");
				await doc.WriteElementStringAsync(null, "TaxaDeTransferencia", null, "2048");
				await doc.WriteElementStringAsync(null, "ImagemPlugin", null, "false");
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
			}
		}
	}
}
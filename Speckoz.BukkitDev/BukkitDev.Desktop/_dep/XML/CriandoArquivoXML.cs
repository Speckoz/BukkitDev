using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Speckoz.BukkitDev._dep.XML
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="CriandoArquivoXML"/>.
	/// </summary>
	internal class CriandoArquivoXML
	{
		/// <summary>
		/// Verifica se o arquivo XML que guarda as configuraçoes do softare existe, caso contrario cria um novo.
		/// </summary>
		/// <param name="nome">Nome do arquivo XML, por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoXML"/>.</param>
		/// <returns>Retorna a tarefa com bool informando se o arquivo existe, ou se a operaçao foi um sucesso.</returns>
		public async Task<bool> VerificarECriarAsync(string nome)
		{
			return !File.Exists(nome) ? await CriarArquivo(nome) : true;
		}
		private async Task<bool> CriarArquivo(string nome)
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
		private XmlWriterSettings Configuracao()
		{
			return new XmlWriterSettings
			{
				Async = true,
			};
		}
		private async Task<bool> InserirDadosArquivoAsync(XmlWriter doc)
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
		private async Task SubTagsAsync(XmlWriter doc)
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
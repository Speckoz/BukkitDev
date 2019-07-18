using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace BukkitDev_System._dep.XML
{
	internal class AtualizandoDadosXML
	{
		public async void AtualizarAsync(string nome, string itemParaAlterar, string novoValor)
		{
			if (!File.Exists(PegarInfos.NomeArquivoXML))
			{
				await CriandoArquivoXML.VerificarECriarAsync(PegarInfos.NomeArquivoXML);
			}
			try
			{
				XmlDocument doc = new XmlDocument();
				CarregarEAtualizar(nome, itemParaAlterar, novoValor, doc);
			}
			catch (Exception erro)
			{
				MetodosConstantes.MostrarExceptions(erro);
			}
		}

		private void CarregarEAtualizar(string nome, string itemParaAlterar, string novoValor, XmlDocument doc)
		{
			doc.Load(nome);

			XPathNavigator pathV = doc.CreateNavigator();

			Atualizar(itemParaAlterar, novoValor, pathV);

			doc.Save(nome);
		}

		private void Atualizar(string itemParaAlterar, string novoValor, XPathNavigator pathV)
		{
			try
			{
				foreach (XPathNavigator item in pathV.SelectDescendants("Config", string.Empty, false))
				{
					item.SelectSingleNode(itemParaAlterar).SetValue(novoValor);
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
			}
		}
	}
}

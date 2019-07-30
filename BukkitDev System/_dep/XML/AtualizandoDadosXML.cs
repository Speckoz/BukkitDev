using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Logikoz.BukkitDevSystem._dep.XML
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="AtualizandoDadosXML"/>.
	/// </summary>
	internal class AtualizandoDadosXML
	{
		/// <summary>
		/// Atualiza o valor de determinado nó dentro do arquivo.
		/// </summary>
		/// <param name="nome">Nome do arquivo XML, por padrao é uma propriedade em <see cref="PegarInfos.NomeArquivoXML"/>.</param>
		/// <param name="itemParaAlterar">Nome do nó que deseja alterar o valor.</param>
		/// <param name="novoValor">Novo valor que será atribuido ao nó.</param>
		public async void AtualizarAsync(string nome, string itemParaAlterar, string novoValor)
		{
			if (!File.Exists(nome))
			{
				_ = await new CriandoArquivoXML().VerificarECriarAsync(nome);
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

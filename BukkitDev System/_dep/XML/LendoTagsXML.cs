using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace BukkitDev_System._dep.XML
{
	internal class LendoTagsXML
	{
		public async Task<List<string>> LerXml(string nome)
		{
			List<string> infos = new List<string>();
			try
			{
				XmlReader ler = XmlReader.Create(nome, new XmlReaderSettings() { Async = true });
				while (await ler.ReadAsync())
				{
					if (ler.NodeType == XmlNodeType.Text)
					{
						infos.Add(await ler.GetValueAsync());
					}
				}
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
			}
			
			return infos;
		}
	}
}

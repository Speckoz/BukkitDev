﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Speckoz.BukkitDev._dep.XML
{
	internal class LendoTagsXML
	{
		/// <summary>
		/// Leitura do arquivo XML que contém as informaçoes de configuraçao do software.
		/// </summary>
		/// <param name="nome">Nome do arquivo, por padrao está em uma propriedade em <see cref="PegarInfos.NomeArquivoXML"/></param>
		/// <returns>Retorna a tarefa com uma lista contendo as informaçoes de dentro do arquivo XML.</returns>
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

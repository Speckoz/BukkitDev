using Logikoz.BukkitDevSystem._dep.XML;
using Logikoz.BukkitDevSystem.Principal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Logikoz.BukkitDevSystem._dep
{
	internal static class MetodosConstantes
	{
		/// <summary>
		/// Mostra as informaçoes da exception disparada.
		/// </summary>
		/// <param name="e">A exception</param>
		public static void MostrarExceptions(Exception e)
		{
			//Tipo: qual exception que foi disparada, lembrando que 'FullName' pega a namespace tbm
			//Erro: mostra a mensagem do erro
			//Informaçoes: mostra as informaçoes gerais do erro 
			//Metodo: deveria mostrar o metodo onde a exceçao foi disparada, mas eu to com preguiça de estudar esse troço ai
			//Linha: troço dificil do cao, passei horas tentando achar um jeito, e nao deu muito certo '-'
			string MensagemErro = $"Tipo: {e.GetType().FullName}\n\nErro: {e.Message}\n\nInformaçoes: {e.StackTrace}";/*\n\nMetodo: {e.StackTrace}\n\nLinha: {new StackTrace().GetFrame(0).GetFileLineNumber()}*/

			//enviando mensagem com o erro
			TelaInicial.EnviarMensagemDialogHostAsync(MensagemErro);
		}
		/// <summary>
		/// Envia uma mensagem no canto da tela que pode ser desativada nas configuraçoes.
		/// </summary>
		/// <param name="mensagem">A mensagem desejada.</param>
		public static void EnviarMenssagem(string mensagem)
		{
			if (TelaInicial.BarraDeNotificacao.IsEnabled)
			{
				TelaInicial.BarraDeNotificacao.MessageQueue.Enqueue(mensagem, "Ok", a => Trace.WriteLine(a), mensagem);
			}
		}
		/// <summary>
		/// Le os dados de dentro do arquivo XML e os armazena nas propriedades em <see cref="PegarInfos"/>.
		/// </summary>
		/// <returns>Retorna a tarefa.</returns>
		public static async Task LerXMLAsync()
		{
			List<string> dados = await new LendoTagsXML().LerXml(PegarInfos.NomeArquivoXML);

			try
			{
				PegarInfos.ConfigMySQL = dados[0];
				PegarInfos.ConfigFTP = dados[1];
				PegarInfos.TamanhoLimitePluginImg = ushort.Parse(dados[2]);
				PegarInfos.Tema = dados[3];
				PegarInfos.Cor = dados[4];
				PegarInfos.TaxaTransferencia = ushort.Parse(dados[5]);
				PegarInfos.ImagemPlugin = dados[6];
			}
			catch (Exception e)
			{
				MostrarExceptions(e);
			}
		}
	}
}

using BukkitDev_System._dep.XML;
using BukkitDev_System.Principal;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace BukkitDev_System._dep
{
	internal class MetodosConstantes
	{
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
		public static void EnviarMenssagem(string mensagem)
		{
			if (TelaInicial.barraDeNotificacao.IsEnabled)
			{
				TelaInicial.barraDeNotificacao.MessageQueue.Enqueue(mensagem, "Ok", a => Trace.WriteLine(a), mensagem);
			}
		}
		#region Leitura dos dados do XML
		public static async System.Threading.Tasks.Task LerXMLAsync()
		{
			List<string> dados = await new LendoTagsXML().LerXml(PegarInfos.NomeArquivoXML);

			try
			{
				PegarInfos.ConfigMySQL = ConfigMySQL(dados);
				PegarInfos.ConfigFTP = ConfigFTP(dados);
				PegarInfos.TamanhoLimitePlugin = TamanhoMaxPlugin(dados);
				PegarInfos.Tema = TemaPrograma(dados);
				PegarInfos.Cor = CorPrograma(dados);
				PegarInfos.TaxaTransferencia = ushort.Parse(dados[5]);
			}
			catch (Exception e)
			{
				MostrarExceptions(e);
			}
		}
		private static string CorPrograma(List<string> dados)
		{
			return dados[4];
		}
		private static string TemaPrograma(List<string> dados)
		{
			return dados[3];
		}
		private static ushort TamanhoMaxPlugin(List<string> dados)
		{
			return ushort.Parse(dados[2]);
		}
		private static string ConfigFTP(List<string> dados)
		{
			return dados[1];
		}
		private static string ConfigMySQL(List<string> dados)
		{
			return dados[0];
		}
		#endregion
	}
}

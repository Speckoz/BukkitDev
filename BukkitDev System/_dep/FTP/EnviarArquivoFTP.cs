using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BukkitDev_System._dep.FTP
{
	internal class EnviarArquivoFTP
	{
		public async Task<bool> EnviarAsync(string tipo, string caminho, string ftpArquivo, List<string> conexaoFTP, ProgressBar carregando_pb)
		{
			try
			{
				CriandoConexaoProtocoloFTP(tipo, caminho, ftpArquivo, conexaoFTP, out FtpWebRequest upload, out FileInfo info);
				using (FileStream fileStream = info.OpenRead())
				{
					using (Stream stream = await upload.GetRequestStreamAsync())
					{
						try
						{
							return await RetornoBool(carregando_pb, info, fileStream, stream);
						}
						catch (Exception e)
						{
							MostrarExceptionMenssagem(e, "Algo deu errado ao enviar o arquivo, verifique sua conexao!");
							//caso aconteça qualquer erro, ele excluirá o arquivo por garantia que nao sobrará restos :D
							new DeletarArquivoFTP().Deletar(tipo, ftpArquivo, conexaoFTP);
						}
					}
				}
			}
			catch (WebException erro)
			{
				MostrarExceptionMenssagem(erro, "Ocorreu um erro ao tentar me comunicar com o servidor FTP!");
			}
			return false;
		}
		private void MostrarExceptionMenssagem(Exception e, string msg)
		{
			MetodosConstantes.MostrarExceptions(e);
			MetodosConstantes.EnviarMenssagem(msg);
		}
		private async Task<bool> RetornoBool(ProgressBar carregando_pb, FileInfo info, FileStream fileStream, Stream stream)
		{
			byte[] buffer = GetBuffer();
			//guarda a quantidade de bytes enviados
			uint bytesEnviar = 0;

			//gerando uma progressbar de dinamicamente
			//ProgressBar bar = new ProgressBar
			//{
			//	Width = 70,
			//	Height = 60,
			//	Value = 0,
			//	Maximum = info.Length,
			//	//setando o style da progressbar, no caso, será circular
			//	Style = (Style)new FrameworkElement().FindResource("MaterialDesignCircularProgressBar")
			//};

			carregando_pb.IsIndeterminate = false;
			carregando_pb.Maximum = info.Length;

			/*
             * ao entrar no loop, so sairá depois que a quantidade de bytes que existem no plugin chegar a ZERO...
             * 
             * ou seja, caso o arquivo tenha 5KiB, ele terá 5120 bytes para enviar, e será enviado parte por parte de acordo com a sua configuraçao de TAXA DE 
             * TRANSFERENCIA, o padrao é 2048, entao será enviado 2048 no primeiro ciclo do loop, depois mais 2048 e irá sobrar apenas 1024, que será enviado no ultimo 
             * e os ciclos do loop terminaram '-'
             */
			uint bytes;
			while ((bytes = (uint)await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
			{
				bytesEnviar = await EnviarBytes(carregando_pb, stream, buffer, bytesEnviar, bytes);
			}
			carregando_pb.IsIndeterminate = true;
			//retornando valor positivo para poder prosseguir com o envio do plugin
			return true;
		}
		private async Task<uint> EnviarBytes(ProgressBar carregando_pb, Stream stream, byte[] buffer, uint bytesEnviar, uint bytes)
		{
			await stream.WriteAsync(buffer, 0, (int)bytes);
			//acumulando os bytes enviados na var {bytesEnviar} e setando o mesmo na progressbar
			carregando_pb.Value = bytesEnviar += bytes;
			return bytesEnviar;
		}
		private byte[] GetBuffer()
		{
			return new byte[PegarInfos.TaxaTransferencia];
		}
		private void CriandoConexaoProtocoloFTP(string tipo, string caminho, string ftpArquivo, List<string> conexaoFTP, out FtpWebRequest upload, out FileInfo info)
		{
			upload = WebRequest(tipo, ftpArquivo, conexaoFTP);
			upload.Credentials = Credenciais(conexaoFTP);
			upload.Method = MetodoRequisicao();
			upload.KeepAlive = false;
			info = new FileInfo(caminho);
		}
		private string MetodoRequisicao()
		{
			return WebRequestMethods.Ftp.UploadFile;
		}
		private FtpWebRequest WebRequest(string tipo, string ftpArquivo, List<string> conexaoFTP)
		{
			return (FtpWebRequest)System.Net.WebRequest.Create(CriandoURI(tipo, ftpArquivo, conexaoFTP[0], conexaoFTP[3]));
		}
		private NetworkCredential Credenciais(List<string> conexaoFTP)
		{
			return new NetworkCredential(conexaoFTP[1], conexaoFTP[2]);
		}
		private Uri CriandoURI(string tipo, string ftpArquivo, string host, string porta)
		{
			return new Uri($"ftp://{host}:{porta}/assets/{tipo}/{ftpArquivo}");
		}
	}
}
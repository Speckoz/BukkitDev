using Speckoz.BukkitDev._dep;
using Speckoz.BukkitDev._dep.FTP;
using Speckoz.BukkitDev._dep.MySQL;
using Speckoz.BukkitDev._dep.SQLite;
using Speckoz.BukkitDev.Principal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Speckoz.BukkitDev.Controles.Plugins.Plugin
{
	public partial class NovoPlugin : UserControl
	{
		public NovoPlugin()
		{
			InitializeComponent();
			if (PegarInfos.ImagemPlugin.Equals("false"))
			{
				cardImagemPlugin.Visibility = Visibility.Collapsed;
			}
		}
		//guarda o caminho da imagem selecionada
		private string _caminhoImagem = null;
		//alterar imagem e guardar o path da imagem no campo {_caminhoImagem}
		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			(string cam, BitmapImage img) = new InformacoesAddPlugins().ProcurarImagem();
			_caminhoImagem = cam;
			ImagemPlugin_img.Source = img;
		}
		//procurar plugin e guardar caminho
		private void ProcurarArquivo_bt_Click(object sender, RoutedEventArgs e)
		{
			(string cam, string sPath) = new InformacoesAddPlugins().ProcurarPlugin();
			CaminhoArquivo_txt.Text = cam;
			NomeDoPlugin_txt.Text = sPath;
			ExcluirArquivo_bt.IsEnabled = true;
			ProcurarArquivo_bt.IsEnabled = false;
		}
		//excluir arquivo selecionado para poder procurar outro
		private void ExcluirArquivo_bt_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(CaminhoArquivo_txt.Text))
			{
				ExcluirArquivo_bt.IsEnabled = false;
				ProcurarArquivo_bt.IsEnabled = true;
				CaminhoArquivo_txt.Clear();
			}
		}

		#region Mudança preço/free && botao limpar
		private void TipoDoPlugin_gb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PrecoDoPlugin_txt.IsEnabled = ((ComboBox)sender).SelectedIndex == 1 ? true : false;
			PrecoDoPlugin_txt.Clear();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			if (await TelaInicial.EscolhaDialogHostAsync("Voce tem certeza que deseja limpar todos os campos?"))
			{
				_caminhoImagem = null;
				ImagemPlugin_img.Source = null;
				ExcluirArquivo_bt.IsEnabled = false;
				ProcurarArquivo_bt.IsEnabled = true;
				CaminhoArquivo_txt.Clear();
				NomeDoPlugin_txt.Clear();
				VersaoDoPlugin_txt.Clear();
				TipoDoPlugin_gb.SelectedIndex = -1;
				PrecoDoPlugin_txt.Clear();
				PrecoDoPlugin_txt.IsEnabled = false;
				AutorDoPlugin_txt.Clear();
				DescricaoDoPlugin_txt.Clear();
				MetodosConstantes.EnviarMenssagem("Os campos foram limpos!");
			}
		}
		#endregion

		//botao de adicionar o plugin
		private async void AdicionarPlugin_bt(object sender, RoutedEventArgs e)
		{
			uint idP = await GerarCodigoPlugin();

			if (!await new Utils().VerificarExisteAsync(idP.ToString(), "PluginList", "ID"))
			{
				//verificando se o cara escolheu alguma imagem
				if (PegarInfos.ImagemPlugin.Equals("true"))
				{
					if (string.IsNullOrEmpty(_caminhoImagem))
					{
						MetodosConstantes.EnviarMenssagem(mensagem: "Voce presica escolher uma imagem!");
						return;
					}
				}
				//verificando se o cara escolheu algum plugin
				if (!string.IsNullOrEmpty(CaminhoArquivo_txt.Text))
				{
					List<string> CamposDados = new List<string>
						{
							NomeDoPlugin_txt.Text,
							AutorDoPlugin_txt.Text,
							VersaoDoPlugin_txt.Text,
							TipoDoPlugin_gb.Text,
							PrecoDoPlugin_txt.Text,
							DescricaoDoPlugin_txt.Text,
							//bool: img padrao(0) ou personalizada(1)
							PegarInfos.ImagemPlugin.Equals("true") ? "1" : "0"
						};
					//verificando se é free ou pago, caso seja free, adicionando 0, pra nao ir como null pro banco
					if (CamposDados[3] == "Gratuito")
					{
						CamposDados[4] = "0";
					}
					//fazendo as demais verificaçoes de campos vazios
					foreach (string @string in CamposDados)
					{
						if (string.IsNullOrEmpty(@string))
						{
							MetodosConstantes.EnviarMenssagem(mensagem: "Voce precisa preencher todos os campos!");
							return;
						}
					}

					try
					{
						//desativando botao para evitar bugs da gambiarra
						AgruparBtArquivo_pb.IsEnabled = false;

						textoTipo_txt.Text = "Plugin";
						//pegando as informaçoes de conexao do ftp de dentro do SQLite
						List<string> dados = await new PegarConexaoMySQL_FTP().PegarAsync(nome: PegarInfos.NomeArquivoSQLite, tipo: PegarInfos.ConfigFTP, "ftp");

						//mostrando progresso bar
						FundoCarregando_gd.Visibility = Visibility.Visible;

						if (await new EnviarArquivoFTP().EnviarAsync(
						//tipo do envio (Plugin|Imagem) para saber em qual pasta por pelo ftp
						"Plugin",
						//caminho do arquivo (plugin)
						CaminhoArquivo_txt.Text,
						//pegando apenas o nome do plugin de dentro do path para nomear o arquivo dentro do servidor
						idP + Path.GetExtension(CaminhoArquivo_txt.Text),
						//conexao ftp
						dados,
						//esse grid é o que mostra a progressbar dentro do card
						Progresso_pb))
						{
							textoTipo_txt.Text = "Imagem";
							try
							{
								if (PegarInfos.ImagemPlugin.Equals("true"))
								{
									//enviando imagem para o servidor
									_ = await new EnviarArquivoFTP().EnviarAsync(tipo: "Images", caminho: _caminhoImagem, ftpArquivo: idP + Path.GetExtension(_caminhoImagem), conexaoFTP: dados, carregando_pb: Progresso_pb);
								}
								//adicionando informaçoes do plugin no banco de dados
								_ = await new PluginInfo().AdicionarDadosAsync(ID: idP, dados: CamposDados);
								//enviando mensagem de sucesso
								MetodosConstantes.EnviarMenssagem(mensagem: "Plugin Adicionado com sucesso");
							}
							catch
							{
								if (await new DeletarArquivoFTP().DeletarAsync("Images", idP + Path.GetExtension(_caminhoImagem), dados))
								{
									MetodosConstantes.EnviarMenssagem("Algo ao adicionar arquivo do plugin no servidor!\nExcluindo restos...");
								}
							}
						}

						//ativando botao novamente
						AgruparBtArquivo_pb.IsEnabled = true;
					}
					catch (Exception erro)
					{
						MetodosConstantes.MostrarExceptions(erro);
					}

					//ocultando grid novamente
					FundoCarregando_gd.Visibility = Visibility.Collapsed;
				}
				else
				{
					MetodosConstantes.EnviarMenssagem(mensagem: "Voce presica escolher o arquivo do plugin");
					return;
				}
			}
			else
			{
				MetodosConstantes.EnviarMenssagem(mensagem: "Codigo gerado ja existe, por favor clique no botao novamente!");
			}
		}

		private static async System.Threading.Tasks.Task<uint> GerarCodigoPlugin()
		{
			return await new Utils().GerarIdAsync(100000, 999999, "PluginList", "ID");
		}

		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (PegarInfos.ImagemPlugin.Equals("false"))
			{
				cardImagemPlugin.Visibility = Visibility.Collapsed;
			}
			else
			{
				cardImagemPlugin.Visibility = Visibility.Visible;
			}
		}
	}
}

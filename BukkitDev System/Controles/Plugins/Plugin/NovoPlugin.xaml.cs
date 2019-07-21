using BukkitDev_System._dep;
using BukkitDev_System._dep.FTP;
using BukkitDev_System._dep.MySQL;
using BukkitDev_System._dep.SQLite;
using BukkitDev_System.Principal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BukkitDev_System.Controles.Plugins.Plugin
{
	public partial class NovoPlugin : UserControl
	{
		public NovoPlugin()
		{
			InitializeComponent();
		}
		//tabela usada para guardar 
		private const string Tabela = "pluginlist";
		//guarda o caminho da imagem selecionada
		private string caminhoImagem = null;
		//alterar imagem e guardar o path da imagem na var {caminhoImagem} 
		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//AdicionarPlugins add = new AdicionarPlugins(PegarInfos.NomeArquivoSQLite);
			OpenFileDialog procurarImg = new OpenFileDialog
			{
				//por enquanto so tem suporte a .png, mas se achar melhor pode 
				Filter = "PNG Files (*.png)|*.png",
				Title = "Procurar Imagem"
			};
			if (procurarImg.ShowDialog().Equals(true))
			{
				//colocando caminho do imagem na var
				caminhoImagem = procurarImg.FileName;
				//
				if (new FileInfo(caminhoImagem).Length / 1024 > 2048)
				{
					MetodosConstantes.EnviarMenssagem("Voce precisa escolher uma imagem de no max 2MiB");
					return;
				}
				//colocando imagem selecionada no campo.
				ImagemdoPlugin_img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(caminhoImagem));
			}
		}
		//procurar plugin e guardar caminho
		private void ProcurarArquivo_bt_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog pluginProcurar = new OpenFileDialog
			{
				Title = "Procurar Plugin de Minerafiti",
				Filter = "JAR Files (*.jar)|*.jar|RAR Files (*.rar)|*.rar"
			};
			if (pluginProcurar.ShowDialog() == true)
			{
				if (new FileInfo(pluginProcurar.FileName).Length / 1024 > PegarInfos.TamanhoLimitePlugin)
				{
					MetodosConstantes.EnviarMenssagem($"Seu plugin possui mais que o tamanho permitido ({PegarInfos.TamanhoLimitePlugin}MiB)");
					return;
				}
				ProcurarArquivo_bt.IsEnabled = false;
				ExcluirArquivo_bt.IsEnabled = true;
				CaminhoArquivo_txt.Text = pluginProcurar.FileName;
				NomeDoPlugin_txt.Text = Path.GetFileNameWithoutExtension(pluginProcurar.FileName);
			}
		}
		//excluir arquivo selecionado para poder procurar outro
		private void ExcluirArquivo_bt_Click(object sender, RoutedEventArgs e)
		{
			if (!ProcurarArquivo_bt.IsEnabled)
			{
				ExcluirArquivo_bt.IsEnabled = false;
				ProcurarArquivo_bt.IsEnabled = true;
				CaminhoArquivo_txt.Clear();
			}
		}

		#region Mudança preço/free && botao limpar
		private void TipoDoPlugin_gb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PrecoDoPlugin_txt.IsEnabled = TipoDoPlugin_gb.SelectedIndex == 1 ? true : false;
			PrecoDoPlugin_txt.Clear();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			if (await TelaInicial.EscolhaDialogHostAsync("Voce tem certeza que deseja limpar todos os campos?"))
			{
				caminhoImagem = null;
				ImagemdoPlugin_img.Source = null;
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

			if (!await new Utils().VerificarExisteAsync(idP.ToString(), Tabela, "id"))
			{
				//verificando se o cara escolheu alguma imagem
				if (!string.IsNullOrEmpty(caminhoImagem))
				{
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
							CaminhoArquivo_txt.Text != string.Empty ? "1" : "0"
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
							PegarConexaoMySQL_FTP get = new PegarConexaoMySQL_FTP();

							//pegando as informaçoes de conexao do ftp de dentro do SQLite
							List<string> dados = await get.PegarAsync(nome: PegarInfos.NomeArquivoSQLite, tipo: PegarInfos.ConfigFTP, "ftp");

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
									//enviando imagem para o servidor
									if (await new EnviarArquivoFTP().EnviarAsync(tipo: "Images", caminho: caminhoImagem, ftpArquivo: idP + Path.GetExtension(caminhoImagem), conexaoFTP: dados, carregando_pb: Progresso_pb))
									{
										//adicionando informaçoes do plugin no banco de dados
										if (await new AdicionarPlugins().AdicionarDadosAsync(id: idP, dados: CamposDados))
										{
											//enviando mensagem de sucesso
											MetodosConstantes.EnviarMenssagem(mensagem: "Plugin Adicionado com sucesso");
										}
									}
								}
								catch
								{
									new DeletarArquivoFTP().DeletarAsync("Images", idP + Path.GetExtension(caminhoImagem), dados);
								}
							}

							//ativando botao novamente
							AgruparBtArquivo_pb.IsEnabled = true;
						}
						catch (Exception erro)
						{
							MetodosConstantes.MostrarExceptions(erro);
							new DeletarArquivoFTP().DeletarAsync("Plugin", idP + Path.GetExtension(CaminhoArquivo_txt.Text), CamposDados);
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
					MetodosConstantes.EnviarMenssagem(mensagem: "Voce presica escolher uma imagem!");
				}
			}
			else
			{
				MetodosConstantes.EnviarMenssagem(mensagem: "Codigo gerado ja existe, por favor clique no botao novamente!");
			}
		}

		private static async System.Threading.Tasks.Task<uint> GerarCodigoPlugin()
		{
			return await new Utils().GerarIdAsync(100000, 999999, Tabela, "id");
		}

		//adiçoes futuras...
		private void ImagemPersonalizada_tb_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}

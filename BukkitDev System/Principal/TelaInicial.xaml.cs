using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.FTP;
using Logikoz.BukkitDevSystem._dep.MySQL;
using Logikoz.BukkitDevSystem._dep.SQLite;
using Logikoz.BukkitDevSystem._dep.XML;
using Logikoz.BukkitDevSystem.Controles.Config;
using Logikoz.BukkitDevSystem.Controles.Plugins.Plugin;
using Logikoz.BukkitDevSystem.Controles.Subs;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Logikoz.BukkitDevSystem.Principal
{
	public partial class TelaInicial : Window
	{
		//propriedades
		public static Snackbar BarraDeNotificacao { get; set; }
		public static DialogHost MensagemPerso { get; set; }
		//campos
		private readonly DispatcherTimer _tema;
		private string _temaAtual;

		[Obsolete]
		public TelaInicial()
		{
			InitializeComponent();
			//criando timer na inicializaçao programa
			_tema = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(3)
			};
			_tema.Tick += Tema_Tick;
			//
			BarraDeNotificacao = BarraNotificacao_sb;
			MensagemPerso = MensagemDialog_dh;
		}
		#region Botoes do topo
		//minimiza a tela
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}
		//maximiza a tela
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			//se a condiçao for verdadeira, ou seja, se a tela estiver maximizada (fullScreen*) ao clicar a tela irá para o normal, senao, será maximizada
			WindowState = (Topmost = WindowState != WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal;
		}

		private async void Button_Click_2(object sender, RoutedEventArgs e)
		{
			//verifica se realmente quer encerrar o programa 
			if (await EscolhaDialogHostAsync("Deseja encerrar o programa?"))
			{
				Application.Current.Shutdown();
			}
		}

		#endregion
		#region Utilitarios
		#region Carregamento Tela Principal
		[Obsolete]
		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//adiciona o nome que aparecerá na barra de tarefas, e tbm no topo da tela caso vc decida remover o topo personalizado {WindowStyle="None"}
			//PADRAO: BukkitDev + System
			//adiciona o nome do programa da textBlock no topo da tela
			TituloPrograma_txt.Text = Title = $"{PegarInfos.Nome} {PegarInfos.SobreNome}";
			//verificando se a janela está carregada
			if (IsLoaded)
			{
				//criando o xml caso ele nao exista
				await CriarLerXmlAsync();
				//verificando se arquivo do banco existe
				//e criando outro caso nao exista.
				await CriandoArquivoSQLiteAsync(new CriarBanco());
				//criar a tabela no banco, caso alguma nao exista...
				await new CriarTabela().CriarAsync();
				//setando seleçoes das configs mysql/ftp
				ConfiguMysqlFtp();
				//setando tema da config
				new TemaWindows().ConfigTemaPrograma(Light_mi, Dark_mi);
				//setando cor da config
				ConfigCorPrograma();
				//setando valor da taxa na config
				ConfigTaxaEnvioPlugin();
				//setando valor do tamanho maximo do plugin permitido
				ConfigTamanhoMaxPlugin();
				//config imagem a ser usada
				ConfigImagem();
			}
		}

		private void ConfigImagem()
		{
			if (!string.IsNullOrEmpty(PegarInfos.ImagemPlugin))
			{
				bool re = PegarInfos.ImagemPlugin.Equals("true");
				EscolherImagemTipo_tb.IsChecked = re;
				EscolherImagem_st.IsEnabled = !re;
			}
		}
		private void ConfigTamanhoMaxPlugin()
		{
			if (!string.IsNullOrEmpty(PegarInfos.TamanhoLimitePlugin.ToString()))
			{
				//alocando valor
				TamanhoInformado_txt.Text = PegarInfos.TamanhoLimitePlugin.ToString();
			}
		}
		private void ConfigTaxaEnvioPlugin()
		{
			if (!string.IsNullOrEmpty(PegarInfos.TaxaTransferencia.ToString()))
			{
				//alocando valor
				TaxaInformada_txt.Text = PegarInfos.TaxaTransferencia.ToString();
			}
		}
		[Obsolete]
		private void ConfigCorPrograma()
		{
			if (!string.IsNullOrEmpty(PegarInfos.Cor))
			{
				PaletteHelper palette = new PaletteHelper();
				//setando cor primaria
				palette.ReplacePrimaryColor(PegarInfos.Cor);
				//setando cor secundaria
				//por enquanto a cor secundaria pega é a mesma da primaria...
				palette.ReplaceAccentColor(PegarInfos.Cor);

				foreach (MenuItem i in Cores_mi.Items)
				{
					i.IsChecked = (string)i.Header == PegarInfos.Cor;
				}
			}
		}
		private void ConfiguMysqlFtp()
		{
			if (!EstaoVazios())
			{
				//verificando valores e setando de acordo com o arquivo xml
				//dados do mysql
				if (PegarInfos.ConfigMySQL == "Local")
				{
					LocalSelecionadoMySQL_mi.IsChecked = true;
					ExternoSelecionadoMySQL_mi.IsChecked = false;
				}
				else if (PegarInfos.ConfigMySQL == "Externo")
				{
					LocalSelecionadoMySQL_mi.IsChecked = false;
					ExternoSelecionadoMySQL_mi.IsChecked = true;
				}
				//dados do ftp
				if (PegarInfos.ConfigFTP == "Local")
				{
					LocalSelecionadoFTP_mi.IsChecked = true;
					ExternoSelecionadoFTP_mi.IsChecked = false;
				}
				else if (PegarInfos.ConfigFTP == "Externo")
				{
					LocalSelecionadoFTP_mi.IsChecked = false;
					ExternoSelecionadoFTP_mi.IsChecked = true;
				}
			}
		}
		private static bool EstaoVazios()
		{
			return string.IsNullOrEmpty(PegarInfos.ConfigMySQL) && string.IsNullOrEmpty(PegarInfos.ConfigFTP);
		}
		private static async Task CriandoArquivoSQLiteAsync(CriarBanco create)
		{
			create.CriarArquivo(PegarInfos.NomeArquivoSQLite);
			//verificando se a tabela nao existe e criando-a caso nao exista
			if (!await create.TabelaExisteAsync(PegarInfos.NomeArquivoSQLite))
			{
				await create.CriarTabelaAsync(PegarInfos.NomeArquivoSQLite);
			}
		}
		private static async Task CriarLerXmlAsync()
		{
			if (await CriandoArquivoXML.VerificarECriarAsync(PegarInfos.NomeArquivoXML))
			{
				//lendo dados do xml e guardando nas variaveis estaticas
				await MetodosConstantes.LerXMLAsync();
			}
		}
		#endregion
		[Obsolete]
		private void Tema_Tick(object sender, EventArgs e)
		{
			TemaWindows temaConfig = new TemaWindows();
			if (temaConfig.TemaClaroHabilitado().@string != _temaAtual)
			{
				new PaletteHelper().SetLightDark(!temaConfig.TemaClaroHabilitado().@bool);
				//MetodosConstantes.EnviarMenssagem("foi mudado");
				_temaAtual = temaConfig.TemaClaroHabilitado().@string;
			}
		}
		//mover a tela
		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				//se nao fizer essa verificaçao, irá ocorrer um erro caso aperte o botao direito do mouse
				//verifico se o botao que foi clicado é o esquerdo, e se isto for verdadeiro, entra no if
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					//se clicar duas vezes em cima do grid, mudará para Maximizado
					//MouseDoubleClick += TelaInicial_MouseDoubleClick;
					//se a janela estiver maximizada, é preciso colocar no estado normal, para entao poder mover.
					if (WindowState == WindowState.Maximized)
					{
						//setando o estado da jenela para o normal
						WindowState = WindowState.Normal;
						//setando a posiçao da janela em relaçao ao topo como 0
						Top = 0;
						//desativando o topMost para evitar bugs
						Topmost = false;
					}
					//com esse metodo, é possivel arrastar a janela na qual está o objeto, no caso, eu coloquei esse para quando ele disparar o evento mouseDown(clicou em cima) no grid do topo (que contem os botoes de fechar, minimizar...etc).
					DragMove();
				}
				else
				{
					//se a propriedade TopMost (sempre na frente) estiver ativa, a mesma é desativada
					if (Topmost)
					{
						Topmost = false;
						MetodosConstantes.EnviarMenssagem("Nao está mais fixado");
					}
					else
					{
						//senao, eh preciso verificar se o estado atual da janela é maximizado, ja que se eu tentar ficar uma janela sendo que ela esta maximizada nao tem sentido
						if (WindowState != WindowState.Maximized)
						{
							Topmost = true;
							MetodosConstantes.EnviarMenssagem("Agora está fixado");
						}
						else
						{
							MetodosConstantes.EnviarMenssagem("Voce nao pode fazer isto com a janela maximizada!");
						}
					}
				}
			}
			//isso nao é necessario, pois ele so entrará no if, se ele apertar o botao esquerdo, e como ele so dispara a exception se eu clicar em outro botao...isso é irrelevante!
			catch (InvalidOperationException erro)
			{
				MetodosConstantes.MostrarExceptions(erro);
			}
		}
		//sobre o software
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			_ = new Sobre().ShowDialog();
		}
		//avaliar software
		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			_ = Process.Start(PegarInfos.GitHubSourceLink);
		}
		#endregion
		#region CONFIGURAÇOES
		private async void MarcarClicadoAsync(string tipoEnvio, string tipoBanco, List<MenuItem> items, object sender)
		{
			foreach (MenuItem item in items)
			{
				item.IsChecked = item == (MenuItem)sender;
			}
			new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, tipoEnvio, tipoBanco);
			await MetodosConstantes.LerXMLAsync();
			MetodosConstantes.EnviarMenssagem($"{tipoEnvio} alterado para {tipoBanco}");
		}

		private void TipoMySQLLocal_Click(object sender, RoutedEventArgs e)
		{
			MarcarClicadoAsync("MySQL", "Local", new List<MenuItem> { ExternoSelecionadoMySQL_mi, LocalSelecionadoMySQL_mi }, LocalSelecionadoMySQL_mi);
		}
		private void TipoMySQLExterno_Click(object sender, RoutedEventArgs e)
		{
			MarcarClicadoAsync("MySQL", "Externo", new List<MenuItem> { ExternoSelecionadoMySQL_mi, LocalSelecionadoMySQL_mi }, ExternoSelecionadoMySQL_mi);
		}

		private void TipoFTPLocal_Click(object sender, RoutedEventArgs e)
		{
			MarcarClicadoAsync("FTP", "Local", new List<MenuItem> { ExternoSelecionadoFTP_mi, LocalSelecionadoFTP_mi }, LocalSelecionadoFTP_mi);
		}
		private void TipoFTPExterno_Click(object sender, RoutedEventArgs e)
		{
			MarcarClicadoAsync("FTP", "Externo", new List<MenuItem> { ExternoSelecionadoFTP_mi, LocalSelecionadoFTP_mi }, ExternoSelecionadoFTP_mi);
		}

		private void GravarTaxa_bt_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(TaxaInformada_txt.Text))
			{
				GravarAsync("TaxaDeTransferencia", TaxaInformada_txt.Text);
			}
		}
		private void GravarTamanho_bt_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(TamanhoInformado_txt.Text))
			{
				GravarAsync("TamanhoMaxPlugin", TamanhoInformado_txt.Text);
			}
		}
		private async void GravarAsync(string nomeXml, string valor)
		{
			try
			{
				//adicionando novo valor a taxa de transferencia
				new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, nomeXml, valor);
				//atualizando variaveis
				await MetodosConstantes.LerXMLAsync();

				//mostrando que foi enviado
				MetodosConstantes.EnviarMenssagem("Dados gravados com sucesso!");
			}
			catch (Exception erro)
			{
				MetodosConstantes.MostrarExceptions(erro);
			}
		}
		#region mudando a thema do programa
		[Obsolete]
		private async void MudarTema_Click(object sender, RoutedEventArgs e)
		{
			//verificando se "quem" disparou o evento foi o menuItem do Light ou do Dark e guardando resultado em uma var
			bool result = (MenuItem)sender == Light_mi;
			//guardando string referente a "quem" disparou o evento
			string cor = result ? "Light" : "Dark";
			//mudando o estado (Checked) de acordo com o resultado da comparaçao acima
			Light_mi.IsChecked = result;
			Dark_mi.IsChecked = !result;
			//setando tema, lembrando que este metodo recebe TRUE como o dark, e como a comparaçao é com o light eu neguei o result
			new PaletteHelper().SetLightDark(!result);
			//atualizando
			new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, "Tema", cor);
			await MetodosConstantes.LerXMLAsync();
			MetodosConstantes.EnviarMenssagem($"Tema do programa alterado para {cor}");
		}
		[Obsolete]
		private void PadraoWindows_mi_Click(object sender, RoutedEventArgs e)
		{
			if (PadraoWindows_mi.IsChecked)
			{
				DesabilitandoMenus();
				_temaAtual = null;
				_tema.Start();
			}
			else
			{
				Light_mi.IsEnabled = true;
				Dark_mi.IsEnabled = true;
				bool isDark = PegarInfos.Tema == "Dark";
				Light_mi.IsChecked = !isDark;
				Dark_mi.IsChecked = isDark;
				new PaletteHelper().SetLightDark(isDark);
				_temaAtual = null;
				_tema.Stop();
			}
		}
		private void DesabilitandoMenus()
		{
			Light_mi.IsEnabled = false;
			Light_mi.IsChecked = false;
			Dark_mi.IsEnabled = false;
			Dark_mi.IsChecked = false;
		}
		#endregion
		#region mudando cor do programa
		[Obsolete]
		private async void SelecionarCorPrograma_Click(object sender, RoutedEventArgs e)
		{
			MenuItem select = (MenuItem)sender;

			foreach (MenuItem i in Cores_mi.Items)
			{
				i.IsChecked = i == select;
			}
			//altera a cor
			await MudarCor.MudarAsync((string)select.Header);
		}
		#endregion
		#region ativar/desativar tool
		private void SelecionarToolPrograma_Click(object sender, RoutedEventArgs e)
		{
			bool @is = ((MenuItem)sender) == AtivarTool_mi;
			DesativarTool_mi.IsChecked = !@is;
			AtivarTool_mi.IsChecked = @is;
			Tool_bt.Visibility = @is ? Visibility.Visible : Visibility.Collapsed;
		}
		#endregion
		#region ativar/desativar Menssagem
		private void MenssagemPrograma_Click(object sender, RoutedEventArgs e)
		{
			bool @is = ((MenuItem)sender) == AtivarMenssagem_mi;
			AtivarMenssagem_mi.IsChecked = @is;
			DesativarMenssagem_mi.IsChecked = !@is;
			BarraDeNotificacao.IsEnabled = @is;
		}
		#endregion
		#region ativar/desativar congiguraçao de imagem
		private async void EscolherImagemTipo_tb_Click(object sender, RoutedEventArgs e)
		{
			if (((ToggleButton)sender).IsChecked.Equals(true))
			{
				new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, "ImagemPlugin", "true");
				EscolherImagem_st.IsEnabled = false;
			}
			else
			{
				new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, "ImagemPlugin", "false");
				EscolherImagem_st.IsEnabled = true;
			}
			await MetodosConstantes.LerXMLAsync();
			MetodosConstantes.EnviarMenssagem("Configuraçao de imagem selecionada foi alterada!");
		}
		private async void Button_Click_5(object sender, RoutedEventArgs e)
		{
			OpenFileDialog img = new OpenFileDialog() { Filter = "PNG Files (*.png)|*.png", Title = "Procurar imagem padrao" };

			if (img.ShowDialog().Equals(true))
			{
				ProgressBar progressBar = new ProgressBar() { Margin = new Thickness(10, 0, 0, 0), IsIndeterminate = true, Height = 40, Width = 40, BorderThickness = new Thickness(6), Style = (Style)FindResource("MaterialDesignCircularProgressBar"), ToolTip = "Enviando imagem..." };
				_ = MenuPrincipal_mn.Items.Add(progressBar);
				if (await new EnviarArquivoFTP().EnviarAsync("images", img.FileName, "default.png", await CredenciaisFTP(), progressBar))
				{
					MenuPrincipal_mn.Items.Remove(progressBar);
					ImagemPadraol_img.ImageSource = new BitmapImage(new Uri(img.FileName));
					MetodosConstantes.EnviarMenssagem("Imagem padrao alterada com sucesso!");
				}
			}
		}
		private static Task<List<string>> CredenciaisFTP()
		{
			return new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, PegarInfos.ConfigFTP, "ftp");
		}
		#endregion
		#endregion
		#region Controles de Usuarios
		private void AdicionarNovoUserControl(UIElement uControl)
		{
			//limpa tudo de dentro do grid
			//ControlesDeTela_gd.Children.Clear();
			ControlesDeTela_sv.Content = uControl;
			//adiciona um novo elemento, que no caso é um UserControl
			//ControlesDeTela_gd.Children.Add(uControl);
		}

		//plugin
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new NovoPlugin());
		}
		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new ListarPlugins());
		}
		private void MenuItem_Click_6(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new RemoverPlugin());
		}
		//licenças
		private void MenuItem_Click_4(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new Controles.Plugins.Licenca.AdicionarLicenca());
		}
		private void MenuItem_Click_5(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new Controles.Plugins.Licenca.ListarLicenca());
		}
		private void MenuItem_Click_7(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new Controles.Plugins.Licenca.SuspenderLicenca());
		}
		//configuraçao
		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new MySQLConfig(PegarInfos.ConfigMySQL));
		}
		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			AdicionarNovoUserControl(new FTPConfig(PegarInfos.ConfigFTP));
		}
		#endregion
		#region ignorar
		public static async void EnviarMensagemDialogHostAsync(string msg)
		{
			try
			{
				_ = await DialogHost.Show(new DialogHostSimples { Mensagem_txt = { Text = msg } }, "RootDialog");
			}
			catch
			{
				MetodosConstantes.EnviarMenssagem("Ouve um problema na inicializaçao");
			}
		}
		public static async Task<bool> EscolhaDialogHostAsync(string msg)
		{
			try
			{
				DialogHostEscolha esc = new DialogHostEscolha { Mensagem_txt = { Text = msg } };
				_ = await DialogHost.Show(esc, "RootDialog");

				return esc.clicouAceitar;
			}
			catch
			{
				MetodosConstantes.EnviarMenssagem("Erro ao escolher uma opçao");
				return false;
			}
		}
		//testes
		//private void Window_MouseMove(object sender, MouseEventArgs e)
		//{
		//	Cursor = e.GetPosition(this).X <= Width && e.GetPosition(this).X > Width - 6 ? Cursors.ScrollWE : Cursors.Arrow;
		//}
		#endregion
	}
}

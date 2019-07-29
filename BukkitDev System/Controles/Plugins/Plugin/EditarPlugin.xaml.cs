using BukkitDev.System._dep;
using BukkitDev.System.Controles.Plugins.Plugin;
using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using Logikoz.BukkitDevSystem.Principal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Logikoz.BukkitDevSystem.Controles.Plugins.Plugin
{
	/// <summary>
	/// Interaction logic for EditarPlugin.xaml
	/// </summary>
	public partial class EditarPlugin : UserControl
	{
		private string _caminhoImagem = null;

		public EditarPlugin()
		{
			InitializeComponent();
		}

		private async void ProcurarPlugin_bt_Click(object sender, RoutedEventArgs e)
		{
			if (PossuiLetras(CodigoPlugin_txt.Text))
			{
				MetodosConstantes.EnviarMenssagem("Voce deve informar apenas o ID do plugin.");
				InformacoesPlugin_wp.Visibility = Visibility.Collapsed;
				return;
			}

			if (!string.IsNullOrEmpty(CodigoPlugin_txt.Text))
			{
				PluginInfo pl = new PluginInfo();

				if (await pl.InformacoesAsync(CodigoPlugin_txt.Text, false))
				{
					InformacoesPlugin_wp.Visibility = Visibility.Visible;
					DataTable a = pl.DataTable;

					if (Convert.ToBoolean(a.Rows[0][7]).Equals(true))
					{
						ImagemPlugin_img.Source = await new BaixarImagem().BaixarAsync(CodigoPlugin_txt.Text);
						cardImagemPlugin.IsEnabled = true;
					}
					else
					{
						ImagemPlugin_img.Source = null;
						cardImagemPlugin.Visibility = Visibility.Collapsed;
					}
					NomeDoPlugin_txt.Text = a.Rows[0][1].ToString();
					AutorDoPlugin_txt.Text = a.Rows[0][2].ToString();
					VersaoDoPlugin_txt.Text = a.Rows[0][3].ToString();
					if (a.Rows[0][4].ToString() == "Pago")
					{
						TipoDoPlugin_gb.Text = "Pago";
						PrecoDoPlugin_txt.Text = a.Rows[0][5].ToString();
					}
					else
					{
						TipoDoPlugin_gb.Text = "Gratuito";
						PrecoDoPlugin_txt.IsEnabled = false;
						PrecoDoPlugin_txt.Clear();
					}
					DescricaoDoPlugin_txt.Text = a.Rows[0][6].ToString();
				}
				else
				{
					MetodosConstantes.EnviarMenssagem("Plugin nao existe!");
					InformacoesPlugin_wp.Visibility = Visibility.Visible;
				}
			}
		}
		private bool PossuiLetras(string @string)
		{
			try
			{
				_ = uint.Parse(@string);
				return false;
			}
			catch (FormatException)
			{
				return true;
			}
		}

		private void CardImagemPlugin_MouseDown(object sender, MouseButtonEventArgs e)
		{
			(string cam, BitmapImage img) = new InformacoesAddPlugins().ProcurarImagem();
			_caminhoImagem = cam;
			ImagemPlugin_img.Source = img;
		}

		private void ProcurarArquivo_bt_Click(object sender, RoutedEventArgs e)
		{
			(string cam, string sPath) = new InformacoesAddPlugins().ProcurarPlugin();
			CaminhoArquivo_txt.Text = cam;
			NomeDoPlugin_txt.Text = sPath;
			ExcluirArquivo_bt.IsEnabled = true;
			ProcurarArquivo_bt.IsEnabled = false;
		}

		private void ExcluirArquivo_bt_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(CaminhoArquivo_txt.Text))
			{
				ExcluirArquivo_bt.IsEnabled = false;
				ProcurarArquivo_bt.IsEnabled = true;
				CaminhoArquivo_txt.Clear();
			}
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			if (await TelaInicial.EscolhaDialogHostAsync("Voce tem certeza que deseja limpar todos os campos?"))
			{
				//caminhoImagem = null;
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

		private void AtualizarInfos_bt_Click(object sender, RoutedEventArgs e)
		{

		}

		private void TipoDoPlugin_gb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PrecoDoPlugin_txt.IsEnabled = ((ComboBox)sender).SelectedIndex == 1 ? true : false;
			PrecoDoPlugin_txt.Clear();
		}
	}
}

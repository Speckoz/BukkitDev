using System.Windows;
using System.Windows.Controls;

namespace Speckoz.BukkitDev.Controles.Config
{
	/// <summary>
	/// Interação lógica para FTPConfig.xam
	/// </summary>
	public partial class FTPConfig : UserControl
	{
		public FTPConfig(string selecionado)
		{
			InitializeComponent();

			if (selecionado == "Local")
			{
				LocalSelecionado_rb.IsChecked = true;
			}
			else if (selecionado == "Externo")
			{
				ExternoSelecionado_rb.IsChecked = true;
			}
		}

		private void Local()
		{
			ConfigFTP_gd.Children.Clear();
			ConfigFTP_gd.Children.Add(new FTPLocal());
		}

		private void Externo()
		{
			ConfigFTP_gd.Children.Clear();
			ConfigFTP_gd.Children.Add(new FTPExterno());
		}

		private void ExternoSelecionado_rb_Checked(object sender, RoutedEventArgs e)
		{
			Externo();
		}

		private void LocalSelecionado_rb_Checked(object sender, RoutedEventArgs e)
		{
			Local();
		}
	}
}

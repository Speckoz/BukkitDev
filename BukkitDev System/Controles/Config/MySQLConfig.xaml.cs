using System.Windows;
using System.Windows.Controls;

namespace Logikoz.BukkitDevSystem.Controles.Config
{
	/// <summary>
	/// Interação lógica para MySQLConfig.xam
	/// </summary>
	public partial class MySQLConfig : UserControl
	{
		public MySQLConfig(string selecionado)
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
			ConfigMysql_gd.Children.Clear();
			ConfigMysql_gd.Children.Add(new MySQLLocalHost());
		}

		private void Externo()
		{
			ConfigMysql_gd.Children.Clear();
			ConfigMysql_gd.Children.Add(new MySQLExterno());
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

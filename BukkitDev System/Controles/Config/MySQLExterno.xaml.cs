using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BukkitDev_System.Controles.Config
{
	/// <summary>
	/// Interação lógica para MySQLExterno.xam
	/// </summary>
	public partial class MySQLExterno : UserControl
	{
		public MySQLExterno()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string[] d = new string[5];
			d[0] = ServidorDoMySQL_txt.Text;
			d[1] = UsuarioDoMySQL_txt.Text;
			d[2] = SenhaDoMySQL_txt.Password;
			d[3] = PortaDoMySQL_txt.Text;
			d[4] = BancoDoMySQL_txt.Text;

			new ConfiguracaoCamposMySQL().BotaoAsync(d, "Externo");
		}

		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ConfiguracaoCamposMySQL config = new ConfiguracaoCamposMySQL();
			List<string> d = await config.CamposCarregadosAsync("Externo", this);

			ServidorDoMySQL_txt.Text = d[0];
			UsuarioDoMySQL_txt.Text = d[1];
			SenhaDoMySQL_txt.Password = d[2];
			PortaDoMySQL_txt.Text = d[3];
			BancoDoMySQL_txt.Text = d[4];
		}
	}
}

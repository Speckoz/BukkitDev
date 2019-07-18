using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BukkitDev_System.Controles.Config
{
	/// <summary>
	/// Interação lógica para FTPLocal.xam
	/// </summary>
	public partial class FTPLocal : UserControl
	{
		public FTPLocal()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string[] d = new string[4];
			d[0] = HostDoFTP_txt.Text;
			d[1] = UsuarioDoFTP_txt.Text;
			d[2] = SenhaDoFTP_txt.Password;
			d[3] = PortaDoFTP_txt.Text;

			ConfiguracaoCamposFTP config = new ConfiguracaoCamposFTP();
			config.BotaoAsync(d, "Local");
		}

		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ConfiguracaoCamposFTP config = new ConfiguracaoCamposFTP();
			List<string> d = await config.CamposCarregadosAsync("Local", this);

			HostDoFTP_txt.Text = d[0];
			UsuarioDoFTP_txt.Text = d[1];
			SenhaDoFTP_txt.Password = d[2];
			PortaDoFTP_txt.Text = d[3];
		}
	}
}

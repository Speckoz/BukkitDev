using Logikoz.BukkitDevSystem._dep.SQLite;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Logikoz.BukkitDevSystem.Controles.Config
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

			new ConfiguracaoCampos().BotaoAsync(d, "Local", "ftp", new AtualizarDadosFTP());
		}

		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			List<string> d = await new ConfiguracaoCampos().CamposCarregadosAsync(this, "Local", "ftp");

			HostDoFTP_txt.Text = d[0];
			UsuarioDoFTP_txt.Text = d[1];
			SenhaDoFTP_txt.Password = d[2];
			PortaDoFTP_txt.Text = d[3];
		}
	}
}

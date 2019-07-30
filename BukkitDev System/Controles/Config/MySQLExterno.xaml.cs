using Logikoz.BukkitDevSystem._dep.SQLite;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Logikoz.BukkitDevSystem.Controles.Config
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

			new ConfiguracaoCampos().BotaoAsync(d, "Externo", "mysql", new AtualizarDadosMySQL());
		}

		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			List<string> d = await new ConfiguracaoCampos().CamposCarregadosAsync(this, "Externo", "mysql");

			ServidorDoMySQL_txt.Text = d[0];
			UsuarioDoMySQL_txt.Text = d[1];
			SenhaDoMySQL_txt.Password = d[2];
			PortaDoMySQL_txt.Text = d[3];
			BancoDoMySQL_txt.Text = d[4];
		}
	}
}

using Logikoz.BukkitDev._dep;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Logikoz.BukkitDev.Principal
{
	public partial class Sobre : Window
	{
		public Sobre()
		{
			InitializeComponent();
			NomeShow_txt.Text = $"Nome: {PegarInfos.Nome} {PegarInfos.SobreNome}";
			VersionShow_txt.Text = $"Versao: {PegarInfos.Versao}";
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			_ = Process.Start("https://www.facebook.com/RuanCarlosCS");
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			_ = Process.Start("https://twitter.com/Logikoz");
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			_ = Process.Start("https://www.instagram.com/logikozrc");
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			_ = Process.Start("https://github.com/LogikozRC");
		}
	}
}

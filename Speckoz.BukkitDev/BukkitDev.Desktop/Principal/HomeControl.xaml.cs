using Speckoz.BukkitDev._dep;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Speckoz.BukkitDev.Principal
{
	public partial class HomeControl : UserControl
	{
		public HomeControl()
		{
			InitializeComponent();
			verState_txt.Text = PegarInfos.Versao;
			MsgWelcome_txt.Text += $" {PegarInfos.Nome} {PegarInfos.SobreNome}";
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_ = new Sobre().ShowDialog();
		}

		private void Sobre_bt_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Sobre_bt.SetResourceReference(StyleProperty, "MaterialDesignRaisedButton");
			Sobre_bt.Foreground = Brushes.White;
		}

		private void Sobre_bt_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Sobre_bt.SetResourceReference(StyleProperty, "MaterialDesignFlatButton");
			Sobre_bt.SetResourceReference(ForegroundProperty, "PrimaryHueMidBrush");
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			_ = Process.Start("https://heartdevs.com/");
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			_ = Process.Start("https://discord.gg/J3saJqq");
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			_ = Process.Start(((Hyperlink)sender).NavigateUri.ToString());
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			_ = Process.Start("https://github.com/Logikoz/BukkitDev-System");
		}
	}
}

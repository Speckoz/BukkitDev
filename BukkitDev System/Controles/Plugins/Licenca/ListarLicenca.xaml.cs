using BukkitDev.System._dep.MySQL;
using BukkitDev_System._dep;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BukkitDev_System.Controles.Plugins.Licenca
{
	public partial class ListarLicenca : UserControl
	{
		public ListarLicenca()
		{
			InitializeComponent();
		}

		private async void ProcurarLicenca_bt_Click(object sender, RoutedEventArgs e)
		{
			PegarLicenca pegarLicenca = new PegarLicenca();
			if (BuscarTodos_rb.IsChecked.Equals(true))
			{
				ListaLicencas_sp.Children.Clear();
				List<string> resultado = await pegarLicenca.PegarAsync();
				foreach (string i in resultado)
				{
					DesenhandoInformaçoes(await pegarLicenca.PegarAsync(i));
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(ProcurarLicencaCodUsuario_txt.Text))
				{
					DesenhandoInformaçoes(await pegarLicenca.PegarAsync(ProcurarLicencaCodUsuario_txt.Text));
				}
				else
				{
					MetodosConstantes.EnviarMenssagem("Voce precisa informar uma licença ou codigo de um cliente");
				}
			}

		}

		private void DesenhandoInformaçoes(List<string> resultado)
		{
			Card card = new Card() { Margin = new Thickness(5) };
			card.SetResourceReference(BackgroundProperty, "MaterialDesignBackground");
			Expander ex = new Expander() { Header = resultado[2] };
			ex.SetResourceReference(ForegroundProperty, "MaterialDesignBody");

			WrapPanel geralWrap = new WrapPanel();
			StackPanel st1 = new StackPanel() { Orientation = Orientation.Vertical, Margin = new Thickness(10) };
			_ = st1.Children.Add(new TextBlock() { Text = $"Codigo cliente: {resultado[0]}", Margin = new Thickness(5) });
			_ = st1.Children.Add(new TextBlock() { Text = Convert.ToBoolean(resultado[3]).Equals(false) ? $"Codigo plugin: {resultado[1]}" : "Licença Global", Margin = new Thickness(5) });
			_ = geralWrap.Children.Add(st1);
			StackPanel st2 = new StackPanel() { Orientation = Orientation.Vertical, Margin = new Thickness(10) };
			_ = st2.Children.Add(new TextBlock() { Text = $"Data criaçao: {resultado[4].Substring(0, 10)} as {resultado[5]}", Margin = new Thickness(5) });
			_ = st2.Children.Add(new TextBlock() { Text = $"Suspenso: {Convert.ToBoolean(resultado[6])}", Margin = new Thickness(5) });
			_ = geralWrap.Children.Add(st2);

			ex.Content = geralWrap;
			card.Content = ex;
			_ = ListaLicencas_sp.Children.Add(card);
		}
	}
}

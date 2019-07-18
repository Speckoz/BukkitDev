using BukkitDev_System._dep.MySQL;
using BukkitDev_System.Principal;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BukkitDev_System.Controles.Plugins.Plugin
{
	/// <summary>
	/// Interação lógica para RemoverPlugin.xaml
	/// </summary>
	public partial class RemoverPlugin : UserControl
	{
		private readonly List<string> plugins = new List<string>();
		public RemoverPlugin()
		{
			InitializeComponent();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(PluginID_txt.Text))
			{
				if (!plugins.Contains(PluginID_txt.Text))
				{
					if (await new Utils().VerificarExisteAsync(PluginID_txt.Text, "pluginlist", "id"))
					{
						//criando Chip dinamicamente
						PegarPlugins p = new PegarPlugins();
						_ = await p.InformacoesAsync(false, PluginID_txt.Text);
						StackPanel st = new StackPanel { Orientation = Orientation.Vertical };
						foreach (string item in p.TooltipInfo)
						{
							_ = st.Children.Add(new TextBlock() { Text = item });
						}
						Chip newChip = new Chip { Cursor = Cursors.Arrow, Content = PluginID_txt.Text, ToolTip = st, IsDeletable = true, DeleteToolTip = "Remover", Margin = new Thickness(2) };
						//setando tempo que o tooltip ficará visivel!
						ToolTipService.SetShowDuration(newChip, 100000);
						//gerando metodo do botao de remover o chip
						newChip.DeleteClick += NewChip_DeleteClick;
						//adicionando o chip  no WrapPanel
						_ = ListChips_wp.Children.Add(newChip);
						//adicionando id do plugin em uma List<> para poder saber quais plugins ja foram adicionados!
						plugins.Add(PluginID_txt.Text);
						//variavel contadora de plugins adicionados
						byte i = 1;
						if (ContarPluginAdd_bg.Badge != null)
						{
							i = Convert.ToByte(ContarPluginAdd_bg.Badge);
							i++;
						}
						ContarPluginAdd_bg.Badge = i;
					}
					else
					{
						TelaInicial.EnviarMensagemDialogHostAsync("Plugin informado nao existe!");
					}
				}
				else
				{
					TelaInicial.EnviarMensagemDialogHostAsync("Este plugin ja foi adicionado na lista!");
				}
			}
		}

		private void NewChip_DeleteClick(object sender, RoutedEventArgs e)
		{
			Chip s = (Chip)sender;
			//removendo Chip que foi clicado
			ListChips_wp.Children.Remove(s);
			//removendo pluginID da lista
			_ = plugins.Remove((string)s.Content);
			//decrementando contador
			byte t = Convert.ToByte(ContarPluginAdd_bg.Badge);
			ContarPluginAdd_bg.Badge = --t;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{

		}
	}
}

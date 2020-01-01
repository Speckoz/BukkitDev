using Speckoz.BukkitDev._dep.MySQL;
using Speckoz.BukkitDev.Principal;
using System.Windows;
using System.Windows.Controls;

namespace Speckoz.BukkitDev.Controles.Plugins.Plugin
{
	public partial class ListarPlugins : UserControl
	{
		public ListarPlugins()
		{
			InitializeComponent();
		}
		#region ProcurarPlugin
		private void Desativar()
		{
			ProcurandoPlugin_pb.Visibility = Visibility.Visible;
			TextoAntesGrid_tb.Visibility = Visibility.Collapsed;
			ListaPlugins_gd.Visibility = Visibility.Collapsed;
		}
		private void Ativar(PluginInfo a)
		{
			ListaPlugins_gd.ItemsSource = a.DataTable.DefaultView;
			ProcurandoPlugin_pb.Visibility = Visibility.Collapsed;
			ListaPlugins_gd.Visibility = Visibility.Visible;
			ListaPlugins_gd.Columns[6].MaxWidth = 250;
		}
		//mostrando plugins no dataGrid, assim que o userControl for carregado ou o usuario apertar o botao
		private async void ProcurarPluginAsync()
		{
			PluginInfo get = new PluginInfo();
			Desativar();
			//pegando dados do banco, e adicionando no dataGrid
			if (await get.InformacoesAsync(true, null))
			{
				Ativar(get);
			}
		}
		//procurar plugin e mostrar os resultados no dataGrid em tempo real.
		private async void ProcurarPluginAsync(string item)
		{
			PluginInfo get = new PluginInfo();
			Desativar();
			//pegando dados do banco, e adicionando no dataGrid
			if (await get.InformacoesAsync(item, true))
			{
				Ativar(get);
			}
		}
		#endregion
		//botao de limpar/restaurar campos/informaçoes
		private void LimparRecarregar_bt_Click(object sender, RoutedEventArgs e)
		{
			//limpar o campo de procura
			ProcurarPluginNomeCodigo_txt.Clear();
			//mostra todos os plugins novamente
			ProcurarPluginAsync();
			//desmarcando opçao de procurar por textChanged
			AutoProcurar_cb.IsChecked = false;
			//habilitando botao de procurar
			ProcurarPlugin_bt.IsEnabled = true;
		}
		//ao marcar ou desmarcar o togglebutton irá habilitar/desabilitar o botao de procurar
		private void AutoProcurar_cb_Click(object sender, RoutedEventArgs e)
		{
			//se estiver marcado, desabilita o botao
			if (AutoProcurar_cb.IsChecked == true)
			{
				//desabilitado
				ProcurarPlugin_bt.IsEnabled = false;
			}
			//senao, habilitará o botao novamente
			else
			{
				//habilitado
				ProcurarPlugin_bt.IsEnabled = true;
			}
		}
		//procurar um plugin ao digitar algo no campo
		private void ProcurarPluginNomeCodigo_txt_TextChanged(object sender, TextChangedEventArgs e)
		{
			//se o toggleButton estiver habilitado, ele faz a busca quando o texto mudar, ou seja, toda vez que vc digitar ou apagar algo
			//senao, nao faz nada
			if (AutoProcurar_cb.IsChecked == true)
			{
				ProcurarPluginAsync(ProcurarPluginNomeCodigo_txt.Text);
			}
		}
		//procurar um plugin pelo botao
		private void ProcurarPlugin_bt_Click(object sender, RoutedEventArgs e)
		{
			//se o campo de texto nao estiver vazio, ele faz a verificaçao
			if (!string.IsNullOrEmpty(ProcurarPluginNomeCodigo_txt.Text))
			{
				//procura o plugin informado pelo campo
				ProcurarPluginAsync(ProcurarPluginNomeCodigo_txt.Text);
			}
		}
	}
}

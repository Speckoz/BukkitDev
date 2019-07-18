using BukkitDev_System._dep.MySQL;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BukkitDev_System.Controles.Plugins.Plugin
{
	public partial class ListarPlugins : UserControl
	{
		//devido o construtor da classe fazer uma verificaçao se as duas tabelas existem, nao é nada legal colocar uma nova instancia dentro do segundo metodo {ProcurarPluginAsync}
		//ja que o mesmo faz uma verificaçao a cada vez que o event keyDown for acionado...e isso pode prejudicar muito a sua maquina, ja que o metodo de verificaçao é assincrono, e como eu nao utilizei o Task<>.Run(), ele irá criar threads para cada verificaçao.
		private PegarPlugins get = new PegarPlugins();
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
		private void Ativar()
		{
			ListaPlugins_gd.ItemsSource = get.DataTable.DefaultView;
			ProcurandoPlugin_pb.Visibility = Visibility.Collapsed;
			ListaPlugins_gd.Visibility = Visibility.Visible;
		}
		//mostrando plugins no dataGrid, assim que o userControl for carregado ou o usuario apertar o botao
		private async void ProcurarPluginAsync()
		{
			get = new PegarPlugins();
			Desativar();
			//pegando dados do banco, e adicionando no dataGrid
			if (await get.InformacoesAsync(true, null))
			{
				Ativar();
			}
		}
		//procurar plugin e mostrar os resultados no dataGrid em tempo real.
		private async void ProcurarPluginAsync(string item)
		{
			Desativar();
			//pegando dados do banco, e adicionando no dataGrid
			if (await get.InformacoesAsync(item))
			{
				Ativar();
			}
		}
		#endregion
		//ao clicar duas vezes em cima de uma row do dataGrid, irá abrir a imagem do plugin correspondente
		private void ListaPlugins_gd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//{ListaPlugins_gd.Items[3]} é referente a row do grid que quando vc clicar, irá te redirecionar. eu ainda nao achei uma forma de pegar a row clicada...sou burro
			//string tipoImagem = (ListaPlugins_gd.Columns[7].GetCellContent(ListaPlugins_gd.Items[3]) as CheckBox).IsChecked == true
			//	? (ListaPlugins_gd.Columns[0].GetCellContent(ListaPlugins_gd.Items[3]) as TextBlock).Text
			//	: "padrao";
			////isto apenas te redireciona para um a imagem dentro do servidor pelo protocolo ftp
			//_ = Process.Start($"ftp://localhost/assets/images/{tipoImagem}.png");
		}
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

using BukkitDev.System._dep.MySQL;
using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using Logikoz.BukkitDevSystem.Principal;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Logikoz.BukkitDevSystem.Controles.Plugins.Licenca
{
	public partial class ListarLicenca : UserControl
	{
		public ListarLicenca()
		{
			InitializeComponent();
		}

		private async void ProcurarLicenca_bt_Click(object sender, RoutedEventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				MetodosConstantes.MostrarExceptions(ex);
			}

		}

		private void DesenhandoInformaçoes(List<string> resultado)
		{
			//StackPanel que guardará o icone e a key da licença
			StackPanel hed = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
			//Icon que guardará a messagem de como deletar a lic
			PackIcon icon = new PackIcon() { Kind = PackIconKind.InfoCircle, Margin = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, ToolTip = "Clique dentro do Card que deseja remover a licença!" };
			//adicionando icon no stack
			_ = hed.Children.Add(icon);
			//adicionando key da lic no stack
			_ = hed.Children.Add(new TextBlock { Text = resultado[2], VerticalAlignment = VerticalAlignment.Center });
			//criando o Expander que conterá as informaçoes da licença
			Expander ex = new Expander() { IsExpanded = true, Margin = new Thickness(5), Background = null, FontFamily = new FontFamily("Consolas"), FontSize = 12 };
			//adicionando o stack na proriedade Header (titulo, cabeçario) do expander.
			ex.Header = hed;
			//setando resource no expander, para que quando for mudado o theme do programa, a cor da fonte será a inversa
			ex.SetResourceReference(ForegroundProperty, "MaterialDesignBody");
			//stack que agrupará o clientCode e o pluginCode|GlobalLic
			StackPanel st1 = new StackPanel() { Orientation = Orientation.Vertical, Margin = new Thickness(10) };
			//adicionando mensagem do codigo do cliente
			_ = st1.Children.Add(new TextBlock() { Text = $"Codigo cliente: {resultado[0]}", Margin = new Thickness(5) });
			//adicionando mensagem do codigo do plugin ou licença global
			_ = st1.Children.Add(new TextBlock() { Text = Convert.ToBoolean(resultado[3]).Equals(false) ? $"Codigo plugin: {resultado[1]}" : "Licença Global", Margin = new Thickness(5) });
			//stack que agrupará a data, hora e se está suspenso.
			StackPanel st2 = new StackPanel() { Orientation = Orientation.Vertical, Margin = new Thickness(10) };
			//adicionando data e hora
			_ = st2.Children.Add(new TextBlock() { Text = $"Data criaçao: {resultado[4].Substring(0, 10)} as {resultado[5]}", Margin = new Thickness(5) });
			//adicionando suspensao!.
			_ = st2.Children.Add(new TextBlock() { Text = $"Suspenso: {Convert.ToBoolean(resultado[6])}", Margin = new Thickness(5) });
			//adicionando {st1} e {st2} dentro de um wrap para o texto ficar responsivo.
			WrapPanel geralWrap = new WrapPanel();
			_ = geralWrap.Children.Add(st1);
			_ = geralWrap.Children.Add(st2);
			//adicionando o wrap dentro da propriedade content do expander, ou seja, será as informaçoes da licença ali dentro.
			ex.Content = geralWrap;
			//criando card que terá o expander dentro, este elemento so serve para o expander ficar bunito '-'
			Card card = new Card() { Margin = new Thickness(5) };
			//envento de MouseDown (ao clicar no Card) para poder aplicar na deleçao da lic
			card.MouseDown += Card_MouseDown;
			//adicionando o expander dentro do card
			card.Content = ex;
			//adicionando o card dentro do Stack que foi criado na View, onde conterá todas as licenças(cards) que foram mostradas, e tbm para poder exclui-los.
			_ = ListaLicencas_sp.Children.Add(card);
		}

		private async void Card_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//pegando a key da licenda dentro header do expander.
			string lic = (((((Card)sender).Content as Expander).Header as StackPanel).Children[1] as TextBlock).Text;
			//fazendo uma verificaçao antes de exluir a licença
			if (await TelaInicial.EscolhaDialogHostAsync($"Realmente quer excluir essa licença?\n{lic}"))
			{
				//excluindo a licença do banco, e verificando o retorno da tarefa
				if (await new RemoverItem().ApagarAsync(lic))
				{
					//removendo o card da tela
					ListaLicencas_sp.Children.Remove((Card)sender);
					//enviando a mensagem de sucesso!.
					MetodosConstantes.EnviarMenssagem($"{lic} foi removida!");
				}
			}
		}
	}
}

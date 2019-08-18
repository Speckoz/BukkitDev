using Logikoz.BukkitDev._dep;
using Logikoz.BukkitDev._dep.MySQL;
using Logikoz.BukkitDev.Principal;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Logikoz.BukkitDev.Controles.Plugins.Licenca
{
	public partial class ListarLicenca : UserControl
	{
		private List<string> licsAdd;
		public ListarLicenca()
		{
			InitializeComponent();
			licsAdd = new List<string>();
		}

		private async void ProcurarLicenca_bt_Click(object sender, RoutedEventArgs e)
		{
			string dataPronta = string.Empty;
			if (UsarData_cb.IsChecked.Value)
			{
				if (!string.IsNullOrEmpty(Data_dp.Text))
				{
					string[] t = Data_dp.Text.Split('/');
					dataPronta = $"{t[2]}-{t[1]}-{t[0]}";
				}
				else
				{
					MetodosConstantes.EnviarMenssagem("Voce precisa selecionar uma data.");
					return;
				}
			}
			try
			{
				LicencaInfo pegarLicenca = new LicencaInfo();
				if (BuscarTodos_rb.IsChecked.Equals(true))
				{
					ListaLicencas_sp.Children.Clear();
					List<string> resultado = await pegarLicenca.PegarAsync();
					byte count = 0;
					foreach (string i in resultado)
					{
						if (!await new Utils().VerificarExisteAsync(new string[] { dataPronta, i }, "LicencaList", new string[] { "DataCriacao", "LicencaKey" }) && UsarData_cb.IsChecked.Value)
						{
							count++;
							continue;
						}
						DesenhandoInformaçoes(await pegarLicenca.PegarAsync(i, UsarData_cb.IsChecked.Value ? dataPronta : null));
					}
					if (count == resultado.Count && UsarData_cb.IsChecked.Value)
					{
						MetodosConstantes.EnviarMenssagem("Nao existe nenhuma licença criada nessa data");
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(ProcurarLicencaCodUsuario_txt.Text))
					{
						if (!licsAdd.Contains(ProcurarLicencaCodUsuario_txt.Text))
						{
							licsAdd.Add(ProcurarLicencaCodUsuario_txt.Text);
						}
						else
						{
							MetodosConstantes.EnviarMenssagem("Está licença ja esta na lista.");
							return;
						}
						if (await new Utils().VerificarExisteAsync(ProcurarLicencaCodUsuario_txt.Text, "LicencaList", "LicencaKey"))
						{
							DesenhandoInformaçoes(await pegarLicenca.PegarAsync(ProcurarLicencaCodUsuario_txt.Text));
						}
						else
						{
							MetodosConstantes.EnviarMenssagem("Licença informada nao existe!");
						}
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
			//icone de copiar
			PackIcon ic = new PackIcon() { Kind = PackIconKind.ContentCopy, Height = 15, Width = 15, ToolTip = resultado[2] };
			ToolTipService.SetIsEnabled(ic, false);
			//criando botao que copiará a licença.
			Button copy = new Button { Margin = new Thickness(10, 0, 0, 0), Width = 20, Height = 20, ToolTip = "Copiar", VerticalAlignment = VerticalAlignment.Center, Content = ic };
			//adicioando style Icon no botao
			copy.SetResourceReference(StyleProperty, "MaterialDesignIconButton");
			//evento de click do botao
			copy.Click += Copy_Click;
			//adicionando botao de copiar no stack
			_ = hed.Children.Add(copy);
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
			_ = st2.Children.Add(new TextBlock() { Text = $"Suspensa: {(Convert.ToBoolean(resultado[6]) ? "Sim" : "Nao")}", Margin = new Thickness(5) });
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

		private void Copy_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText((string)(((Button)sender).Content as PackIcon).ToolTip);
		}

		private async void Card_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//pegando a key da licenda dentro header do expander.
			string lic = (((((Card)sender).Content as Expander).Header as StackPanel).Children[1] as TextBlock).Text;
			//fazendo uma verificaçao antes de exluir a licença
			if (await TelaInicial.EscolhaDialogHostAsync($"Realmente quer excluir essa licença?\n{lic}"))
			{
				//excluindo a licença do banco, e verificando o retorno da tarefa
				if (await new LicencaInfo().ApagarAsync(lic))
				{
					//removendo o card da tela
					ListaLicencas_sp.Children.Remove((Card)sender);
					//enviando a mensagem de sucesso!.
					MetodosConstantes.EnviarMenssagem($"{lic} foi removida!");
				}
			}
		}

		private void LimparLista_Click(object sender, RoutedEventArgs e)
		{
			licsAdd.Clear();
			ListaLicencas_sp.Children.Clear();
		}
	}
}

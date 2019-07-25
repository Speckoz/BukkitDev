using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using Logikoz.BukkitDevSystem.Principal;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using add = Logikoz.BukkitDevSystem._dep.MySQL;

namespace Logikoz.BukkitDevSystem.Controles.Plugins.Licenca
{
	public partial class AdicionarLicenca : UserControl
	{
		//todos os caracteres que poderam ter na key, caso queria colocar caracteres especiais basta inserir na string (warning)
		private const string tipoDeCaracteres = "0abc1def2ghi3jkl4mno5pqr6stu7vwy8xz9";

		public AdicionarLicenca()
		{
			InitializeComponent();
		}

		//gera uma key aleatoria
		private string KeyGerada()
		{
			//array que guardará as 5 strings "sorteadas".
			string[] preKey = new string[5];
			//
			CarregarPreKey(preKey, new Random());
			//fazendo a interpolaçao e retornando a licença pronta
			return $"{preKey[0]}-{preKey[1]}-{preKey[2]}-{preKey[3]}-{preKey[4]}";
		}

		private static void CarregarPreKey(string[] preKey, Random ale)
		{
			for (byte i = 0; i < 5; i++)
			{
				//gera uma string aleatoria com os caracteres escolhidos em {tipoDeCaracteres}
				//5 é o tamanho de cada parte da key
				preKey[i] = new string(Enumerable.Repeat(tipoDeCaracteres, 5).Select(key => key[ale.Next(key.Length)]).ToArray());
			}
		}

		private void GerarLicenca_bt_Click(object sender, RoutedEventArgs e)
		{
			LicencaGerada_txt.Text = KeyGerada();
		}

		private void LicencaGlobal_tb_Click(object sender, RoutedEventArgs e)
		{
			CodigoPlugin_txt.IsEnabled = LicencaGlobal_tb.IsChecked != true;
		}

		private async void AdicionarLicenca_bt_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				await VerificarEAdicionarAsync();
			}
			catch (Exception erro)
			{
				MetodosConstantes.MostrarExceptions(erro);
			}
		}

		private async Task VerificarEAdicionarAsync()
		{
			if (!string.IsNullOrEmpty(LicencaGerada_txt.Text))
			{
				bool keyGlobal = LicencaGlobal_tb.IsChecked == true;

				if (VerificarCodigoValido(CodigoPlugin_txt.Text) || keyGlobal)
				{
					if (!await LicencaExisteAsync())
					{
						if (await CodigoExisteOuLicencaIsGlobal(keyGlobal))
						{
							await GerarIDAndAdicionarLicencaAsync(keyGlobal, keyGlobal ? 0 : PluginIDValue());
						}
						else
						{
							TelaInicial.EnviarMensagemDialogHostAsync("ERRO: Plugin informado nao existe!");
						}
					}
					else
					{
						TelaInicial.EnviarMensagemDialogHostAsync("ERRO: Está licença ja existe\nClique no botao para gerar outra licença!");
					}
				}
				else
				{
					TelaInicial.EnviarMensagemDialogHostAsync("Codigo informado é inválido!");
				}
			}
			else
			{
				TelaInicial.EnviarMensagemDialogHostAsync("Voce precisa gerar uma licença primeiro!");
			}
		}

		private async Task<bool> CodigoExisteOuLicencaIsGlobal(bool glob)
		{
			return glob ? true : await CodigoExisteAsync();
		}

		private async Task GerarIDAndAdicionarLicencaAsync(bool keyGlobal, uint pluginID)
		{
			uint userID = await new Utils().GerarIdAsync(10000000, 99999999, "licencalist", "cliente_id");

			await AdicioarLicAsync(keyGlobal, pluginID, userID);
		}

		private async Task<bool> LicencaExisteAsync()
		{
			return await new Utils().VerificarExisteAsync(LicencaGerada_txt.Text, "licencalist", "licenca_key");
		}

		private async Task<bool> CodigoExisteAsync()
		{
			return await new Utils().VerificarExisteAsync(CodigoPlugin_txt.Text, "pluginlist", "id");
		}

		private uint PluginIDValue()
		{
			return VerificarCodigoValido(CodigoPlugin_txt.Text) && !(CodigoPlugin_txt.Text == string.Empty) ? uint.Parse(CodigoPlugin_txt.Text) : 0;
		}

		private async Task AdicioarLicAsync(bool keyGlobal, uint pluginID, uint userID)
		{
			if (await MessageBoxResult(keyGlobal, pluginID, userID))
			{
				if (await new add.AdicionarLicenca().AdicionarAsync(userID, pluginID, LicencaGerada_txt.Text, keyGlobal))
				{
					MetodosConstantes.EnviarMenssagem("Licença adicionada com sucesso!");
				}
			}
		}
		private async Task<bool> MessageBoxResult(bool keyGlobal, uint pluginID, uint userID)
		{
			return await TelaInicial.EscolhaDialogHostAsync($"Voce realmente quer adicionar a licença ao banco?\n\nInformaçoes:\nKey: {LicencaGerada_txt.Text}\nClienteID: {userID}{((pluginID == 0) ? $"\nGlobal: {keyGlobal.ToString()}" : $"\nPluginID: {CodigoPlugin_txt.Text}")}");
		}

		//metodo nao está funcionando
		//motivo: nao sei
		private void CodigoPlugin_txt_KeyDown(object sender, KeyEventArgs e)
		{
			//if (!VerificarCodigoValido(CodigoPlugin_txt.Text))
			//{
			//	e.Handled = false;
			//	MessageBox.Show("caiu");
			//	return;
			//}
			//MessageBox.Show("entrou");
		}

		private bool VerificarCodigoValido(string codigo)
		{
			try
			{
				//se disparar uma exception quer dizer que possui um caracter nao numerico na string, logo, cairá no catch e retornará falso
				//nota: estou usando o discart ( _ ) pois nao preciso do retorno do metodo .Parse()
				_ = uint.Parse(codigo);
				//caso contrario, retornará true pois até este momento a string so contem numeros!
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

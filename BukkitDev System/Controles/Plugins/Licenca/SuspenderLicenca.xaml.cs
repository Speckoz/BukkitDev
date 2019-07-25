using BukkitDev.System._dep.MySQL;
using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem._dep.MySQL;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Logikoz.BukkitDevSystem.Controles.Plugins.Licenca
{
	/// <summary>
	/// Interaction logic for SuspenderLicenca.xaml
	/// </summary>
	public partial class SuspenderLicenca : UserControl
	{
		public SuspenderLicenca()
		{
			InitializeComponent();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(KeyLicencaProcurar_txt.Text))
			{
				bool ret = await new Utils().VerificarExisteAsync(KeyLicencaProcurar_txt.Text, "licencalist", "licenca_key");

				if (ret == true)
				{
					BotoesAcao_sp.IsEnabled = true;
					TrocarStyle(await new SuspencaoLicenca().SuspensoAsync(false, KeyLicencaProcurar_txt.Text, false));
				}
				else
				{
					ZerarStyle();
					BotoesAcao_sp.IsEnabled = false;
				}
			}
		}

		private async void Acao_bts_Click(object sender, RoutedEventArgs e)
		{
			bool ret = ((Button)sender) == SuspenderLicenca_bt;

			TrocarStyle(ret);
			_ = await new SuspencaoLicenca().SuspensoAsync(true, KeyLicencaProcurar_txt.Text, ret);
			MetodosConstantes.EnviarMenssagem($"A licença ({KeyLicencaProcurar_txt.Text}) foi {(ret ? "suspensa" : "realocada")}.");
		}

		private void TrocarStyle(bool ret)
		{
			if (ret)
			{
				RealocarLicenca_bt.SetResourceReference(StyleProperty, "MaterialDesignRaisedButton");
				RealocarLicenca_bt.Foreground = Brushes.White;
				SuspenderLicenca_bt.SetResourceReference(StyleProperty, "MaterialDesignFlatButton");
			}
			else
			{
				SuspenderLicenca_bt.SetResourceReference(StyleProperty, "MaterialDesignRaisedButton");
				SuspenderLicenca_bt.Foreground = Brushes.White;
				RealocarLicenca_bt.SetResourceReference(StyleProperty, "MaterialDesignFlatButton");
			}
		}

		private void ZerarStyle()
		{
			RealocarLicenca_bt.SetResourceReference(StyleProperty, "MaterialDesignFlatButton");
			SuspenderLicenca_bt.SetResourceReference(StyleProperty, "MaterialDesignFlatButton");
		}
	}
}

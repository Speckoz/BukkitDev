using System.Windows;
using System.Windows.Controls;

namespace BukkitDev_System.Controles.Subs
{
	/// <summary>
	/// Interação lógica para DialogHostEscolha.xam
	/// </summary>
	public partial class DialogHostEscolha : UserControl
	{
		//campos
		public bool clicouAceitar;

		public DialogHostEscolha()
		{
			InitializeComponent();
			clicouAceitar = false;
		}

		private void Aceitar_bt_Click(object sender, RoutedEventArgs e)
		{
			clicouAceitar = true;
		}
	}
}

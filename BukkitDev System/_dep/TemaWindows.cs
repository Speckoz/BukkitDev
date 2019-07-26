using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Controls;
using Reg = Microsoft.Win32.Registry;

namespace Logikoz.BukkitDevSystem._dep
{
	internal class TemaWindows
	{
		public (bool @bool, string @string) TemaClaroHabilitado()
		{
			try
			{
				//verificando se o tema do windows está no claro ou escuro (light or dark)
				//retorna 0 se estiver no modo escuro
				//retorna 1 se estiver no modo claro
				//no caso eu coloquei no terceiro parametro que o falor default seria 1, ou seja, eu quero verificar se o tema do windows está como light
				//se voce mudar para 0, o metodo retornará o falor com base no dark, e nao mais no light
				object resultado = Reg.GetValue(Registro(), "AppsUseLightTheme", 1);
				return (resultado.ToString() == "1", resultado.ToString());
			}
			catch (Exception e)
			{
				MetodosConstantes.MostrarExceptions(e);
				return (false, null);
			}
		}
		private static string Registro()
		{
			return @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
		}

		[System.Obsolete]
		public void ConfigTemaPrograma(MenuItem Light_mi, MenuItem Dark_mi)
		{
			if (!string.IsNullOrEmpty(PegarInfos.Tema))
			{
				//verificando qual a cor que esta na config
				//dark ou light
				bool resultado = PegarInfos.Tema == "Dark";
				//atribuino a negaçao do resultado a propriedade IsChecked do menuItem light
				Light_mi.IsChecked = !resultado;
				//atribuino o resultado a propriedade IsChecked do menuItem dark
				Dark_mi.IsChecked = resultado;
				//setando o valor do resultado no metodo
				//sendo que este medo leva como base, o dark! ou seja, se o resultado da comparaçao de .tema e "dark" for verdadeiro, o tema do programa será alterado para dark
				//caso contrario, para light
				new PaletteHelper().SetLightDark(resultado);
			}
		}
	}
}

using Logikoz.BukkitDev._dep.SQLite;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Logikoz.BukkitDev.Controles.Config
{
    /// <summary>
    /// Interação lógica para FTPExterno.xam
    /// </summary>
    public partial class FTPExterno : UserControl
    {
        public FTPExterno()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ConfiguracaoCampos().BotaoAsync(new string[]
            {
                HostDoFTP_txt.Text,
                UsuarioDoFTP_txt.Text,
                SenhaDoFTP_txt.Password,
                PortaDoFTP_txt.Text
            }, "Externo", "ftp", new AtualizarDadosFTP());
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> d = await new ConfiguracaoCampos().CarregarAsync(this, "Externo", "ftp");

            HostDoFTP_txt.Text = d[0];
            UsuarioDoFTP_txt.Text = d[1];
            SenhaDoFTP_txt.Password = d[2];
            PortaDoFTP_txt.Text = d[3];
        }
    }
}

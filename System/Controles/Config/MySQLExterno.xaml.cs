using Logikoz.BukkitDev._dep.SQLite;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Logikoz.BukkitDev.Controles.Config
{
    /// <summary>
    /// Interação lógica para MySQLExterno.xam
    /// </summary>
    public partial class MySQLExterno : UserControl
    {
        public MySQLExterno()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ConfiguracaoCampos().BotaoAsync(new string[]
            {
                ServidorDoMySQL_txt.Text,
                UsuarioDoMySQL_txt.Text,
                SenhaDoMySQL_txt.Password,
                PortaDoMySQL_txt.Text,
                BancoDoMySQL_txt.Text
            }, "Externo", "mysql", new AtualizarDadosMySQL());
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> d = await new ConfiguracaoCampos().CarregarAsync(this, "Externo", "mysql");

            ServidorDoMySQL_txt.Text = d[0];
            UsuarioDoMySQL_txt.Text = d[1];
            SenhaDoMySQL_txt.Password = d[2];
            PortaDoMySQL_txt.Text = d[3];
            BancoDoMySQL_txt.Text = d[4];
        }
    }
}

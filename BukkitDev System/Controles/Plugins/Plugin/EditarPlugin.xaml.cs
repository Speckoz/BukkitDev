using Logikoz.BukkitDevSystem._dep;
using Logikoz.BukkitDevSystem.Controles.Plugins.Plugin;
using Logikoz.BukkitDevSystem._dep.MySQL;
using Logikoz.BukkitDevSystem.Principal;
using System;
using System.Collections.Generic;
using System.Data;
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
using Logikoz.BukkitDevSystem._dep.FTP;
using Logikoz.BukkitDevSystem._dep.SQLite;

namespace Logikoz.BukkitDevSystem.Controles.Plugins.Plugin
{
    /// <summary>
    /// Interaction logic for EditarPlugin.xaml
    /// </summary>
    public partial class EditarPlugin : UserControl
    {
        private string _caminhoImagem = null;

        public EditarPlugin()
        {
            InitializeComponent();
        }

        private async void ProcurarPlugin_bt_Click(object sender, RoutedEventArgs e)
        {
            if (PossuiLetras(CodigoPlugin_txt.Text))
            {
                MetodosConstantes.EnviarMenssagem("Voce deve informar apenas o ID do plugin.");
                InformacoesPlugin_wp.Visibility = Visibility.Collapsed;
                return;
            }

            if (!string.IsNullOrEmpty(CodigoPlugin_txt.Text))
            {
                PluginInfo pl = new PluginInfo();

                if (await pl.InformacoesAsync(CodigoPlugin_txt.Text, false))
                {
                    InformacoesPlugin_wp.Visibility = Visibility.Visible;
                    DataTable a = pl.DataTable;

                    if (Convert.ToBoolean(a.Rows[0][7]).Equals(true))
                    {
                        ImagemPlugin_img.Source = await new BaixarImagem().BaixarAsync(CodigoPlugin_txt.Text);
                        UsarImg_cb.IsChecked = true;
                        EscolherNovaImagem_cb.Visibility = Visibility.Visible;
                        EscolherNovaImagem_cb.IsChecked = false;
                    }
                    else
                    {
                        ImagemPlugin_img.Source = null;
                        UsarImg_cb.IsChecked = false;
                        EscolherNovaImagem_cb.Visibility = Visibility.Collapsed;
                        EscolherNovaImagem_cb.IsChecked = false;
                    }
                    NomeDoPlugin_txt.Text = a.Rows[0][1].ToString();
                    AutorDoPlugin_txt.Text = a.Rows[0][2].ToString();
                    VersaoDoPlugin_txt.Text = a.Rows[0][3].ToString();
                    if (a.Rows[0][4].ToString() == "Pago")
                    {
                        TipoDoPlugin_gb.SelectedIndex = 1;
                        PrecoDoPlugin_txt.Text = a.Rows[0][5].ToString();
                    }
                    else
                    {
                        TipoDoPlugin_gb.SelectedIndex = 0;
                    }
                    DescricaoDoPlugin_txt.Text = a.Rows[0][6].ToString();
                }
                else
                {
                    MetodosConstantes.EnviarMenssagem("Plugin nao existe!");
                    InformacoesPlugin_wp.Visibility = Visibility.Collapsed;
                }
            }
        }
        private bool PossuiLetras(string @string)
        {
            try
            {
                _ = uint.Parse(@string);
                return false;
            }
            catch (FormatException)
            {
                return true;
            }
        }

        private void CardImagemPlugin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (string cam, BitmapImage img) = new InformacoesAddPlugins().ProcurarImagem();
            _caminhoImagem = cam;
            ImagemPlugin_img.Source = img;
        }

        private void ProcurarArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            (string cam, string sPath) = new InformacoesAddPlugins().ProcurarPlugin();
            if (cam != null && sPath != null)
            {
                CaminhoArquivo_txt.Text = cam;
                NomeDoPlugin_txt.Text = sPath;
                ExcluirArquivo_bt.IsEnabled = true;
                ProcurarArquivo_bt.IsEnabled = false;
            }
        }

        private void ExcluirArquivo_bt_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CaminhoArquivo_txt.Text))
            {
                ExcluirArquivo_bt.IsEnabled = false;
                ProcurarArquivo_bt.IsEnabled = true;
                CaminhoArquivo_txt.Clear();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (await TelaInicial.EscolhaDialogHostAsync("Voce tem certeza que deseja limpar todos os campos?"))
            {
                //caminhoImagem = null;
                ImagemPlugin_img.Source = null;
                ExcluirArquivo_bt.IsEnabled = false;
                ProcurarArquivo_bt.IsEnabled = true;
                CaminhoArquivo_txt.Clear();
                NomeDoPlugin_txt.Clear();
                VersaoDoPlugin_txt.Clear();
                TipoDoPlugin_gb.SelectedIndex = -1;
                PrecoDoPlugin_txt.Clear();
                PrecoDoPlugin_txt.IsEnabled = false;
                AutorDoPlugin_txt.Clear();
                DescricaoDoPlugin_txt.Clear();
                MetodosConstantes.EnviarMenssagem("Os campos foram limpos!");
                InformacoesPlugin_wp.Visibility = Visibility.Collapsed;
                CodigoPlugin_txt.Clear();
            }
        }

        private async void AtualizarInfos_bt_Click(object sender, RoutedEventArgs e)
        {
            List<string> CamposDados = new List<string>
            {
                NomeDoPlugin_txt.Text,
                AutorDoPlugin_txt.Text,
                VersaoDoPlugin_txt.Text,
                TipoDoPlugin_gb.Text,
                PrecoDoPlugin_txt.Text,
                DescricaoDoPlugin_txt.Text,
				//bool: img padrao(0) ou personalizada(1)
				UsarImg_cb.IsChecked.Value ? "1" : "0"
            };
            //verificando se é free ou pago, caso seja free, adicionando 0, pra nao ir como null pro banco
            if (CamposDados[3] == "Gratuito")
            {
                CamposDados[4] = "0";
            }
            //fazendo as demais verificaçoes de campos vazios
            foreach (string i in CamposDados)
            {
                if (string.IsNullOrEmpty(i))
                {
                    MetodosConstantes.EnviarMenssagem("Voce precisa preencher todos os campos!");
                    return;
                }
            }
            //pegando conexao ftp do sqlite
            List<string> con = await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, PegarInfos.ConfigFTP, "ftp");
            //criando progressBar que mostrará o progresso no menu.
            ProgressBar progress = new ProgressBar() { Height = 40, Width = 40, IsIndeterminate = true, HorizontalAlignment = HorizontalAlignment.Right };
            //setando style da progressBar
            progress.SetResourceReference(StyleProperty, "MaterialDesignCircularProgressBar");
            //adicionando progressBar no menu
            _ = TelaInicial.MenuPrincipal.Items.Add(progress);
            //...
            progress.ToolTip = "Enviando dados...";
            PluginInfo pl = new PluginInfo();
            //verificando se a opçao de imagem está marcada.
            if (UsarImg_cb.IsChecked.Value)
            {
                if (ImagemPlugin_img.Source == null || (string.IsNullOrEmpty(_caminhoImagem) && EscolherNovaImagem_cb.IsChecked.Value))
                {
                    MetodosConstantes.EnviarMenssagem("Voce precisa selecionar uma imagem.");
                    TelaInicial.MenuPrincipal.Items.Remove(progress);
                    return;
                }
                if (EscolherNovaImagem_cb.IsChecked.Value)
                {
                    progress.ToolTip = "Enviando imagem...";
                    if (await new EnviarArquivoFTP().EnviarAsync("images", _caminhoImagem, $"{CodigoPlugin_txt.Text}.png", con, progress))
                    {
                        MetodosConstantes.EnviarMenssagem($"Novo imagem foi setada para {CodigoPlugin_txt.Text}");
                    }
                }
            }
            else
            {
                _ = await pl.InformacoesAsync(CodigoPlugin_txt.Text, false);
                if (Convert.ToBoolean(pl.DataTable.Rows[0][7]).Equals(true))
                {
                    progress.ToolTip = "Exclundo imagem...";
                    if (await new DeletarArquivoFTP().DeletarAsync("images", $"{CodigoPlugin_txt.Text}.png", con))
                    {
                        MetodosConstantes.EnviarMenssagem("Imagem do plugin foi removida com sucesso!");
                        ImagemPlugin_img.Source = null;
                    }
                }
            }
            if (await pl.AtualizarAsync(CodigoPlugin_txt.Text, CamposDados))
            {
                MetodosConstantes.EnviarMenssagem("Informaçoes do banco foram atualizadas com sucesso!");
            }
            if (!string.IsNullOrEmpty(CaminhoArquivo_txt.Text))
            {
                progress.ToolTip = "Enviando plugin...";
                if (await new EnviarArquivoFTP().EnviarAsync("plugin", CaminhoArquivo_txt.Text, $"{CodigoPlugin_txt.Text}.jar", con, progress))
                {
                    MetodosConstantes.EnviarMenssagem("Arquivo .jar foi atualizado");
                }
            }
            TelaInicial.MenuPrincipal.Items.Remove(progress);
        }

        private void TipoDoPlugin_gb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrecoDoPlugin_txt.IsEnabled = ((ComboBox)sender).SelectedIndex == 1 ? true : false;
            PrecoDoPlugin_txt.Clear();
        }

        private void UsarImg_cb_Click(object sender, RoutedEventArgs e)
        {
            EscolherNovaImagem_cb.Visibility = ((CheckBox)sender).IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EscolherNovaImagem_cb_Click(object sender, RoutedEventArgs e)
        {
            cardImagemPlugin.Visibility = ((CheckBox)sender).IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

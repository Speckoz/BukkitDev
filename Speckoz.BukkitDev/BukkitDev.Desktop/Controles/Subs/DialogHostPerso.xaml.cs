﻿using Speckoz.BukkitDev._dep;
using System.Windows;
using System.Windows.Controls;

namespace Speckoz.BukkitDev.Controles.Subs
{
    public partial class DialogHostSimples : UserControl
    {
        public DialogHostSimples()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Mensagem_txt.Text);
            MetodosConstantes.EnviarMenssagem("Mensagem copiada com sucesso.");
        }
    }
}

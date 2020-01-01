using Speckoz.BukkitDev._dep.XML;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;

namespace Speckoz.BukkitDev._dep
{
    internal static class MudarCor
    {
        /// <summary>
        /// Altera a cor do programa e atualiza o arquivo XML e as propriedades em <see cref="PegarInfos"/>.
        /// </summary>
        /// <param name="cor">O nome da cor que deseja alterar</param>
        /// <returns>Retorna a tarefa.</returns>
        [Obsolete]
        public static async Task MudarAsync(string cor)
        {
            try
            {
                await AlterarAsync(cor);
            }
            catch
            {
                MetodosConstantes.EnviarMenssagem("Erro ao alterar a cor!");
            }
        }

        [Obsolete]
        private static async Task AlterarAsync(string cor)
        {
            PaletteHelper a = new PaletteHelper();
            a.ReplacePrimaryColor(cor);
            a.ReplaceAccentColor(cor);
            await AtualizarEConcluirAsync(cor);
        }

        private static async Task AtualizarEConcluirAsync(string cor)
        {
            new AtualizandoDadosXML().AtualizarAsync(PegarInfos.NomeArquivoXML, "Cor", cor);
            await MetodosConstantes.LerXMLAsync();

            MetodosConstantes.EnviarMenssagem($"Cor do programa foi alterada para {cor}!");
        }
    }
}

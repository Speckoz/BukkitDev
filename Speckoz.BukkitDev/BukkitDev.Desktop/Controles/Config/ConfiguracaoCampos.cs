using Speckoz.BukkitDev._dep;
using Speckoz.BukkitDev._dep.SQLite;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Speckoz.BukkitDev.Controles.Config
{
    /// <summary>
    /// Crian uma nova instancia de <see cref="ConfiguracaoCampos"/>.
    /// </summary>
    internal class ConfiguracaoCampos
    {
        public async void BotaoAsync(string[] dados, string conexao, string tipoConexao, object nomeClass)
        {
            foreach (string v in dados)
            {
                if (tipoConexao.Equals("mysql") || tipoConexao.Equals("ftp"))
                {
                    if (v.Equals(string.Empty))
                    {
                        continue;
                    }
                }

                if (string.IsNullOrEmpty(v))
                {
                    MetodosConstantes.EnviarMenssagem("Voce precisa preencher todos os campos");
                    return;
                }
            }

            if (tipoConexao == "mysql")
            {
                await ((AtualizarDadosMySQL)nomeClass).AtualizarAsync(PegarInfos.NomeArquivoSQLite, dados, conexao);
            }
            else
            {
                await ((AtualizarDadosFTP)nomeClass).AtualizarAsync(PegarInfos.NomeArquivoSQLite, dados, conexao);
            }
            MetodosConstantes.EnviarMenssagem("Dados Gravados com sucesso!");
        }

        public async Task<List<string>> CarregarAsync(UserControl uS, string conexao, string tipoConexao)
        {
            return uS.IsLoaded ? await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, conexao, tipoConexao) : null;
        }
    }
}
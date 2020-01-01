using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Speckoz.BukkitDev._dep.MySQL
{
    /// <summary>
    /// Cria uma nova instancia de <see cref="Utils"/>.
    /// </summary>
    internal class Utils
    {
        /// <summary>
        /// Procurar por um determinado item dentro do banco de dados
        /// </summary>
        /// <param name="itemProcurado">valor do item a ser procurado</param>
        /// <param name="tabela">Nome da tabela no banco na qual será feito a busca.</param>
        /// <param name="coluna">Index para a busca.</param>
        public async Task<bool> VerificarExisteAsync(string itemProcurado, string tabela, string coluna)
        {
            using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
            {
                try
                {
                    await con.OpenAsync();

                    using (MySqlCommand ver = new MySqlCommand($"select count(*) from {tabela} where {coluna} = @a", con))
                    {
                        _ = ver.Parameters.Add(new MySqlParameter("@a", itemProcurado));

                        byte valid = byte.Parse((await ver.ExecuteScalarAsync()).ToString());

                        return valid > 0;
                    }
                }
                catch (Exception e)
                {
                    MetodosConstantes.MostrarExceptions(e);
                    return false;
                }
            }
        }
        /// <summary>
        /// Procurar por um determinado item dentro do banco de dados
        /// </summary>
        /// <param name="itemProcurado">array contendo os valores dos itens a ser procurados</param>
        /// <param name="tabela">Nome da tabela no banco na qual será feito a busca.</param>
        /// <param name="coluna">Indexs para a busca.</param>
        public async Task<bool> VerificarExisteAsync(string[] itemProcurado, string tabela, string[] coluna)
        {
            using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
            {
                try
                {
                    await con.OpenAsync();

                    using (MySqlCommand ver = new MySqlCommand($"select count(*) from {tabela} where {coluna[0]} = @a && {coluna[1]} = @b", con))
                    {
                        _ = ver.Parameters.Add(new MySqlParameter("@a", itemProcurado[0]));
                        _ = ver.Parameters.Add(new MySqlParameter("@b", itemProcurado[1]));

                        byte valid = byte.Parse((await ver.ExecuteScalarAsync()).ToString());

                        return valid > 0;
                    }
                }
                catch (Exception e)
                {
                    MetodosConstantes.MostrarExceptions(e);
                    return false;
                }
            }
        }
        /// <summary>
        /// Gera um numero aleatorio para ser adicionado no banco como algum tipo de Index.
        /// </summary>
        /// <param name="min">Valor minimo a ser gerado.</param>
        /// <param name="max">Valor maximo a ser gerado.</param>
        /// <param name="tabela">Nome da tabela que será consultada.</param>
        /// <param name="coluna">Nome da coluna dentro da tabela para verificar se ja existe um index com esse codigo.</param>
        /// <returns>Retorna o numero gerado com o tamanho informado</returns>
        public async Task<uint> GerarIdAsync(int min, int max, string tabela, string coluna)
        {
            uint idGerado;
            try
            {
                Random ale = new Random();
                do
                {
                    idGerado = (uint)ale.Next(min, max);
                }
                while (await VerificarExisteAsync(idGerado.ToString(), tabela, coluna));

                return idGerado;
            }
            catch (Exception e)
            {
                MetodosConstantes.MostrarExceptions(e);
                return 0;
            }

        }
    }
}

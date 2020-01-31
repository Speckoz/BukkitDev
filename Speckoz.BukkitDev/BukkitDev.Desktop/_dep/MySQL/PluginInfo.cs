using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

//como o tipo byte representa apenas valores de 0-255, pode ocorrer de ter mais de 255 plugin na busca kkkk entao basta trocar o tipo da variavel aqui
using MaxSize = System.Byte;

namespace Speckoz.BukkitDev._dep.MySQL
{
    /// <summary>
    /// Cria uma nova instancia de <see cref="PluginInfo"/>.
    /// </summary>
    internal class PluginInfo
    {
        #region queries

        //referente ao primeiro metodo de Read
        private const string CmdText2 = "select * from PluginList";

        private const string CmdText3 = "select count(*) from PluginList";

        //referente ao segundo metodo de Read
        private const string CmdText = "select * from PluginList where ID like @a or NomePlugin like @a";

        private const string CmdText1 = "select count(*) from PluginList where ID like @a or NomePlugin like @a";

        //query de atualizaçao
        private const string Atualizar = "update PluginList set NomePlugin = @a, AutorPlugin = @b, VersaoPlugin = @c, TipoPlugin = @d, PrecoPlugin = @e, DescricaoPlugin = @f, ImagemPadraoPersonalizada = @g where ID = @ID";

        #endregion queries

        //guarda a DataTable com informaçoes do plugin.
        public DataTable DataTable { get; set; }

        //usada para mostrar informaçoes nos chips antes de excluir um plugin.
        public List<string> TooltipInfo { get; set; }

        /// <summary>
        /// Adiciona um novo plugin ao banco
        /// </summary>
        /// <param name="ID">Codigo do plugin</param>
        /// <param name="dados">Lista contendo informaçoes a serem adicionadas (Nome, Autor, Versao, Tipo, Preço, Descriçao, Imagem(0, 1)) respectivamente.</param>
        /// <returns></returns>
        public async Task<bool> AdicionarDadosAsync(uint ID, List<string> dados)
        {
            using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
            {
                try
                {
                    await con.OpenAsync();

                    using (MySqlCommand add = new MySqlCommand("insert into PluginList values (@a, @b, @c, @d, @f, @g, @h, @i)", con))
                    {
                        add.Parameters.Add(new MySqlParameter("@a", ID));
                        add.Parameters.Add(new MySqlParameter("@b", dados[0]));
                        add.Parameters.Add(new MySqlParameter("@c", dados[1]));
                        add.Parameters.Add(new MySqlParameter("@d", dados[2]));
                        add.Parameters.Add(new MySqlParameter("@f", dados[3]));
                        add.Parameters.Add(new MySqlParameter("@g", dados[4]));
                        add.Parameters.Add(new MySqlParameter("@h", dados[5]));
                        add.Parameters.Add(new MySqlParameter("@i", dados[6]));

                        await add.ExecuteNonQueryAsync();
                        return true;
                    }
                }
                catch (MySqlException e)
                {
                    MetodosConstantes.MostrarExceptions(e);
                    return false;
                }
            }
        }

        /// <summary>
        /// Pega as informaçoes de todos plugins e guarda na propriedade <see cref="DataTable"/>
        /// </summary>
        /// <param name="dataGrid">se true, adiciona tabela na propriedade <see cref="DataTable"/>. senao, adiciona informaçoes de um determinado plugin na propriedade <see cref="TooltipInfo"/>.</param>
        /// <param name="itemProcurarID">se dataGrid for false, deve informar o codigo do plugin para ser informado. senao, <see cref="Nullable"/></param>
        /// <returns>Retorna o resultado da tarefa em bool.</returns>
        public async Task<bool> InformacoesAsync(bool dataGrid, string itemProcurarID)
        {
            using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
            {
                try
                {
                    await con.OpenAsync();

                    using (var get = new MySqlCommand(CmdText2, con))
                    {
                        if (dataGrid)
                        {
                            using (var mySqlData = new MySqlDataAdapter())
                            {
                                mySqlData.SelectCommand = get;

                                var dataTable = new DataTable();
                                await mySqlData.FillAsync(dataTable);

                                DataTable = dataTable;
                                NomeColunas();

                                using (MySqlCommand num = new MySqlCommand(CmdText3, con))
                                {
                                    MetodosConstantes.EnviarMenssagem(MaxSize.Parse((await num.ExecuteScalarAsync()).ToString()) + " Plugins encontrados!");
                                }

                                return true;
                            }
                        }
                        else
                        {
                            TooltipInfo = new List<string>();
                            get.CommandText = $"{CmdText2} where ID = @ID";
                            get.Parameters.Add(new MySqlParameter("@ID", itemProcurarID));
                            using (var a = (MySqlDataReader)await get.ExecuteReaderAsync())
                            {
                                if (await a.ReadAsync())
                                {
                                    for (byte i = 1; i <= 4; i++)
                                    {
                                        TooltipInfo.Add(a.GetString(i));
                                    }
                                }
                                return true;
                            }
                        }
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
        /// Pega informaçoes de um determinado plugin e guarda na propriedade <see cref="DataTable"/>.
        /// </summary>
        /// <param name="itemProcurar">Index (ID ou Nome do plugin) do plugin que deseja as informaçoes.</param>
        /// <param name="messageReturn">true, se deseja informar informar a mensagem no canto da tela mostrando a quantidade de resultados encontrados.</param>
        /// <returns>Retorna o resultado da tarefa em bool.</returns>
        public async Task<bool> InformacoesAsync(string itemProcurar, bool messageReturn)
        {
            using (var con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
            {
                try
                {
                    await con.OpenAsync();

                    using (var get = new MySqlCommand(CmdText, con))
                    {
                        get.Parameters.Add(new MySqlParameter("@a", "%" + itemProcurar + "%"));

                        using (var mySqlData = new MySqlDataAdapter())
                        {
                            mySqlData.SelectCommand = get;

                            var dataTable = new DataTable();
                            await mySqlData.FillAsync(dataTable);

                            DataTable = dataTable;
                            NomeColunas();

                            using (var num = new MySqlCommand(CmdText1, con))
                            {
                                num.Parameters.Add(new MySqlParameter("@a", "%" + itemProcurar + "%"));
                                string qtd = (await num.ExecuteScalarAsync()).ToString();

                                if (qtd == "0")
                                {
                                    return false;
                                }
                                if (messageReturn)
                                {
                                    MetodosConstantes.EnviarMenssagem(MaxSize.Parse(qtd) + " Plugins encontrados!");
                                }
                            }

                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    MetodosConstantes.MostrarExceptions(e);
                    return false;
                }
            }
        }

        //alterando nome das colunas do DataGrid.
        private void NomeColunas()
        {
            DataTable.Columns[0].ColumnName = "ID";
            DataTable.Columns[1].ColumnName = "Nome";
            DataTable.Columns[2].ColumnName = "Autor";
            DataTable.Columns[3].ColumnName = "Versao";
            DataTable.Columns[4].ColumnName = "Tipo";
            DataTable.Columns[5].ColumnName = "Preço";
            DataTable.Columns[6].ColumnName = "Descriçao";
            DataTable.Columns[7].ColumnName = "Imagem";
        }

        /// <summary>
        /// Atualiza informaçoes do plugin.
        /// </summary>
        /// <param name="pluginID">Identificador do plugin que deseja atualizar</param>
        /// <param name="dados"><see cref="string[]"/> contendo informaçoes do plugin (nome, autor, versao, tipo, preço, descriçao e configImagem) respectivamente.</param>
        /// <returns>Retorna a tarefa informando se foi concluido ou nao (true, false).</returns>
        public async Task<bool> AtualizarAsync(string pluginID, List<string> dados)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
                {
                    await con.OpenAsync();

                    using (MySqlCommand att = new MySqlCommand(Atualizar, con))
                    {
                        att.Parameters.Add(new MySqlParameter("@ID", pluginID));
                        att.Parameters.Add(new MySqlParameter("@a", dados[0]));
                        att.Parameters.Add(new MySqlParameter("@b", dados[1]));
                        att.Parameters.Add(new MySqlParameter("@c", dados[2]));
                        att.Parameters.Add(new MySqlParameter("@d", dados[3]));
                        att.Parameters.Add(new MySqlParameter("@e", dados[4]));
                        att.Parameters.Add(new MySqlParameter("@f", dados[5]));
                        att.Parameters.Add(new MySqlParameter("@g", dados[6]));

                        await att.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                MetodosConstantes.MostrarExceptions(e);
                return false;
            }
        }

        /// <summary>
        /// Remover plugin do banco de dados.
        /// </summary>
        /// <param name="pluginID">Codigo do plugin que dejesa apagar.</param>
        /// <returns>Retorna a tarefa contendo um tupla com os retornos da consulta.</returns>
        public async Task<(bool @bool, string nomePlugin, bool img)> ApagarAsync(uint pluginID)
        {
            try
            {
                using (var con = new MySqlConnection(await PegarConexaoMySQL.ConexaoAsync()))
                {
                    await con.OpenAsync();

                    using (var rem = new MySqlCommand("select NomePlugin, ImagemPadraoPersonalizada from PluginList where ID = @a; delete from PluginList where ID = @a;", con))
                    {
                        rem.Parameters.Add(new MySqlParameter("@a", pluginID));

                        using (var dataReader = (MySqlDataReader)await rem.ExecuteReaderAsync())
                        {
                            string nome = null;
                            bool img = false;
                            if (await dataReader.ReadAsync())
                            {
                                nome = dataReader.GetString(0);
                                img = dataReader.GetBoolean(1);
                            }
                            return (true, nome, img);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MetodosConstantes.MostrarExceptions(e);
                return (false, null, false);
            }
        }
    }
}
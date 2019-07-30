using Logikoz.BukkitDevSystem._dep.SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logikoz.BukkitDevSystem._dep.MySQL
{
	/// <summary>
	/// Cria uma nova instancia de <see cref="PegarConexaoMySQL"/>.
	/// </summary>
	internal class PegarConexaoMySQL
	{
		/// <summary>
		/// Pegar informaçoes dentro do SQLite referente a conexao com o banco MySQL.
		/// </summary>
		/// <returns>Retorna a tarefa contendo a string pronta para fazer a conexao com o banco.</returns>
		public static async Task<string> ConexaoAsync()
		{
			//pegando dados de conexao do MySQL de dentro SQLite
			List<string> db = await new PegarConexaoMySQL_FTP().PegarAsync(PegarInfos.NomeArquivoSQLite, PegarInfos.ConfigMySQL, "mysql");

			return $"server={db[0]};user={db[1]};password={db[2]};port={db[3]};database={db[4]}";
		}
	}
}

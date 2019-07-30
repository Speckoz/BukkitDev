using System;

namespace Logikoz.BukkitDevSystem._dep
{
	/// <summary>
	/// Contem informaçoes de configuraçao do software.
	/// </summary>
	internal static class PegarInfos
	{
		//======================= Propriedades =======================
		/// <summary>
		/// Nome do programa Default 'BukkitDev'
		/// </summary>
		public static string Nome { get; } = "BukkitDev";

		/// <summary>
		/// Segundo nome, o default é apenas um um aplido de 'System'
		/// </summary>
		public static string SobreNome { get; } = "System";
		/// <summary>
		/// Versao atual do software levando em consideraçao as att no GitHub a partir do poste na GB
		/// </summary>
		public static string Versao { get; } = "1.0";
		/// <summary>
		/// Nome do arquivo XML que guarda as informaçoes de configuraçao do software
		/// </summary>
		public static string NomeArquivoXML { get; } = "BukkitdevConfigs.xml";
		/// <summary>
		/// Nome do DB SQLite que guarda a conexao local e externa do FTP e MySQL
		/// </summary>
		public static string NomeArquivoSQLite { get; } = "bukkitdev.db";
		/// <summary>
		/// Link da Source original do GitHub
		/// </summary>
		public static string GitHubSourceLink { get; } = "https://github.com/Logikoz/BukkitDev-System";
		/// <summary>
		/// Tamanho maximo do plugin e Imagem em KibiBytes.
		/// </summary>
		public static ushort TamanhoLimitePluginImg { get; set; }
		/// <summary>
		/// Seta/Pega a velocidade da transferencia FTP em Bytes;
		/// </summary>
		public static ushort TaxaTransferencia { get; set; }
		/// <summary>
		/// Seta/Pega qual tipo de conexao está sendo usada (Local/Externa).
		/// </summary>
		public static string ConfigMySQL { get; set; }
		/// <summary>
		/// Seta/Pega qual tipo de conexao está sendo usada (Local/Externa).
		/// </summary>
		public static string ConfigFTP { get; set; }
		/// <summary>
		/// Seta/Pega o tema do software (Light/Dark).
		/// </summary>
		public static string Tema { get; set; }
		/// <summary>
		/// Seta/Pega a Cor do software, veja as cores suportadas em
		/// <see cref="https://github.com/LogikozRC/BukkitDev-System"/>
		/// </summary>
		public static string Cor { get; set; }
		/// <summary>
		/// Seta/Pega a informaçao se será usada a imagem padrao ou uma personalizada.
		/// (true) Personalizada | (false) Padrao.
		/// </summary>
		public static string ImagemPlugin { get; set; }
	}
}

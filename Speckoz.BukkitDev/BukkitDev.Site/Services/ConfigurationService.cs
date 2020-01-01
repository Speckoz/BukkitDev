using Microsoft.Extensions.Configuration;

using System.IO;

namespace Web.Services
{
    /// <summary>
    /// Serviço para recuperar configurações do appsettings
    /// </summary>
    public class ConfigurationService
    {
        /// <summary>
        /// Recupera os dados do appsettings
        /// </summary>
        /// <param name="dad">Nome do nó pai no json</param>
        /// <param name="children">Nome do nó filho no json</param>
        /// <returns></returns>
        public static string NodeJsonValue(string dad, string children) => GetJson().GetSection(dad).GetSection(children).Value;

        /// <summary>
        /// Lê o arquivo appsettings
        /// </summary>
        private static IConfigurationRoot GetJson() => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();
    }
}
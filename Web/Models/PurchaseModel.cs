using System.IO;
using Microsoft.Extensions.Configuration;

namespace Web.Models
{
    public class PurchaseModel
    {
        public string NodeJsonValue(string dad, string child)
        {
            return GetJson().GetSection(dad).GetSection(child).Value;
        }

        public IConfigurationRoot GetJson()
        {
            var b = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true);
            return b.Build();
        }
    }
}

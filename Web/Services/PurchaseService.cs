using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using Web.Models;

namespace Web.Services
{
    public class PurchaseService
    {
        private readonly string _linkAPI;
        /// <summary>
        /// Cria um novo pagamento, chamando <see cref="CreatePayment(int)"/> para a criaçao do model.
        /// </summary>
        /// <param name="dad">Nome do Nó dentro do json.</param>
        /// <param name="children">Nome do filho desse nó.</param>
        public PurchaseService(string dad, string children)
        {
            _linkAPI = NodeJsonValue(dad, children);
        }

        // POST CreatePayment
        public PurchaseModel CreatePayment(int pluginId)
        {
            RestClient client = new RestClient($"{_linkAPI}/CreatePayment");
            RestRequest request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject(new { pluginId });
            _ = request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Json;
            _ = request.AddJsonBody(json);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<PurchaseModel>(response.Content);
        }

        private static string NodeJsonValue(string a, string b)
        {
            return GetJson().GetSection(a).GetSection(b).Value;
        }

        private static IConfigurationRoot GetJson()
        {
            var b = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true);
            return b.Build();
        }
    }
}

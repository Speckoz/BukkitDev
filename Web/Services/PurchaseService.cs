using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Web.Models;

namespace Web.Services
{
    public class PurchaseService
    {
        private IConfiguration _config;
        private static string LinkAPI;

        public PurchaseService(IConfiguration config)
        {
            _config = config;
            LinkAPI = _config.GetConnectionString("BukkitDevSystemAPI");
        }


        // POST CreatePayment
        static public PurchaseModel CreatePayment(int pluginId)
        {
            RestClient client = new RestClient($"{LinkAPI}/CreatePayment");
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject(new { pluginId });
            request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(json);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<PurchaseModel>(response.Content);
        }
    }
}

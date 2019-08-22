using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Web.Models;

namespace Web.Services
{
    public class PurchaseService
    {
        private static string _linkAPI;

        public PurchaseService()
        {
            _linkAPI = new PurchaseModel().NodeJsonValue("Links", "BukkitDevSystemAPI");
        }

        public static string LinkAPI { get; } = _linkAPI;

        // POST CreatePayment
        public static PurchaseModel CreatePayment(int pluginId)
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
    }
}

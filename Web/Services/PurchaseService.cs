using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services
{
    public static class PurchaseService
    {
        // POST CreatePayment
        static public object CreatePayment(int pluginId)
        {
            RestClient client = new RestClient(@"http://localhost:3000/CreatePayment");
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject(pluginId);
            request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(json);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<object>(response.Content);
        }
    }
}

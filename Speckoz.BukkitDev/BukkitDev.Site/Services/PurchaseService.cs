using Newtonsoft.Json;

using RestSharp;

using System;

using Web.Exceptions;
using Web.Models;

namespace Web.Services
{
    /// <summary>
    /// Representa os serviços relacionados à compras.
    /// </summary>
    public static class PurchaseService
    {
        /// <summary>
        /// URI da api do BukkitDev
        /// </summary>
        private static string _linkAPI = ConfigurationService.NodeJsonValue("Links", "BukkitDevSystemAPI");

        /// <summary>
        /// Cria um novo link de pagamento.
        /// </summary>
        /// <param name="pluginId">ID do plugin à ser comprado</param>
        public static PurchaseModel CreatePayment(int pluginId)
        {
            try
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
            catch (Exception)
            {
                throw new APIException();
            }
        }
    }
}
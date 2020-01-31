using Newtonsoft.Json;

using RestSharp;

using System;
using System.Threading;
using System.Threading.Tasks;

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
        private static readonly string _linkAPI = ConfigurationService.NodeJsonValue("Links", "BukkitDevSystemAPI");

        /// <summary>
        /// Cria um novo link de pagamento.
        /// </summary>
        /// <param name="pluginId">ID do plugin à ser comprado</param>
        public static async Task<PurchaseModel> CreatePayment(int pluginId)
        {
            try
            {
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(JsonConvert.SerializeObject(new { pluginId }));
                var client = new RestClient($"{_linkAPI}/CreatePayment");
                return JsonConvert.DeserializeObject<PurchaseModel>((await client.ExecuteAsync(request, new CancellationToken())).Content);
            }
            catch (Exception)
            {
                throw new APIException();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using Web.Services;

namespace Web.Controllers
{
    public class ComprasController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Plugins");

        public async Task<IActionResult> CreatePaymentLink(int id) => Redirect((await PurchaseService.CreatePayment(id)).Link);

        public IActionResult Resgatar() => View();
    }
}
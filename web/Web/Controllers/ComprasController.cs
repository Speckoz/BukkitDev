using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class ComprasController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Plugins");
        }

        public IActionResult CreatePaymentLink(int id)
        {
            return Redirect(PurchaseService.CreatePayment(id).Link);
        }

        public IActionResult Resgatar()
        {
            return View();
        }
    }
}
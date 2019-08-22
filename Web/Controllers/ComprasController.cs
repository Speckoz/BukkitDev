using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class ComprasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePaymentLink(int pluginId)
        {
            var purchase = PurchaseService.CreatePayment(pluginId);
            return Redirect(purchase.Link) ;
        }
    }
}
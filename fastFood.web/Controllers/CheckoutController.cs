
using Microsoft.AspNetCore.Mvc;
using FastFood.web.ViewModels;

namespace fastFood.web.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View(new CheckoutViewModel());
        }

        [HttpPost]
        public IActionResult Pay(CheckoutViewModel model)
        {
            ViewBag.Status = "Success";
            return View("Result");
        }
    }
}

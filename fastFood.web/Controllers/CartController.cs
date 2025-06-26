
using Microsoft.AspNetCore.Mvc;
using FastFood.web.ViewModels;
using FastFood.web.Models;

namespace fastFood.web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var model = new CartViewModel
            {
                Items = new List<CartItem>(),
                Total = 0
            };
            return View(model);
        }

        public IActionResult Clear()
        {
            return RedirectToAction("Index");
        }
    }
}

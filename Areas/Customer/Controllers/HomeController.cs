using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FastFood.web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.SubCategory)
                .ToListAsync();

            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(int itemId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var userId = claim.Value;

            var existingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId && c.ItemId == itemId);

            if (existingCart != null)
            {
                existingCart.Count += 1;
            }
            else
            {
                var cart = new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ItemId = itemId,
                    Count = 1
                };
                _context.ShoppingCarts.Add(cart);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Item berhasil ditambahkan ke keranjang.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var carts = await _context.ShoppingCarts
                .Include(c => c.Item)
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            if (!carts.Any())
            {
                TempData["Error"] = "Keranjang kamu kosong.";
                return RedirectToAction(nameof(Cart));
            }

            var cartTotal = carts.Sum(c => c.Item.Price * c.Count);

            var orderVM = new OrderViewModel
            {
                OrderHeader = new OrderHeader(),
                CartItems = carts,
                CartTotal = (decimal)cartTotal
            };

            return View(orderVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutConfirmed(OrderHeader orderHeader)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var carts = await _context.ShoppingCarts
                .Include(c => c.Item)
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            if (!carts.Any())
            {
                TempData["Error"] = "Keranjang kamu kosong.";
                return RedirectToAction(nameof(Cart));
            }

            var orderSubTotal = carts.Sum(c => c.Item.Price * c.Count);
string transId = $"INV-{Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()}";

orderHeader.ApplicationUserId = userId;
orderHeader.OrderDate = DateTime.Now;
orderHeader.SubTotal = orderSubTotal; 
orderHeader.OrderTotal = orderSubTotal; 
orderHeader.Status = "Submitted";
orderHeader.PaymentStatus = "Pending";
orderHeader.TransId = transId;


            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();

            foreach (var item in carts)
            {
                var orderDetail = new OrderDetail
                {
                    OrderHeaderId = orderHeader.Id,
                    ItemId = item.ItemId,
                    Count = item.Count,
                    Price = (double)item.Item.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.ShoppingCarts.RemoveRange(carts);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(OrderConfirmation), new { status = "pending", orderId = orderHeader.TransId });
        }

        [Authorize]
        public IActionResult OrderConfirmation(string status, string orderId)
        {
            ViewBag.Status = status;
            ViewBag.OrderId = orderId;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var carts = await _context.ShoppingCarts
                .Include(c => c.Item)
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();

            var cartTotal = carts.Sum(c => c.Item.Price * c.Count);

            var cartVM = new CartViewModel
            {
                CartItems = carts,
                CartTotal = (decimal)cartTotal
            };

            return View(cartVM);
        }
    }
}

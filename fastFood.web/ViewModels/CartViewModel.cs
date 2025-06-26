
using FastFood.web.Models;
using System.Collections.Generic;

namespace FastFood.web.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }
    }
}

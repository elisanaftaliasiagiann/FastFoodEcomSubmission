using FastFood.Models;

namespace FastFood.Models.ViewModels
{
    public class CartViewModel
    {
        public List<ShoppingCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }


}

using FastFood.Models;

namespace FastFood.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ShoppingCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}

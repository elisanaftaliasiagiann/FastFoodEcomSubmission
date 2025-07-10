using FastFood.Models;

namespace FastFood.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public List<ShoppingCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}

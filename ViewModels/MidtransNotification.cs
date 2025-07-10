namespace FastFood.Models.ViewModels
{
    public class MidtransNotification
    {
        public string order_id { get; set; }
        public string transaction_status { get; set; }
        public string fraud_status { get; set; }
    }
}
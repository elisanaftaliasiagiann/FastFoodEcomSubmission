namespace FastFood.web.Models.Midtrans

{
    public class MidtransTransactionRequest
    {
        public TransactionDetails transaction_details { get; set; }
        public CustomerDetails customer_details { get; set; }
        public List<ItemDetail> item_details { get; set; }
    }

    public class TransactionDetails
    {
        public string order_id { get; set; }
        public int gross_amount { get; set; }
    }

    public class CustomerDetails
    {
        public string first_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class ItemDetail
    {
        public string id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}

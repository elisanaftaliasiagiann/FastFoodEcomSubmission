using FastFood.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderDetail
{
    [Key]
    public int Id { get; set; }

    public int OrderHeaderId { get; set; } // Foreign Key

    [ForeignKey("OrderHeaderId")]
    public OrderHeader OrderHeader { get; set; }

    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    public Item Item { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public double Total => Quantity * Price;

    public int Count { get; set; }

}

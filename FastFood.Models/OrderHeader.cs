using FastFood.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

public class OrderHeader
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")]
    public ApplicationUser ApplicationUser { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime TimeOfPick { get; set; }
    public DateTime DateOfPick { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    public string Status { get; set; }
    public string PaymentStatus { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public string TransId { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}

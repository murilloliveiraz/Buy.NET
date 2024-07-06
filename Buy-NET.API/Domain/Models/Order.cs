using System.ComponentModel.DataAnnotations;

namespace Buy_NET.API.Domain.Models;

public class Order
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long CustomerId { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
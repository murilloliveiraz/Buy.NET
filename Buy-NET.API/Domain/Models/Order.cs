using System.ComponentModel.DataAnnotations;

namespace Buy_NET.API.Domain.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
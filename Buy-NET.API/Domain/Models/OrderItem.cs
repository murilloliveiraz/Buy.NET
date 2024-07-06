using System.ComponentModel.DataAnnotations;

namespace Buy_NET.API.Domain.Models;

public class OrderItem
{
    [Key]
    public long Id { get; set; }
    [Required]
    public long OrderId { get; set; }
    [Required]
    public long ProductId { get; set; }
    [Required(ErrorMessage = "É necessário informar a quantidadade")]
    public int Quantity { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
}
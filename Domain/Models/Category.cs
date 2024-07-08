using System.ComponentModel.DataAnnotations;

namespace Buy_NET.API.Domain.Models;

public class Category
{
    [Key]
    public long Id { get; set; }
    [Required(ErrorMessage = "O Nome da categoria é obrigatório")]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; }
    [Required]
    public DateTime RegistrationDate { get; set; }
    public DateTime? InactivationDate { get; set; }
}
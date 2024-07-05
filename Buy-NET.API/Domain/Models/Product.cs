using System.ComponentModel.DataAnnotations;

namespace Buy_NET.API.Domain.Models;

public class Product
{
    [Key]
    public long Id {get; set; }    
    [Required(ErrorMessage = "O Nome do produto é obrigatório")]
    public string Name {get; set; } = string.Empty;   
    public string Description {get; set; }  = string.Empty;
    [Required(ErrorMessage = "O Preço do produto é obrigatório")]  
    public double Price {get; set; }    
    [Required(ErrorMessage = "A quantidade em estoque é obrigatório")]  
    public int StockQuantity {get; set; }   
    [Required] 
    public long CategoryId {get; set; }
    public Category Category { get; set; }
    [Required]
    public DateTime RegistrationDate { get; set; }
    public DateTime? InactivationDate { get; set; }

}
using System.ComponentModel.DataAnnotations;

namespace Buy_NET.API.Domain.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "É obrigatório informar um email")]
    public string Email { get; set; }    
    [Required(ErrorMessage = "É obrigatório informar uma senha")]
    public string Password { get; set; }
    public string? Role { get; set; }
    [Required]
    public DateTime RegistrationDate { get; set; }
    public DateTime? InactivationDate { get; set; }
}
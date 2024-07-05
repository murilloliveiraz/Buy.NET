namespace Buy_NET.API.Contracts.Product;

public class ProductResponseContract : ProductRequestContract
{
    public DateTime RegistrationDate { get; set; }
    public DateTime? InactivationDate { get; set; }    
}
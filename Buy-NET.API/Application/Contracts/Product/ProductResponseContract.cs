namespace Buy_NET.API.Contracts.Product;

public class ProductResponseContract : ProductRequestContract
{
    public long Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? InactivationDate { get; set; }    
}
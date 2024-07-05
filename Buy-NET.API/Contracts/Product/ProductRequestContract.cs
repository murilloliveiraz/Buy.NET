namespace Buy_NET.API.Contracts.Product;

public class ProductRequestContract
{
    public string? Name {get; set; } = string.Empty;   
    public string? Description {get; set; }  = string.Empty;
    public double? Price {get; set; }    
    public int? StockQuantity {get; set; }  
    public long CategoryId {get; set; }
}
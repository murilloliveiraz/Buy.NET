namespace Buy_NET.API.Contracts.OrderItem;

public class OrderItemResponseContract
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
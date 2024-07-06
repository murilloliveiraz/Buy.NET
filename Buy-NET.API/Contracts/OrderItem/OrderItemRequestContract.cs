namespace Buy_NET.API.Contracts.OrderItem;

public class OrderItemRequestContract
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
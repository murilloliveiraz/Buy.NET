using Buy_NET.API.Contracts.OrderItem;

namespace Buy_NET.API.Contracts.Order;

public class OrderResponseContract
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public double Total { get; set; }
    public List<OrderItemResponseContract> Items { get; set; } = new List<OrderItemResponseContract>();
}

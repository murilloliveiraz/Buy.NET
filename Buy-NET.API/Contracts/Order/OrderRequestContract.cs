using Buy_NET.API.Contracts.OrderItem;

namespace Buy_NET.API.Contracts.Order;

public class OrderRequestContract
{
    public int CustomerId { get; set; }
    public List<OrderItemRequestContract> Items { get; set; }
}

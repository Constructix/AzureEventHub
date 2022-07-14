public class Order
{
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public List<SubOrder> SubOrders { get; set; } = new List<SubOrder>();
}
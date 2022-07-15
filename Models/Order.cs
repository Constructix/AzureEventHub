public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public List<SubOrder> SubOrders { get; set; } = new List<SubOrder>();
}
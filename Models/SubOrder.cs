public class SubOrder
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public Customer Customer { get; set; }
    public bool IsOrderPickUp { get; set; }
    public Address DeliveryAddress { get; set; }
}
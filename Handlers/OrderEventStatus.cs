namespace EventTopicDemo.Handlers;

public class OrderEventStatus
{
    private string Status { get; init; }
    public override string ToString() => this.Status;

    public static OrderEventStatus Created => new OrderEventStatus() { Status = "Created" };
    public static OrderEventStatus Updated => new OrderEventStatus() { Status = "Updated" };
    public static OrderEventStatus Cancelled => new OrderEventStatus() { Status = "Cancelled" };


    public static implicit operator string(OrderEventStatus orderEventStatus) => orderEventStatus.Status; 
    public static explicit operator OrderEventStatus(string status)
    {
        switch (status.ToUpperInvariant())
        {
            case "CREATED":
                return Created;
            case "UPDATED":
                return Updated;
            case "CANCELLED":
                return Cancelled;
        }
        return null;
    }
}
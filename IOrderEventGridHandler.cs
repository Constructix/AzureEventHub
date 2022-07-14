namespace EventTopicDemo;

public interface IOrderEventGridHandler
{
    Task<SendOrderEventResponse> SendCreateOrderEventAsync(CreateOrderEvent createOrderEvent);
    Task<SendOrderEventResponse> SendUpdateOrderEventAsync(UpdateOrderEvent updateOrderEvent);
    Task<SendOrderEventResponse> SendCancelOrderEventAsync(CancelOrderEvent cancelOrderEvent);
}


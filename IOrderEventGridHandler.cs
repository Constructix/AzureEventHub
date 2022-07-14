namespace EventTopicDemo;

public interface IOrderEventGridHandler
{
    Task<SendOrderEventResponse> SendEventAsync(SendOrderEvent sendOrderEvent);
}
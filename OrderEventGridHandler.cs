using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Options;

namespace EventTopicDemo;

public class OrderEventGridHandler : IOrderEventGridHandler
{
    private const string EventSubject = "OrderEvents";
    private const string Version = "1.0";
    private readonly IOptions<EventGridSettings> _options;
    private readonly EventGridPublisherClient _client;
    public OrderEventGridHandler(IOptions<EventGridSettings> options)
    {
        _options = options;
        _client= new EventGridPublisherClient(new Uri(_options.Value.Endpoint), new AzureKeyCredential(_options.Value.AccessKey));
    }
    public async Task<SendOrderEventResponse> SendCreateOrderEventAsync(CreateOrderEvent createOrderEvent)
    {
        return await SendData(OrderEventStatus.Created,  BinarizeData(createOrderEvent));
    }

    public async Task<SendOrderEventResponse> SendUpdateOrderEventAsync(UpdateOrderEvent updateOrderEvent)
    {
        return await SendData(OrderEventStatus.Updated,  BinarizeData(updateOrderEvent));
    }

    public async Task<SendOrderEventResponse> SendCancelOrderEventAsync(CancelOrderEvent cancelOrderEvent)
    {
        return await SendData(OrderEventStatus.Cancelled,  BinarizeData(cancelOrderEvent));
    }

    private async Task<SendOrderEventResponse> SendData(string eventType, BinaryData data)
    {
        var eventGridEvent = new EventGridEvent(EventSubject, eventType, Version, data);
        var response =  await _client.SendEventAsync(eventGridEvent);
        var status = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.Status.ToString());
        return new SendOrderEventResponse()
        {
            Status = status 
        };
    }

    private static BinaryData BinarizeData(OrderCommand createOrderEvent)
    {
        var serializationOptions = new JsonSerializerOptions() { WriteIndented = true };
        var binaryData = new BinaryData(createOrderEvent.Order, serializationOptions);
        return binaryData;
    }
}
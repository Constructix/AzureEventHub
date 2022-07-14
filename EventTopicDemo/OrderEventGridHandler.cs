using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Options;

namespace EventTopicDemo;

public class 
    OrderEventGridHandler : IOrderEventGridHandler
{
 
    private IOptions<EventGridSettings> _options;

    public OrderEventGridHandler(IOptions<EventGridSettings> options)
    {
        _options = options;
    }
    public async Task<SendOrderEventResponse> SendEventAsync( SendOrderEvent sendOrderEvent)
    {
        EventGridPublisherClient client = new EventGridPublisherClient(new Uri(_options.Value.Endpoint),
            new AzureKeyCredential(_options.Value.AccessKey));
        
        var serializationOptions = new JsonSerializerOptions() { WriteIndented = true };
        var binaryData = new BinaryData(sendOrderEvent.Order, serializationOptions);
        var eventGridEvent = new EventGridEvent("ExampleEventGridSubject", "OrderCreated", "1.0", binaryData);
        var response =  await client.SendEventAsync(eventGridEvent);
        Console.WriteLine("Sent Event off to EventGrid Demo");
        var s = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.Status.ToString());
        return new SendOrderEventResponse()
        {
            Status =s 
        };
    }    
}
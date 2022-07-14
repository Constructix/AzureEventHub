using System.Net;
using System.Text.Json;
using EventTopicDemo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var svc = new ServiceCollection()
    .Configure<EventGridSettings>(config.GetSection("EventGridSettings"))
    .AddScoped<IOrderEventGridHandler, OrderEventGridHandler>()
    .BuildServiceProvider();

var handler = svc.GetService<IOrderEventGridHandler>();
if (handler == null)
{
    Console.WriteLine(" --------- NULL IS DEFINED ---------");
}

var order = new Order();

var sendOrderEvent = new CreateOrderEvent()
 {
     Order = order
 };
 var status  = await handler.SendCreateOrderEventAsync(sendOrderEvent);
 Console.WriteLine(JsonSerializer.Serialize(status));
status = await handler.SendCancelOrderEventAsync(new CancelOrderEvent() { Order = order });
Console.WriteLine(JsonSerializer.Serialize(status));
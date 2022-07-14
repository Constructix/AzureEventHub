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

var sendOrderEvent = new SendOrderEvent()
 {
     Order = new Order()
 };
 var status  = await handler.SendEventAsync(sendOrderEvent);
 Console.WriteLine(JsonSerializer.Serialize(status));
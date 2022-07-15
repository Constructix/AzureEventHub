using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EventTopicDemo;
using EventTopicDemo.Handlers;
using EventTopicDemo.Validation;
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

var order = new Order { SubOrders = new List<SubOrder> { 
    new SubOrder() { 
        IsOrderPickUp = true,
       // DeliveryAddress = new Address() { StreetName = "11 Carbine Court", Postcode = "4306", Suburb = "Karalee", State = "Qld"},
        Customer = new Customer() { 
            FirstName = "Richard", LastName = "Jones", Address = new Address()
    {
    StreetName = "11 Carbine Court", Suburb = "Karalee", Postcode = "4306", State = "QLD"
}}}}};


// validate the order...
var validation = new OrderValidator();
var validationResult = validation.Validate(order);
Console.WriteLine($"Validation Result: {validationResult.IsValid}");
if (!validationResult.IsValid)
{
    foreach (var currentError in validationResult.Errors)
    {
        Console.WriteLine(currentError.ErrorMessage);
    }
}
    var missingDeliveryAddressesInOrder = order.SubOrders.Exists(x => x.IsOrderPickUp && x.DeliveryAddress == null);

Console.WriteLine($"MissingDeliveryAddressesInOrder Result: {missingDeliveryAddressesInOrder }");
if (!missingDeliveryAddressesInOrder)
{
    var sendOrderEvent = new CreateOrderEvent()
    {
        Order = order
    };
    var status = await handler.SendCreateOrderEventAsync(sendOrderEvent);
    Console.WriteLine(JsonSerializer.Serialize(status));
    status = await handler.SendCancelOrderEventAsync(new CancelOrderEvent() { Order = order });
    Console.WriteLine(JsonSerializer.Serialize(status));
}
else
    Console.WriteLine("Missing Delivery Address. - Did not send onto azure topic.");
using System.Text.Json;
using EventTopicDemo;
using EventTopicDemo.Handlers;
using EventTopicDemo.Validation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = BuildConfiguration();

var svc = AddServices(config);
var handler = svc.GetService<IOrderEventGridHandler>();
var order = BuildOrder();
var validationResult = SetupValidation(order);

if (!validationResult.IsValid)
{
    foreach (var currentError in validationResult.Errors)
        Console.WriteLine(currentError.ErrorMessage);
}
else
{
    var sendOrderEvent = new CreateOrderEvent() { Order = order };
    var status = await handler.SendCreateOrderEventAsync(sendOrderEvent);
    Console.WriteLine(JsonSerializer.Serialize(status));
    status = await handler.SendCancelOrderEventAsync(new CancelOrderEvent() { Order = order });
    Console.WriteLine(JsonSerializer.Serialize(status));
}

Order BuildOrder()
{
    var order1 = new Order
    {
        SubOrders = new List<SubOrder>
        {
            new SubOrder()
            {
                IsOrderPickUp = true,
                DeliveryAddress = new Address() { StreetName = "11 Carbine Court", Postcode = "4306", Suburb = "Karalee", State = "Qld"},
                Customer = new Customer()
                {
                    FirstName = "Richard", LastName = "Jones", Address = new Address()
                    {
                        StreetName = "11 Carbine Court", Suburb = "Karalee", Postcode = "4306", State = "QLD"
                    }
                }
            }
        }
    };
    return order1;
}

ValidationResult SetupValidation(Order order2)
{
    var validation = new OrderValidator();
    var validationResult1 = validation.Validate(order2);
    return validationResult1;
}

IConfigurationRoot BuildConfiguration()
{
    IConfigurationRoot configurationRoot = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();
    return configurationRoot;
}

ServiceProvider AddServices(IConfigurationRoot config1)
{
    var serviceProvider = new ServiceCollection()
        .Configure<EventGridSettings>(config1.GetSection("EventGridSettings"))
        .AddScoped<IOrderEventGridHandler, OrderEventGridHandler>()
        .BuildServiceProvider();
    return serviceProvider;
}
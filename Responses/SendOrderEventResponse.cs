using System.Net;

namespace EventTopicDemo.Responses;

public class SendOrderEventResponse
{
    public HttpStatusCode Status { get; set; }
}
using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Domain.Payments;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    ProcessingStarted = 0,
    Successful = 1,
    Rejected = 2,
    Failed = 3
}

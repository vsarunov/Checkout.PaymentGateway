using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Domain.Payments;

public record PaymentProcessingResult
{
    [JsonConstructor]
    public PaymentProcessingResult(Status paymentStatus)
    {
        PaymentStatus = paymentStatus;
    }

    public Status PaymentStatus { get; init; }
}

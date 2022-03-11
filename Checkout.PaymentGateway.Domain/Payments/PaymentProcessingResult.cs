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

    public bool IsRejected() => PaymentStatus == Status.Rejected;
    public bool IsFailed() => PaymentStatus == Status.Failed;
}

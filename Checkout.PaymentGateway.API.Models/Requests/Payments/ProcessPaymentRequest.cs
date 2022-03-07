using Checkout.PaymentGateway.API.Models.Shared.Payments;

namespace Checkout.PaymentGateway.API.Models.Requests.Payments;

public class ProcessPaymentRequest
{
    public PaymentId Id { get; init; }
    public CardDto CardDetails { get; init; }
    public PaymentDto Value { get; init; }
    public TransactionTimeStampDto TransactionTimeStamp { get; init; }
}
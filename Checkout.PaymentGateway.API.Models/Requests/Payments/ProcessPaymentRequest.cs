using Checkout.PaymentGateway.API.Models.Shared.Payments;

namespace Checkout.PaymentGateway.API.Models.Requests.Payments;

public record struct ProcessPaymentRequest(PaymentId Id, CardDto CardDetails, PaymentDto Value, TransactionTimeStampDto TransactionTimeStamp);
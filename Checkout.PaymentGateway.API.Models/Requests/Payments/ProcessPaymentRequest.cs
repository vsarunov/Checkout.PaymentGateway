using Checkout.PaymentGateway.API.Models.Shared.Constants;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using FastEndpoints;

namespace Checkout.PaymentGateway.API.Models.Requests.Payments;

public class ProcessPaymentRequest
{
    public PaymentId Id { get; init; }
    public PayerDto Payer { get; init; }
    public PaymentDto Value { get; init; }
    public MerchantDto Merchant { get; init; }
    public TransactionTimeStampDto TransactionTimeStamp { get; init; }

    //[FromHeader(Headers.DefaultCorrelationIdHeaderName)]
    //public Guid CorrelationId { get; init; }
}
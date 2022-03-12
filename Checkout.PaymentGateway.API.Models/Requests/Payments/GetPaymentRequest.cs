using Checkout.PaymentGateway.API.Models.Shared.Constants;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using FastEndpoints;

namespace Checkout.PaymentGateway.API.Models.Requests.Payments;

public record GetPaymentRequest
{
    public Guid Id { get; init; }

    [FromHeader(Headers.DefaultCorrelationIdHeaderName)] 
    public Guid CorrelationId { get; init; }
}


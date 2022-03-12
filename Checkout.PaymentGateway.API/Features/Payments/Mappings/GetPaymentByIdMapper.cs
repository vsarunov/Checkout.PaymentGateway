using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Responses.Payments;
using Checkout.PaymentGateway.Application.QueryHandlers.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;

namespace Checkout.PaymentGateway.API.Features.Payments.Mappings
{
    internal static class GetPaymentByIdMapper
    {
        internal static GetPaymentByIdQuery ToQuery(this GetPaymentRequest request) =>
            GetPaymentByIdQuery.Create(new CQRS.Models.Payments.PaymentId(request.Id), request.CorrelationId);

        internal static PaymentResponse ToResponse(this PaymentRoot payment)
        {
            return new PaymentResponse(
                    new Models.Shared.Payments.CardDto(
                        new string(payment.Payer.Card.Number.Select((p, index) => index <= 12 ? '*' : p).ToArray()),
                        payment.Payer.Card.CVV,
                        new Models.Shared.Payments.CardExpirationDateDto(payment.Payer.Card.Expiration.Year, payment.Payer.Card.Expiration.Month)
                        ),
                    new Models.Shared.Payments.PaymentDto(payment.Value.Amount, payment.Value.ISOCurrencyCode),
                    new PaymentStatusDto(payment.Status.ToString())
                );
        }
    }
}

using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Application.CommandHandlers.Payments;

namespace Checkout.PaymentGateway.API.Features.Payments.Mappings
{
    internal static class ProcessPaymentMapper
    {
        internal static ProcessPaymentCommand ToCommand(this ProcessPaymentRequest request) =>
                ProcessPaymentCommand.Create(
                    Guid.NewGuid(),//request.CorrelationId,
                    new CQRS.Models.Payments.PaymentId(request.Id.Value),
                    new CQRS.Models.Payments.Payer(
                        new CQRS.Models.Payments.Address(request.Payer.Address.AddressLine, request.Payer.Address.PostCode),
                        new CQRS.Models.Payments.Name(request.Payer.Name.FirstName, request.Payer.Name.LastName, request.Payer.Name.MiddleName),
                        new CQRS.Models.Payments.Card(request.Payer.Card.Number, request.Payer.Card.CVV, new CQRS.Models.Payments.CardExpirationDate(request.Payer.Card.Expiration.Year, request.Payer.Card.Expiration.Month))
                        ),
                    new CQRS.Models.Payments.Payment(request.Value.Amount, request.Value.ISOCurrencyCode),
                    new CQRS.Models.Payments.Merchant(
                        new CQRS.Models.Payments.MerchantId(request.Merchant.Id.Value),
                        new CQRS.Models.Payments.Card(request.Merchant.Card.Number, request.Merchant.Card.CVV, new CQRS.Models.Payments.CardExpirationDate(request.Merchant.Card.Expiration.Year, request.Merchant.Card.Expiration.Month))
                        ),
                    new CQRS.Models.Payments.TransactionTimeStamp(request.TransactionTimeStamp.TimeStamp)
                );
    }
}

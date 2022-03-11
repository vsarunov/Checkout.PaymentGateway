using Checkout.PaymentGateway.Domain.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments.Mappers
{
    internal static class DomainMapper
    {
        internal static PaymentRoot ToDomainPayment(this ProcessPaymentCommand command) =>
                PaymentRoot.Create(
                    new PaymentId(command.Id.Value),
                    new Payer(
                        new Address(command.Payer.Address.AddressLine, command.Payer.Address.PostCode),
                        new Name(command.Payer.Name.FirstName, command.Payer.Name.LastName, command.Payer.Name.MiddleName),
                        new Card(command.Payer.Card.Number, command.Payer.Card.CVV, new CardExpirationDate(command.Payer.Card.Expiration.Year, command.Payer.Card.Expiration.Month))
                        ),
                    new Merchant(
                        new MerchantId(command.Merchant.Id.Value),
                        new Card(command.Merchant.Card.Number, command.Merchant.Card.CVV, new CardExpirationDate(command.Merchant.Card.Expiration.Year, command.Merchant.Card.Expiration.Month))
                        ),
                    new Payment(command.Value.Amount, command.Value.ISOCurrencyCode),
                    new TransactionTimeStamp(command.TransactionTimeStamp.TimeStamp),
                    command.CorrelationId
                );       
    }
}

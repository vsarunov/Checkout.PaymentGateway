using Checkout.AcquiringBank.Models;

namespace Checkout.AcquiringBank.Mappers
{
    internal static class PaymentMapper
    {
        internal static  PaymentRoot ToBankPayment(this PaymentGateway.Domain.Payments.Aggregates.PaymentRoot payment) =>
        PaymentRoot.Create(
            payment.Id.Value,
            payment.Payer.MapToBankPayer(),
            payment.Merchant.MapToBankMerchant(),
            payment.Value.MapToPaymentDetails(),
            payment.TimeStamp.MapToPaymentTransactionTime(),
            payment.CorrelationId
            );

        internal static Payer MapToBankPayer(this PaymentGateway.Domain.Payments.Payer payer) =>
            new Payer(payer.Address.MapToAddress(), payer.Name.MapToName(), payer.Card.MapToBankCardDetails());

        internal static Merchant MapToBankMerchant(this PaymentGateway.Domain.Payments.Merchant merchant) =>
            new Merchant(merchant.Card.MapToBankCardDetails());

        internal static Name MapToName(this PaymentGateway.Domain.Payments.Name name) =>
            new Name(name.FirstName, name.LastName, name.MiddleName);

        internal static Address MapToAddress(this PaymentGateway.Domain.Payments.Address address) =>
            new Address(address.AddressLine, address.PostCode);

        internal static Card MapToBankCardDetails(this PaymentGateway.Domain.Payments.Card card) =>
            new Card(card.Number, card.CVV, card.Expiration.MapToCardExpiration());

        internal static CardExpirationDate MapToCardExpiration(this PaymentGateway.Domain.Payments.CardExpirationDate cardExpirationDate) =>
            new CardExpirationDate(cardExpirationDate.Year, cardExpirationDate.Month);

        internal static Payment MapToPaymentDetails(this PaymentGateway.Domain.Payments.Payment payment) =>
            new Payment(payment.Amount, payment.ISOCurrencyCode);

        internal static TransactionTimeStamp MapToPaymentTransactionTime(this PaymentGateway.Domain.Payments.TransactionTimeStamp transactionTimeStamp) =>
            new TransactionTimeStamp(transactionTimeStamp.TimeStamp);
    }
}

using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using Checkout.PaymentGateway.Domain.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;

namespace Checkout.PaymentGateway.Tests.Shared.Mappers;

public static class PaymentResponseToDomainMapper
{
    public static PaymentRoot MapRequestToDomain(this ProcessPaymentRequest paymentRequest) =>
            PaymentRoot.Create(
                new Domain.Payments.PaymentId(paymentRequest.Id.Value),
                paymentRequest.Payer.MapRequestPayerToDomain(),
                paymentRequest.Merchant.MapRequestMerchantToDomain(),
                paymentRequest.Value.MapRequestPaymentToDomain(),
                paymentRequest.TransactionTimeStamp.MapRequestTransactionTimeStampToDomain(),
                Guid.NewGuid()
                );

    public static Payer MapRequestPayerToDomain(this PayerDto payerDto) =>
        new Payer(payerDto.Address.MapRequestAddressToDomain(), payerDto.Name.MapRequestNameToDomain(), payerDto.Card.MapRequestCardToDomain());

    public static Merchant MapRequestMerchantToDomain(this MerchantDto merchantDto) =>
        new Merchant(new Domain.Payments.MerchantId(merchantDto.Id.Value), merchantDto.Card.MapRequestCardToDomain());

    public static Name MapRequestNameToDomain(this NameDto nameDto) =>
        new Name(nameDto.FirstName, nameDto.LastName, nameDto.MiddleName);

    public static Domain.Payments.Address MapRequestAddressToDomain(this API.Models.Shared.Payments.AddressDto addressDto) =>
        new Domain.Payments.Address(addressDto.AddressLine, addressDto.PostCode);

    public static Domain.Payments.Card MapRequestCardToDomain(this API.Models.Shared.Payments.CardDto cardDto) =>
        new Domain.Payments.Card(cardDto.Number, cardDto.CVV, cardDto.Expiration.MapRequestCardExpirationToDomain());

    public static CardExpirationDate MapRequestCardExpirationToDomain(this CardExpirationDateDto cardExpirationDateDto) =>
        new CardExpirationDate(cardExpirationDateDto.Year, cardExpirationDateDto.Month);

    public static Payment MapRequestPaymentToDomain(this PaymentDto paymentDto) =>
        new Payment(paymentDto.Amount, paymentDto.ISOCurrencyCode);

    public static TransactionTimeStamp MapRequestTransactionTimeStampToDomain(this TransactionTimeStampDto transactionTimeStampDto) =>
        new TransactionTimeStamp(transactionTimeStampDto.TimeStamp);
}

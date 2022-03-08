using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using System;

namespace Checkout.PaymentGateway.API.ComponentTests.Mappers;

internal static class PaymentMapper
{
    internal static Domain.Payments.Aggregates.PaymentRoot MapRequestToDomain(this ProcessPaymentRequest paymentRequest) =>
            Domain.Payments.Aggregates.PaymentRoot.Create(
                new Domain.Payments.PaymentId(paymentRequest.Id.Value),
                paymentRequest.Payer.MapRequestPayerToDomain(),
                paymentRequest.Merchant.MapRequestMerchantToDomain(),
                paymentRequest.Value.MapRequestPaymentToDomain(),
                paymentRequest.TransactionTimeStamp.MapRequestTransactionTimeStampToDomain(),
                Guid.NewGuid()
                );

    internal static Domain.Payments.Payer MapRequestPayerToDomain(this PayerDto payerDto) =>
        new Domain.Payments.Payer(payerDto.Address.MapRequestAddressToDomain(), payerDto.Name.MapRequestNameToDomain(), payerDto.Card.MapRequestCardToDomain());

    internal static Domain.Payments.Merchant MapRequestMerchantToDomain(this MerchantDto merchantDto) =>
        new Domain.Payments.Merchant(new Domain.Payments.MerchantId(merchantDto.Id.Value), merchantDto.Card.MapRequestCardToDomain());

    internal static Domain.Payments.Name MapRequestNameToDomain(this NameDto nameDto) =>
        new Domain.Payments.Name(nameDto.FirstName, nameDto.LastName, nameDto.MiddleName);

    internal static Domain.Payments.Address MapRequestAddressToDomain(this AddressDto addressDto) =>
        new Domain.Payments.Address(addressDto.AddressLine, addressDto.PostCode);

    internal static Domain.Payments.Card MapRequestCardToDomain(this CardDto cardDto) =>
        new Domain.Payments.Card(cardDto.Number, cardDto.CVV, cardDto.Expiration.MapRequestCardExpirationToDomain());

    internal static Domain.Payments.CardExpirationDate MapRequestCardExpirationToDomain(this CardExpirationDateDto cardExpirationDateDto) =>
        new Domain.Payments.CardExpirationDate(cardExpirationDateDto.Year, cardExpirationDateDto.Month);

    internal static Domain.Payments.Payment MapRequestPaymentToDomain(this PaymentDto paymentDto) =>
        new Domain.Payments.Payment(paymentDto.Amount, paymentDto.ISOCurrencyCode);

    internal static Domain.Payments.TransactionTimeStamp MapRequestTransactionTimeStampToDomain(this TransactionTimeStampDto transactionTimeStampDto) =>
        new Domain.Payments.TransactionTimeStamp(transactionTimeStampDto.TimeStamp);
}

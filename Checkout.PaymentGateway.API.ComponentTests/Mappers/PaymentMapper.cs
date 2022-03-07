using Checkout.PaymentGateway.API.Models.Requests.Payments;
using System;

namespace Checkout.PaymentGateway.API.ComponentTests.Mappers;

internal static class PaymentMapper
{
    internal static Domain.Payments.Aggregates.Payment MapRequestToDomain(this ProcessPaymentRequest paymentRequest) =>
            Domain.Payments.Aggregates.Payment.Create(
                new Domain.Payments.PaymentId(paymentRequest.Id.Value),
                new Domain.Payments.Payment(paymentRequest.Value.Amount, paymentRequest.Value.ISOCurrencyCode),
                new Domain.Payments.Card(paymentRequest.CardDetails.Number, paymentRequest.CardDetails.CVV, new Domain.Payments.CardExpirationDate(paymentRequest.CardDetails.Expiration.Year, paymentRequest.CardDetails.Expiration.Month)),
                new Domain.Payments.TransactionTimeStamp(paymentRequest.TransactionTimeStamp.TimeStamp),
                Guid.NewGuid()
                );

}

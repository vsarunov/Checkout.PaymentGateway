using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using System;

namespace Checkout.PaymentGateway.API.ComponentTests.Shared
{
    internal class ProcessPaymentFactory
    {
        internal static ProcessPaymentRequest CreateDefaultPaymentRequest() =>
            new ProcessPaymentRequest()
            {
                Id = new PaymentId(Guid.NewGuid()),
                CardDetails = new CardDto("378282246310005", 451, new CardExpirationDateDto(DateTime.UtcNow.Year, DateTime.UtcNow.AddMonths(3).Month)),
                Value = new PaymentDto(6500.00m,"USD"),
                TransactionTimeStamp = new TransactionTimeStampDto(DateTime.UtcNow)
            };

        internal static ProcessPaymentRequest CreatePaymentRequest(
            PaymentId id,
            CardDto card,
            PaymentDto value,
            TransactionTimeStampDto transactionTimeStamp) =>
           new ProcessPaymentRequest()
           {
               Id = id,
               CardDetails = card,
               Value = value,
               TransactionTimeStamp = transactionTimeStamp
           };

        internal static CardDto CreateCardDto(int cvv, string number, CardExpirationDateDto expiration) =>
            new CardDto(number, cvv, expiration);

        internal static CardExpirationDateDto CreateExpirationDateDto(int year, int month) =>
            new CardExpirationDateDto(year,month);

        internal static PaymentDto CreateMoneyDto(decimal amount, string iso) =>
            new PaymentDto(amount,iso);
    }
}

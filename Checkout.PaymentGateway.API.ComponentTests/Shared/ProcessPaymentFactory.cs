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
                CardDetails = new CardDto
                {
                    CVV = 451,
                    Number = "6476330754325643",
                    Expiration = new CardExpirationDateDto
                    {
                        Month = DateTime.UtcNow.AddMonths(3).Month,
                        Year = DateTime.UtcNow.AddYears(3).Year,
                    }
                },
                Value = new PaymentDto
                {
                    Amount = 6500.00m,
                    ISOCurrencyCode = "USD"
                },
                TransactionTimeStamp = new TransactionTimeStampDto
                {
                    TimeStamp = DateTime.UtcNow
                }
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
            new CardDto
            {
                CVV = cvv,
                Number = number,
                Expiration = expiration
            };

        internal static CardExpirationDateDto CreateExpirationDateDto(int year, int month) =>
            new CardExpirationDateDto
            {
                Month = month,
                Year = year,
            };

        internal static PaymentDto CreateMoneyDto(decimal amount, string iso) =>
            new PaymentDto
            {
                Amount = amount,
                ISOCurrencyCode = iso
            };
    }
}

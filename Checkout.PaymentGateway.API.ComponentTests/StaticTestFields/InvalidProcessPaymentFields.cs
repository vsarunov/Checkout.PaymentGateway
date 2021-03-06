using AutoFixture;
using Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using System;
using Xunit;

namespace Checkout.PaymentGateway.API.ComponentTests.StaticTestFields
{
    internal static class InvalidProcessPaymentFields
    {
        internal static IFixture fixture = CustomAutoDataAttribute.Customizations();

        internal static TheoryData<PaymentFixture, ProcessPaymentRequest, string, object> GetInvalidProcessPaymentFields() =>
            new TheoryData<PaymentFixture, ProcessPaymentRequest, string, object>
            {
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address.AddressLine), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address.AddressLine), string.Empty },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address.AddressLine), "  " },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address.PostCode), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address.PostCode), string.Empty },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Address.PostCode), " " },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name.FirstName), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name.FirstName), string.Empty },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name.FirstName), "   " },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name.LastName), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name.LastName), string.Empty },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Name.LastName), " " },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.CVV), 0 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.CVV), -1 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.CVV), 98 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.CVV), 10001 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Number), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Number), string.Empty },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Number), " " },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Month), -1 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Month), 0 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Month), 13 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Month), DateTime.UtcNow.AddMonths(-1).Month },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Year), -1 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Year), 0 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Payer.Card.Expiration.Year), DateTime.UtcNow.AddYears(-1).Year },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.CVV), 0 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.CVV), -1 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.CVV), 98 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.CVV), 10001 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Number), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Number), string.Empty },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Number), " " },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Month), -1 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Month), 0 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Month), 13 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Month), DateTime.UtcNow.AddMonths(-1).Month },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Year), -1 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Year), 0 },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Merchant.Card.Expiration.Year), DateTime.UtcNow.AddYears(-1).Year },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.Amount), -1m },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.Amount), 0m },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.Amount), 0.09m },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.Amount), 10001.0m},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.Amount), 10000.09m},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.Amount), 10000.1m},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.ISOCurrencyCode), null},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.ISOCurrencyCode), string.Empty},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.ISOCurrencyCode), "  "},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.Value.ISOCurrencyCode), "randomString"},
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp), null },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp), DateTime.UtcNow.AddDays(-1) },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp), DateTime.UtcNow.AddDays(1) },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp), default(DateTime) },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp), DateTime.MinValue },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp), DateTime.MaxValue },
                { fixture.Create<PaymentFixture>(),fixture.Create<ProcessPaymentRequest>(), nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp), DateTime.UtcNow.Date }
            };

    }
}

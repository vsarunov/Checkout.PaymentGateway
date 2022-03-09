using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;

namespace Checkout.PaymentGateway.Tests.Shared.ModelFactories
{
    public class ProcessPaymentRequestFactory
    {
        public static ProcessPaymentRequest CreateDefaultPaymentRequest() =>
            new ProcessPaymentRequest()
            {
                Id = new PaymentId(Guid.NewGuid()),
                Payer = new PayerDto(new AddressDto("19 Barn lea", "SJ178SE"), new NameDto("Tom", "Riddle", null), new CardDto("378282246310005", 451, new CardExpirationDateDto(DateTime.UtcNow.Year, DateTime.UtcNow.AddMonths(3).Month))),
                Merchant = new MerchantDto(new MerchantId(Guid.NewGuid()), new CardDto("378282246310005", 632, new CardExpirationDateDto(DateTime.UtcNow.Year, DateTime.UtcNow.AddMonths(5).Month))),
                Value = new PaymentDto(6500.00m, "USD"),
                TransactionTimeStamp = new TransactionTimeStampDto(DateTime.UtcNow)
            };
    }
}

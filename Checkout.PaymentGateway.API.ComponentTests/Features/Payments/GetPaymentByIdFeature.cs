using Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Xbehave;
using Xunit;

namespace Checkout.PaymentGateway.API.ComponentTests.Features.Payments
{
    [Trait("TestType", "ComponentTests")]
    public class GetPaymentByIdFeature
    {
        [Scenario, CustomAutoData]
        public void GetPaymentById(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment exists".x(() => fixture.GivenPaymentExists(request));
            "When I try to get payment by id".x(() => fixture.WhenIGetExistingPaymentById());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the payment details match expected".x(fixture.ThenPaymentIsAsExpected);
        }

        [Scenario, CustomAutoData]
        public void GetNonExistingPayment(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a valid non existing payment".x(() => fixture.GivenAValidPayment(request));
            "When I try to get payment by id".x(() => fixture.WhenIGetExistingPaymentById());
            "Then a bad request response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
        }
    }
}

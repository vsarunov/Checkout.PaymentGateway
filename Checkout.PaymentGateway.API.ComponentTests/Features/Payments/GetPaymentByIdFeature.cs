using Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Common.Enums;
using Xbehave;
using Xunit;

namespace Checkout.PaymentGateway.API.ComponentTests.Features.Payments
{
    [Trait("TestType", "ComponentTests")]
    public class GetPaymentByIdFeature
    {
        [Scenario, CustomAutoData]
        public void GetSuccessfulPaymentById(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment exists".x(() => fixture.GivenPaymentExists(request, Domain.Payments.Status.Successful));
            "When I try to get payment by id".x(() => fixture.WhenIGetExistingPaymentById());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the payment details match expected".x(x => fixture.ThenPaymentIsAsExpected(Domain.Payments.Status.Successful));
            "And the payment was read from the database".x(fixture.ThenPaymentRepositoryWasCalled);
        }

        [Scenario, CustomAutoData]
        public void GetRejectedPaymentById(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment exists".x(() => fixture.GivenPaymentExists(request, Domain.Payments.Status.Rejected));
            "When I try to get payment by id".x(() => fixture.WhenIGetExistingPaymentById());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the payment details match expected".x(x => fixture.ThenPaymentIsAsExpected(Domain.Payments.Status.Rejected));
            "And the payment was read from the database".x(fixture.ThenPaymentRepositoryWasCalled);
        }

        [Scenario, CustomAutoData]
        public void GetFailedPaymentById(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment exists".x(() => fixture.GivenPaymentExists(request, Domain.Payments.Status.Failed));
            "When I try to get payment by id".x(() => fixture.WhenIGetExistingPaymentById());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And the payment details match expected".x(x => fixture.ThenPaymentIsAsExpected(Domain.Payments.Status.Failed));
            "And the payment was read from the database".x(fixture.ThenPaymentRepositoryWasCalled);
        }

        [Scenario, CustomAutoData]
        public void GetNonExistingPayment(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a valid non existing payment".x(() => fixture.GivenAValidPayment(request));
            "When I try to get payment by id".x(() => fixture.WhenIGetExistingPaymentById());
            "Then a bad request response is returned".x(fixture.ThenANotFoundResponseIsReturned);
            "And the payment was read from the database".x(fixture.ThenPaymentRepositoryWasCalled);
            "And the response contains Payment Not found failure".x(x => fixture.ThenResponseIsFailureWithErrorCodeMessage(ErrorCode.PaymentNotFound));
        }
    }
}

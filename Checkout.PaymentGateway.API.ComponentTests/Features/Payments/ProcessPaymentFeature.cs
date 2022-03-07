using Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using Checkout.PaymentGateway.API.ComponentTests.StaticTestFields;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Xbehave;
using Xunit;

namespace Checkout.PaymentGateway.API.ComponentTests.Features.Payments
{
    [Trait("TestType", "ComponentTests")]
    public class ProcessPaymentFeature
    {
        [Scenario, CustomAutoData]
        public void ProcessValidPayment(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a valid payment".x(() => fixture.GivenAValidPayment(request));
            "And given bank accepts payment".x(() => fixture.GivenBankAcceptsPayment());
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And then the payment was submitted to bank successfully".x(fixture.ThenPaymentWasSubmittedToBankSuccessfully);
            "And then the payment was store".x(fixture.ThenPaymentWasStored);
        }

        [Scenario]
        [MemberData(nameof(GetInvalidProcessPaymentFields))]
        public void ProcessInvalidPayment(PaymentFixture fixture, ProcessPaymentRequest request, string fieldName, object value)
        {
            "Given a payment request".x(() => fixture.GivenAValidPayment(request));
            $"And given {fieldName} has invalid value".x(() => fixture.GivenAFieldIsSet(fieldName, value));
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a bad request response is returned".x(fixture.ThenABadRequestResponseIsReturned);
            "And the error property is returned".x(x=>fixture.ThenAnErrorPropertyIsReturned(fieldName));
        }

        [Scenario, CustomAutoData]
        public void PaymentRejectedByBank(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment request".x(() => fixture.GivenAValidPayment(request));
            "And given the bank reject transaction".x(() => fixture.GivenBankRejectsTransactions());
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a bad request response is returned".x(fixture.ThenABadRequestResponseIsReturned);
        }

        [Scenario, CustomAutoData]
        public void PaymentSendingFailed(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment request".x(() => fixture.GivenAValidPayment(request));
            "And given the bank request is failing".x(() => fixture.GivenBankRequestIsFailing());
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a service unavailable error is returned".x(fixture.ThenAServiceUnavailableIsReturned);
            "And payment is not stored".x(fixture.ThenPaymentIsNotStored);
        }

        [Scenario, CustomAutoData]
        public void PaymentSuccessfulStorageOperationFailed(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment request".x(() => fixture.GivenAValidPayment(request));
            "And given storage fails".x(() => fixture.GivenStorageFailure());
            "And given bank accepts payment".x(() => fixture.GivenBankAcceptsPayment());
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a service unavailable error is returned".x(fixture.ThenAServiceUnavailableIsReturned);
        }

        [Scenario, CustomAutoData]
        public void PaymentAlreadyExists(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment exists".x(() => fixture.GivenPaymentAlreadyExists(request));
            "When I submit the payment again".x(() => fixture.WhenISubmitThePayment());
            "Then a bad request response is returned".x(fixture.ThenABadRequestResponseIsReturned);
        }

        public static TheoryData<PaymentFixture, ProcessPaymentRequest, string, object> GetInvalidProcessPaymentFields() => InvalidProcessPaymentFields.GetInvalidProcessPaymentFields();
    }
}

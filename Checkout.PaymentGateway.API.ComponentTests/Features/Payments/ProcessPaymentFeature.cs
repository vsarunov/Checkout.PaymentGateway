using Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using Checkout.PaymentGateway.API.ComponentTests.StaticTestFields;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Common.Enums;
using Checkout.PaymentGateway.Domain.Payments;
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
            "And then the payment was stored".x(fixture.ThenPaymentWasStored);
            "And then the payment was searched for".x(fixture.ThenThePaymentWasSearchedFor);
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
            "And payment is stored".x(fixture.ThenPaymentIsStored);
            "And the response contains Payment rejected failure".x(x => fixture.ThenResponseIsFailureWithErrorCodeMessage(ErrorCode.PaymentRejected));
            "And then the payment was searched for".x(fixture.ThenThePaymentWasSearchedFor);
        }

        [Scenario, CustomAutoData]
        public void PaymentSendingFailed(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a payment request".x(() => fixture.GivenAValidPayment(request));
            "And given the bank request is failing".x(() => fixture.GivenBankRequestIsFailing());
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a service unavailable error is returned".x(fixture.ThenAServiceUnavailableIsReturned);
            "And payment is stored".x(fixture.ThenPaymentIsStored);
            "And the response contains Payment failed failure".x(x => fixture.ThenResponseIsFailureWithErrorCodeMessage(ErrorCode.PaymentFailed));
            "And then the payment was searched for".x(fixture.ThenThePaymentWasSearchedFor);
        }

        [Scenario, CustomAutoData]
        public void SuccessfulPaymentAlreadyExists(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a successful payment exists".x(() => fixture.GivenPaymentAlreadyExists(request, Status.Successful));
            "When I submit the payment again".x(() => fixture.WhenISubmitThePayment());
            "Then a bad request response is returned".x(fixture.ThenAConflictResponseIsReturned);
            "And the response contains Payment already exists failure".x(x=> fixture.ThenResponseIsFailureWithErrorCodeMessage(ErrorCode.PaymentAlreadyExists));
            "And then the payment was searched for".x(fixture.ThenThePaymentWasSearchedFor);
        }

        [Scenario, CustomAutoData]
        public void RejectedPaymentAlreadyExists(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a successful payment exists".x(() => fixture.GivenPaymentAlreadyExists(request, Status.Rejected));
            "And given bank accepts payment".x(() => fixture.GivenBankAcceptsPayment());
            "When I submit the payment again".x(() => fixture.WhenISubmitThePayment());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And then the payment was stored".x(fixture.ThenPaymentWasStored);
            "And then the payment was searched for".x(fixture.ThenThePaymentWasSearchedFor);
        }

        [Scenario, CustomAutoData]
        public void FailedPaymentAlreadyExists(PaymentFixture fixture, ProcessPaymentRequest request)
        {
            "Given a successful payment exists".x(() => fixture.GivenPaymentAlreadyExists(request, Status.Failed));
            "And given bank accepts payment".x(() => fixture.GivenBankAcceptsPayment());
            "When I submit the payment again".x(() => fixture.WhenISubmitThePayment());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
            "And then the payment was stored".x(fixture.ThenPaymentWasStored);
            "And then the payment was searched for".x(fixture.ThenThePaymentWasSearchedFor);
        }

        public static TheoryData<PaymentFixture, ProcessPaymentRequest, string, object> GetInvalidProcessPaymentFields() => InvalidProcessPaymentFields.GetInvalidProcessPaymentFields();
    }
}

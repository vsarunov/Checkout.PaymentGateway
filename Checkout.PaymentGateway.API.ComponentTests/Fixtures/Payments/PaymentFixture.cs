using Checkout.PaymentGateway.API.ComponentTests.Shared;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments
{
    public class PaymentFixture : ApiClientFixture
    {
        private ProcessPaymentRequest payment;

        public PaymentFixture() : base()
        {
        }

        protected override void RegisterDependencies(IServiceCollection collection)
        {

        }

        internal void GivenAValidPayment(ProcessPaymentRequest request)
        {
            this.payment = request;
        }

        internal async Task GivenPaymentExists(ProcessPaymentRequest request)
        {
            this.payment = request;
            await CreatePaymentAsync(payment);
        }

        internal void GivenBankRejectsTransactions()
        {
            // reject payment
        }

        internal void GivenBankRequestIsFailing()
        {
            // bank request is failing
        }

        internal void GivenStorageFailure()
        {
            // storage failure
        }

        internal async Task WhenISubmitThePayment()
        {
            await CreatePaymentAsync(payment);
        }

        internal async Task WhenIGetExistingPaymentById()
        {
            await GetPaymentByIdAsync(payment.Id);
        }

        internal void ThenPaymentWasStored()
        {
            // check if payment was stored
        }

        internal void ThenPaymentWasSubmittedToBankSuccessfully()
        {
            // check if was submitted to bank
        }

        internal void ThenPaymentIsAsExpected()
        {
            // check expected payment
        }

        internal void ThenPaymentIsNotStored()
        {
            // check payment is not stored
        }

        internal void GivenBankAcceptsPayment()
        {
            // bank request is successful
        }

        internal void GivenAFieldIsSet(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(ProcessPaymentRequest.CardDetails.CVV):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, ProcessPaymentFactory.CreateCardDto((int)value, payment.CardDetails.Number, payment.CardDetails.Expiration), payment.Value, payment.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Number):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, ProcessPaymentFactory.CreateCardDto(payment.CardDetails.CVV, (string)value, payment.CardDetails.Expiration), payment.Value, payment.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration.Month):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, ProcessPaymentFactory.CreateCardDto(payment.CardDetails.CVV, payment.CardDetails.Number, ProcessPaymentFactory.CreateExpirationDateDto(payment.CardDetails.Expiration.Year, (int)value)), payment.Value, payment.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration.Year):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, ProcessPaymentFactory.CreateCardDto(payment.CardDetails.CVV, payment.CardDetails.Number, ProcessPaymentFactory.CreateExpirationDateDto((int)value, payment.CardDetails.Expiration.Month)), payment.Value, payment.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.Value.Amount):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, payment.CardDetails, ProcessPaymentFactory.CreateMoneyDto((decimal)value, payment.Value.ISOCurrencyCode), payment.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.Value.ISOCurrencyCode):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, payment.CardDetails, ProcessPaymentFactory.CreateMoneyDto(payment.Value.Amount, (string)value), payment.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp):
                    payment = ProcessPaymentFactory.CreatePaymentRequest(payment.Id, payment.CardDetails, payment.Value, new TransactionTimeStampDto((DateTime)value));
                    break;
                default:
                    throw new ArgumentException("Not defined field");

            };
        }
    }
}

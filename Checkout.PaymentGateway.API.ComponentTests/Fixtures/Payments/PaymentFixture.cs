using Checkout.PaymentGateway.API.ComponentTests.Mappers;
using Checkout.PaymentGateway.API.ComponentTests.Shared;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments
{
    public class PaymentFixture : ApiClientFixture
    {
        private ProcessPaymentRequest paymentRequest;
        private readonly IPaymentRepository paymentRepository;

        public PaymentFixture() : base()
        {
            paymentRepository = Substitute.For<IPaymentRepository>();
        }

        protected override void RegisterDependencies(IServiceCollection collection)
        {
            collection.AddSingleton<IPaymentRepository>(paymentRepository);
        }

        internal void GivenAValidPayment(ProcessPaymentRequest request)
        {
            paymentRequest = request;
        }

        internal async Task GivenPaymentExists(ProcessPaymentRequest request)
        {
            paymentRequest = request;
            await CreatePaymentAsync(paymentRequest);
        }

        internal void GivenBankRejectsTransactions()
        {
            // reject payment
        }

        internal void GivenBankRequestIsFailing()
        {
            // bank request is failing
        }

        internal void GivenBankAcceptsPayment()
        {
            // bank request is successful
        }

        internal void GivenStorageFailure()
        {
            paymentRepository.WhenForAnyArgs(x => x.SaveAsync(Arg.Any<Domain.Payments.Aggregates.Payment>())).Do(x => throw new Exception());
        }

        internal void GivenPaymentAlreadyExists(ProcessPaymentRequest request)
        {
            paymentRequest = request;
            paymentRepository.GetByIdAsync(new Domain.Payments.PaymentId(request.Id.Value)).Returns(paymentRequest.MapRequestToDomain());
        }

        internal async Task WhenISubmitThePayment()
        {
            await CreatePaymentAsync(paymentRequest);
        }

        internal async Task WhenIGetExistingPaymentById()
        {
            await GetPaymentByIdAsync(paymentRequest.Id);
        }

        internal void ThenPaymentWasStored()
        {
            paymentRepository.ReceivedWithAnyArgs().SaveAsync(Arg.Any<Domain.Payments.Aggregates.Payment>());
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
            paymentRepository.DidNotReceiveWithAnyArgs().SaveAsync(Arg.Any<Domain.Payments.Aggregates.Payment>());
        }

        internal void GivenAFieldIsSet(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(ProcessPaymentRequest.CardDetails.CVV):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, ProcessPaymentFactory.CreateCardDto((int)value, paymentRequest.CardDetails.Number, paymentRequest.CardDetails.Expiration), paymentRequest.Value, paymentRequest.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Number):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, ProcessPaymentFactory.CreateCardDto(paymentRequest.CardDetails.CVV, (string)value, paymentRequest.CardDetails.Expiration), paymentRequest.Value, paymentRequest.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration.Month):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, ProcessPaymentFactory.CreateCardDto(paymentRequest.CardDetails.CVV, paymentRequest.CardDetails.Number, ProcessPaymentFactory.CreateExpirationDateDto(paymentRequest.CardDetails.Expiration.Year, (int)value)), paymentRequest.Value, paymentRequest.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration.Year):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, ProcessPaymentFactory.CreateCardDto(paymentRequest.CardDetails.CVV, paymentRequest.CardDetails.Number, ProcessPaymentFactory.CreateExpirationDateDto((int)value, paymentRequest.CardDetails.Expiration.Month)), paymentRequest.Value, paymentRequest.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.Value.Amount):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, paymentRequest.CardDetails, ProcessPaymentFactory.CreateMoneyDto((decimal)value, paymentRequest.Value.ISOCurrencyCode), paymentRequest.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.Value.ISOCurrencyCode):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, paymentRequest.CardDetails, ProcessPaymentFactory.CreateMoneyDto(paymentRequest.Value.Amount, (string)value), paymentRequest.TransactionTimeStamp);
                    break;
                case nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp):
                    paymentRequest = ProcessPaymentFactory.CreatePaymentRequest(paymentRequest.Id, paymentRequest.CardDetails, paymentRequest.Value, new TransactionTimeStampDto((DateTime)value));
                    break;
                default:
                    throw new ArgumentException("Not defined field");

            };
        }
    }
}

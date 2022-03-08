using Amido.Stacks.Testing.Extensions;
using Checkout.PaymentGateway.API.ComponentTests.Mappers;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Services;
using Checkout.AcquiringBank.Configuration;
using Checkout.PaymentGateway.API.ComponentTests.Setup;

namespace Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments
{
    public class PaymentFixture : ApiClientFixture
    {
        private ProcessPaymentRequest paymentRequest;
        private readonly IPaymentRepository paymentRepository;

        private readonly MockableHttpMessageHandler mockBankServiceHttpMessageHandler;

        public PaymentFixture() : base()
        {
            paymentRepository = Substitute.For<IPaymentRepository>();
            mockBankServiceHttpMessageHandler = Substitute.ForPartsOf<MockableHttpMessageHandler>();
        }

        protected override void RegisterDependencies(IServiceCollection collection)
        {
            collection.Configure<BankDetails>(x =>
            {
                x.Url = "www.acquiringbank.com";
            });

            collection.AddSingleton<IPaymentRepository>(paymentRepository);

            collection.AddHttpClient<IBankService, AcquiringBankService>().AddHttpMessageHandler(_ => mockBankServiceHttpMessageHandler);
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

        internal async Task ThenAnErrorPropertyIsReturned(string fieldName)
        {
            var result = await GetResponseJsonString();
            var json = JObject.Parse(result);
            var errors = json["errors"].FirstOrDefault(x => ((JProperty)x).Name.Contains(fieldName)).Values().FirstOrDefault().ToObject<string>();

            errors.ShouldNotBeEmpty();
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
                case nameof(ProcessPaymentRequest.CardDetails):
                    paymentRequest.With(x => x.CardDetails, (CardDto)value);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.CVV):
                    paymentRequest.CardDetails.With(x => x.CVV, (int)value);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Number):
                    paymentRequest.CardDetails.With(x => x.Number, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration):
                    paymentRequest.CardDetails.With(x => x.Expiration, (CardExpirationDateDto)value);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration.Month):
                    paymentRequest.CardDetails.Expiration.With(x => x.Month, (int)value);
                    break;
                case nameof(ProcessPaymentRequest.CardDetails.Expiration.Year):
                    paymentRequest.CardDetails.Expiration.With(x => x.Year, (int)value);
                    break;
                case nameof(ProcessPaymentRequest.Value):
                    paymentRequest.With(x => x.Value, (PaymentDto)value);
                    break;
                case nameof(ProcessPaymentRequest.Value.Amount):
                    paymentRequest.Value.With(x => x.Amount, (decimal)value);
                    break;
                case nameof(ProcessPaymentRequest.Value.ISOCurrencyCode):
                    paymentRequest.Value.With(x => x.ISOCurrencyCode, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.TransactionTimeStamp):
                    paymentRequest.With(x => x.TransactionTimeStamp, (TransactionTimeStampDto)value);
                    break;
                case nameof(ProcessPaymentRequest.TransactionTimeStamp.TimeStamp):
                    paymentRequest.TransactionTimeStamp.With(x => x.TimeStamp, (DateTime)value);
                    break;
                default:
                    throw new ArgumentException("Not defined field");

            };
        }
    }
}

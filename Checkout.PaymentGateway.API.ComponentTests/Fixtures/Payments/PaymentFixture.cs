using Amido.Stacks.Testing.Extensions;
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
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using Checkout.PaymentGateway.Tests.Shared.Mappers;
using Checkout.PaymentGateway.Domain.Payments;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

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
            //collection.Configure<BankDetails>(x =>
            //{
            //    x.Url = "www.acquiringbank.com";
            //});

            //collection.AddSingleton<IPaymentRepository>(paymentRepository);

            //collection.AddHttpClient<IBankService, AcquiringBankService>().AddHttpMessageHandler(_ => mockBankServiceHttpMessageHandler);
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
            var paymentProcessingResult = new PaymentProcessingResult(Status.Rejected);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentProcessingResult))
            };

            mockBankServiceHttpMessageHandler.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);
        }

        internal void GivenBankRequestIsFailing()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            mockBankServiceHttpMessageHandler.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);
        }

        internal void GivenBankAcceptsPayment()
        {
            var paymentProcessingResult = new PaymentProcessingResult(Status.Successful);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentProcessingResult))
            };

            mockBankServiceHttpMessageHandler.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);
        }

        internal void GivenStorageFailure()
        {
            paymentRepository.WhenForAnyArgs(x => x.SaveAsync(Arg.Any<Domain.Payments.Aggregates.PaymentRoot>())).Do(x => throw new Exception());
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
            paymentRepository.ReceivedWithAnyArgs().SaveAsync(Arg.Any<Domain.Payments.Aggregates.PaymentRoot>());
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
            mockBankServiceHttpMessageHandler.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        internal void ThenPaymentIsAsExpected()
        {
            // check expected payment
        }

        internal void ThenPaymentIsNotStored()
        {
            paymentRepository.DidNotReceiveWithAnyArgs().SaveAsync(Arg.Any<Domain.Payments.Aggregates.PaymentRoot>());
        }

        internal void GivenAFieldIsSet(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(ProcessPaymentRequest.Payer):
                    paymentRequest.With(x => x.Payer, (PayerDto)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Card):
                    paymentRequest.Payer.With(x => x.Card, (Models.Shared.Payments.CardDto)value);
                    paymentRequest.Merchant.With(x => x.Card, (Models.Shared.Payments.CardDto)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Card.CVV):
                    paymentRequest.Payer.Card.With(x => x.CVV, (int)value);
                    paymentRequest.Merchant.Card.With(x => x.CVV, (int)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Card.Number):
                    paymentRequest.Payer.Card.With(x => x.Number, (string)value);
                    paymentRequest.Merchant.Card.With(x => x.Number, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Card.Expiration):
                    paymentRequest.Payer.Card.With(x => x.Expiration, (CardExpirationDateDto)value);
                    paymentRequest.Merchant.Card.With(x => x.Expiration, (CardExpirationDateDto)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Card.Expiration.Month):
                    paymentRequest.Payer.Card.Expiration.With(x => x.Month, (int)value);
                    paymentRequest.Merchant.Card.Expiration.With(x => x.Month, (int)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Card.Expiration.Year):
                    paymentRequest.Payer.Card.Expiration.With(x => x.Year, (int)value);
                    paymentRequest.Merchant.Card.Expiration.With(x => x.Year, (int)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Address):
                    paymentRequest.Payer.With(x => x.Address, (Models.Shared.Payments.AddressDto)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Address.AddressLine):
                    paymentRequest.Payer.Address.With(x => x.AddressLine, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Address.PostCode):
                    paymentRequest.Payer.Address.With(x => x.PostCode, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Name):
                    paymentRequest.Payer.With(x => x.Name, (NameDto)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Name.FirstName):
                    paymentRequest.Payer.Name.With(x => x.FirstName, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.Payer.Name.LastName):
                    paymentRequest.Payer.Name.With(x => x.LastName, (string)value);
                    break;
                case nameof(ProcessPaymentRequest.Merchant):
                    paymentRequest.With(x => x.Merchant, (MerchantDto)value);
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

using Checkout.AcquiringBank.Configuration;
using Checkout.AcquiringBank.Services;
using Checkout.PaymentGateway.Domain.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Tests.Shared;
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.AcquiringBank.UnitTests.Services
{
    [Trait("TestType", "UnitTest")]
    public class AcquiringBankServiceTests
    {
        private readonly MockableHttpMessageHandler aqcuiringBankHttpClientMock;
        private readonly MockLogger<AcquiringBankService> aqcuiringBankLoggerMock;
        private readonly BankDetails bankDetails;

        private readonly AcquiringBankService sut;

        public AcquiringBankServiceTests()
        {
            aqcuiringBankHttpClientMock = Substitute.ForPartsOf<MockableHttpMessageHandler>();

            aqcuiringBankLoggerMock = Substitute.For<MockLogger<AcquiringBankService>>();

            bankDetails = new BankDetails { Url = "www.randomUri.com" };

            IOptions<BankDetails> bankOptions = Options.Create(bankDetails);

            sut = new AcquiringBankService(new HttpClient(aqcuiringBankHttpClientMock), aqcuiringBankLoggerMock, bankOptions);
        }

        [Fact]
        public async Task ProcessPayment_WhenValidRequest_ShouldReturnsSuccessStatus()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Successful.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentProcessingResult))
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            var response = await sut.ProcessPayment(payment);

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(paymentProcessingResult);
        }
        [Fact]
        public async Task ProcessPayment_WhenValidRequest_MessageShouldHaveBeenSent()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Successful.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentProcessingResult))
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            await aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentIsRejected_ShouldReturnRejectedStatus()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Rejected.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentProcessingResult))
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            var response = await sut.ProcessPayment(payment);

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(paymentProcessingResult);
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentIsRejected_MessageShouldHaveBeenSent()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Rejected.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentProcessingResult))
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            await aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailed_ShouldReturnFailedStatus()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Failed.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            var response = await sut.ProcessPayment(payment);

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(paymentProcessingResult);
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailed_MessageShouldHaveBeenSent()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Failed.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            await aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailed_ShouldLogAnError()
        {
            var payment = CreateDomainPayment();

            var paymentStatus = Status.Failed.ToString();

            var paymentProcessingResult = new PaymentProcessingResult(paymentStatus);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankLoggerMock.Received().Log(LogLevel.Error, 1000, Arg.Any<string>());
        }

        private static PaymentRoot CreateDomainPayment()
        {
            return PaymentRoot.Create(
                new PaymentId(Guid.NewGuid()),
                new Payer(
                    new Address("17 Plumstead high street", "SE181JT"),
                    new Name("Tom", "Riddle", null),
                    new Card("12345678", 543, new CardExpirationDate(DateTime.UtcNow.AddYears(1).Year, DateTime.UtcNow.AddMonths(1).Month))
                    ),
                new Merchant(new MerchantId(Guid.NewGuid()), new Card("98765432", 123, new CardExpirationDate(DateTime.UtcNow.AddYears(3).Year, DateTime.UtcNow.AddMonths(3).Month))),
                new Payment(5000.0m, "USD"),
                new TransactionTimeStamp(DateTime.UtcNow),
                Guid.NewGuid()
                );
        }
    }
}

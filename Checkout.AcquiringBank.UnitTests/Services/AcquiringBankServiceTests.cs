using Checkout.AcquiringBank.Configuration;
using Checkout.AcquiringBank.Services;
using Checkout.PaymentGateway.Common.Exceptions;
using Checkout.PaymentGateway.Domain.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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

            aqcuiringBankLoggerMock.IsEnabled(Arg.Any<LogLevel>()).ReturnsForAnyArgs(true);

            bankDetails = new BankDetails { Url = "https://stackoverflow.com" };

            IOptions<BankDetails> bankOptions = Options.Create(bankDetails);

            sut = new AcquiringBankService(new HttpClient(aqcuiringBankHttpClientMock), aqcuiringBankLoggerMock, bankOptions);
        }

        [Fact]
        public async Task ProcessPayment_WhenValidRequest_ShouldReturnsSuccessStatus()
        {
            var payment = CreateDomainPayment();

            var paymentProcessingResult = new PaymentProcessingResult(Status.Successful);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(paymentProcessingResult))
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

            var paymentProcessingResult = new PaymentProcessingResult(Status.Successful);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(paymentProcessingResult))
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentIsRejected_ShouldReturnRejectedStatus()
        {
            var payment = CreateDomainPayment();

            var paymentProcessingResult = new PaymentProcessingResult(Status.Rejected);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(paymentProcessingResult))
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

            var paymentProcessingResult = new PaymentProcessingResult(Status.Rejected);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(paymentProcessingResult))
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailedWithInternalServerError_ShouldReturnFailedStatus()
        {
            var payment = CreateDomainPayment();

            var paymentProcessingResult = new PaymentProcessingResult(Status.Failed);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            var response = await sut.ProcessPayment(payment);

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(paymentProcessingResult);
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailedWithInternalServerError_MessageShouldHaveBeenSent()
        {
            var payment = CreateDomainPayment();

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailedWithInternalServerError_ShouldLogAnError()
        {
            var payment = CreateDomainPayment();

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankLoggerMock.Received().Log(LogLevel.Error, 6300, Arg.Any<string>());
        }


        [Fact]
        public async Task ProcessPayment_WhenPaymentFailedWithBadRequest_ShouldReturnFailedStatus()
        {
            var payment = CreateDomainPayment();

            var paymentProcessingResult = new PaymentProcessingResult(Status.Failed);

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            var response = await sut.ProcessPayment(payment);

            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(paymentProcessingResult);
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailedWithBadRequest_MessageShouldHaveBeenSent()
        {
            var payment = CreateDomainPayment();

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankHttpClientMock.ReceivedWithAnyArgs().SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment")));
        }

        [Fact]
        public async Task ProcessPayment_WhenPaymentFailedWithBadRequest_ShouldLogAnError()
        {
            var payment = CreateDomainPayment();

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            await sut.ProcessPayment(payment);

            aqcuiringBankLoggerMock.Received().Log(LogLevel.Error, 6300, Arg.Any<string>());
        }

        [Fact]
        public async Task ProcessPayment_WhenReturnedStatusIsInvalid_ShouldThrowInvalidPaymentStatusException()
        {
            var payment = CreateDomainPayment();

            var responseJsonMock = "{\"PaymentStatus\":\"RandomString\"}";

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJsonMock)
            };

            aqcuiringBankHttpClientMock.SendAsync(Arg.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post && x.RequestUri.ToString().EndsWith($"/v1/process-payment"))).Returns(httpResponseMessage);

            Func<Task> act = () => sut.ProcessPayment(payment);
            await act.Should().ThrowAsync<InvalidPaymentStatusException>();
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

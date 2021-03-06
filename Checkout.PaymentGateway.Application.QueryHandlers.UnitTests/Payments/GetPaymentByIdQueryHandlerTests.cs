using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Application.QueryHandlers.Payments;
using Checkout.PaymentGateway.Common.Enums;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Domain.Shared;
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using FluentAssertions;
using LanguageExt.UnitTesting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.PaymentGateway.Application.QueryHandlers.UnitTests.Payments
{
    [Trait("TestType", "UnitTest")]
    public class GetPaymentByIdQueryHandlerTests
    {
        private readonly GetPaymentByIdQueryHandler sut;
        private readonly MockLogger<GetPaymentByIdQueryHandler> loggerMock;
        private readonly IPaymentRepository paymentRepository;

        public GetPaymentByIdQueryHandlerTests()
        {
            paymentRepository = Substitute.For<IPaymentRepository>();

            loggerMock = Substitute.For<MockLogger<GetPaymentByIdQueryHandler>>();
            loggerMock.IsEnabled(Arg.Any<LogLevel>()).ReturnsForAnyArgs(true);

            sut = new GetPaymentByIdQueryHandler(paymentRepository, loggerMock);
        }

        [Fact]
        public async Task Handle_GivenPaymentExists_ShouldReturnPayment()
        {
            var query = CreateQuery();

            PaymentRoot repositoryResponse = CreateDomainPayment();

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);

            var result = await sut.Handle(query, CancellationToken.None);

            result.ShouldBeLeft(x =>
            {
                x.Should().BeEquivalentTo(repositoryResponse);
            });
        }

        [Fact]
        public async Task Handle_GivenPaymentDoesNotExists_ShouldNone()
        {
            var query = CreateQuery();

            PaymentRoot repositoryResponse = null;

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);

            var result = await sut.Handle(query, CancellationToken.None);

            var failure = Failure.Of(query.Id.Value, ErrorCode.PaymentNotFound);

            result.ShouldBeRight(x =>
            {
                x.Should().BeEquivalentTo(failure);
            });
        }

        [Fact]
        public async Task Handle_GivenPaymentDoesNotExists_ShouldLogInformation()
        {
            var query = CreateQuery();

            PaymentRoot repositoryResponse = null;

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);

            var result = await sut.Handle(query, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Information, 6100, Arg.Any<string>());
        }

        private static GetPaymentByIdQuery CreateQuery() =>
            GetPaymentByIdQuery.Create(new CQRS.Models.Payments.PaymentId(Guid.NewGuid()), Guid.NewGuid());

        private static PaymentRoot CreateDomainPayment()
        {
            return PaymentRoot.Create(
                new Domain.Payments.PaymentId(Guid.NewGuid()),
                new Domain.Payments.Payer(
                    new Domain.Payments.Address("17 Plumstead high street", "SE181JT"),
                    new Domain.Payments.Name("Tom", "Riddle", null),
                    new Domain.Payments.Card("12345678", 543, new Domain.Payments.CardExpirationDate(DateTime.UtcNow.AddYears(1).Year, DateTime.UtcNow.AddMonths(1).Month))
                    ),
                new Domain.Payments.Merchant(new Domain.Payments.MerchantId(Guid.NewGuid()), new Domain.Payments.Card("98765432", 123, new Domain.Payments.CardExpirationDate(DateTime.UtcNow.AddYears(3).Year, DateTime.UtcNow.AddMonths(3).Month))),
                new Domain.Payments.Payment(5000.0m, "USD"),
                new Domain.Payments.TransactionTimeStamp(DateTime.UtcNow),
                Guid.NewGuid()
                );
        }

    }
}

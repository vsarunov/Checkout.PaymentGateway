using Checkout.PaymentGateway.Application.CommandHandlers.Payments;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.CQRS.Models.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using LanguageExt.UnitTesting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.PaymentGateway.Application.CommandHandlers.UnitTests.Payments
{
    [Trait("TestType", "UnitTest")]
    public class ProcessPaymentCommandHandlerTests
    {
        private readonly ProcessPaymentCommandHandler sut;
        private readonly MockLogger<ProcessPaymentCommandHandler> loggerMock;
        private readonly IPaymentRepository paymentRepository;
        private readonly IBankService bankService;

        public ProcessPaymentCommandHandlerTests()
        {
            paymentRepository = Substitute.For<IPaymentRepository>();
            bankService = Substitute.For<IBankService>();

            loggerMock = Substitute.For<MockLogger<ProcessPaymentCommandHandler>>();
            loggerMock.IsEnabled(Arg.Any<LogLevel>()).ReturnsForAnyArgs(true);

            sut = new ProcessPaymentCommandHandler(paymentRepository, bankService, loggerMock);
        }

        [Fact]
        public async Task Handle_GivenValidPayment_ShouldProcessPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldBeNone();
        }

        [Fact]
        public async Task Handle_GivenPaymentExistsById_ShouldReturnFailure()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = CreateDomainPayment();

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldBeSome();
        }

        [Fact]
        public async Task Handle_GivenPaymentExistsById_ShouldLoggAnError()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = CreateDomainPayment();

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Error, 6300, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenPaymentExistsByEquality_ShouldReturnFailure()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldBeSome();
        }

        [Fact]
        public async Task Handle_GivenPaymentExistsByEquality_ShouldLogAnError()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentRepository.GetByIdAsync(Arg.Any<Domain.Payments.PaymentId>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Error, 6300, Arg.Any<string>());
        }

        private static ProcessPaymentCommand CreateCommand()
        {
            return ProcessPaymentCommand.Create(
                Guid.NewGuid(),
                new PaymentId(Guid.NewGuid()),
                new Payer(
                    new Address("17 Plumstead high street", "SE181JT"),
                    new Name("Tom", "Riddle", null),
                    new Card("12345678", 543, new CardExpirationDate(DateTime.UtcNow.AddYears(1).Year, DateTime.UtcNow.AddMonths(1).Month))
                    ),
                new Payment(5000.0m, "USD"),
                new Merchant(new MerchantId(Guid.NewGuid()), new Card("98765432", 123, new CardExpirationDate(DateTime.UtcNow.AddYears(3).Year, DateTime.UtcNow.AddMonths(3).Month))),
                new TransactionTimeStamp(DateTime.UtcNow)
                );
        }

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

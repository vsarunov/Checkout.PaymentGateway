using Checkout.PaymentGateway.Application.CommandHandlers.Payments;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Common.Enums;
using Checkout.PaymentGateway.CQRS.Models.Payments;
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

namespace Checkout.PaymentGateway.Application.CommandHandlers.UnitTests.Payments
{
    [Trait("TestType", "UnitTest")]
    public class ProcessPaymentCommandHandlerTests
    {
        private readonly ProcessPaymentCommandHandler sut;
        private readonly MockLogger<ProcessPaymentCommandHandler> loggerMock;
        private readonly IPaymentSearchService paymentSearchService;
        private readonly IPaymentRepository paymentRepository;
        private readonly IBankService bankService;

        public ProcessPaymentCommandHandlerTests()
        {
            paymentSearchService = Substitute.For<IPaymentSearchService>();
            paymentRepository = Substitute.For<IPaymentRepository>();
            bankService = Substitute.For<IBankService>();

            loggerMock = Substitute.For<MockLogger<ProcessPaymentCommandHandler>>();
            loggerMock.IsEnabled(Arg.Any<LogLevel>()).ReturnsForAnyArgs(true);

            paymentRepository = Substitute.For<IPaymentRepository>();
            sut = new ProcessPaymentCommandHandler(paymentSearchService, paymentRepository, bankService, loggerMock);
        }

        [Fact]
        public async Task Handle_GivenValidPayment_ShouldProcessPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldBeNone();
            bankService.ReceivedWithAnyArgs().ProcessPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenValidPayment_ShouldSavePayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            paymentRepository.ReceivedWithAnyArgs().UpsertPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenSuccessfulPaymentExists_ShouldReturnFailure()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Successful);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            result.ShouldBeSome(x =>
            {
                x.Should().BeEquivalentTo(failure);
            });
        }

        [Fact]
        public async Task Handle_GivenSuccessfulPaymentExists_ShouldLogAnError()
        {
            var command = CreateCommand();

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Successful);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);

            await sut.Handle(command, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Error, 6301, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenSuccessfulPaymentExists_ShouldNotProcessPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Successful);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            bankService.DidNotReceiveWithAnyArgs().ProcessPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenSuccessfulPaymentExists_ShouldNotSavePayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Successful);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            paymentRepository.DidNotReceiveWithAnyArgs().UpsertPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenRejectedPaymentExistsReturnsSuccessfulOnReprocessing_ShouldProcessPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Rejected);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            bankService.ReceivedWithAnyArgs().ProcessPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenRejectedPaymentExistsReturnsSuccessfulOnReprocessing_ShouldLogAWarning()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Rejected);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            loggerMock.Received().Log(LogLevel.Warning, 6200, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenRejectedPaymentExistsReturnsSuccessfulOnReprocessing_UpdatedPaymentInStore()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Rejected);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            paymentRepository.Received().UpsertPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenFailedPaymentExistsReturnsSuccessfulOnReprocessing_ShouldProcessPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Failed);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            bankService.ReceivedWithAnyArgs().ProcessPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenFailedPaymentExistsReturnsSuccessfulOnReprocessing_ShouldLogAWarning()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Failed);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            loggerMock.Received().Log(LogLevel.Warning, 6200, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenFailedPaymentExistsReturnsSuccessfulOnReprocessing_UpdatedPaymentInStore()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            var repositoryResponse = CreateDomainPayment();
            repositoryResponse.UpdateStatus(Domain.Payments.Status.Failed);

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentAlreadyExists);

            paymentRepository.Received().UpsertPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenPaymentRejected_ShouldReturnFailure()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Rejected);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentRejected);

            result.ShouldBeSome(x =>
            {
                x.Should().BeEquivalentTo(failure);
            });
        }

        [Fact]
        public async Task Handle_GivenPaymentRejected_ShouldLogAnError()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Rejected);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Error, 6302, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenPaymentRejected_ShouldStorePayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Rejected);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            paymentRepository.ReceivedWithAnyArgs().UpsertPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenPaymentRejected_ShouldHaveProcessedPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Rejected);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            bankService.ReceivedWithAnyArgs().ProcessPayment(Arg.Is<PaymentRoot>(x => x.Id.Value == command.Id.Value));
        }

        [Fact]
        public async Task Handle_GivenPaymentFailed_ShouldReturnFailure()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Failed);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            var failure = Failure.Of(command.Id.Value, ErrorCode.PaymentFailed);

            result.ShouldBeSome(x =>
            {
                x.Should().BeEquivalentTo(failure);
            });
        }

        [Fact]
        public async Task Handle_GivenPaymentFailed_ShouldLogAnError()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Failed);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Error, 6303, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenPaymentFailed_ShouldStorePayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Failed);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            loggerMock.Received().Log(LogLevel.Error, 6303, Arg.Any<string>());
        }

        [Fact]
        public async Task Handle_GivenPaymentFailed_ShouldHaveProcessedPayment()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Failed);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            await sut.Handle(command, CancellationToken.None);

            bankService.ReceivedWithAnyArgs().ProcessPayment(Arg.Is<PaymentRoot>(x=>x.Id.Value== command.Id.Value));
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
                    new Domain.Payments.Card("12345678", 543,
                        new Domain.Payments.CardExpirationDate(DateTime.UtcNow.AddYears(1).Year,
                            DateTime.UtcNow.AddMonths(1).Month))
                ),
                new Domain.Payments.Merchant(new Domain.Payments.MerchantId(Guid.NewGuid()),
                    new Domain.Payments.Card("98765432", 123,
                        new Domain.Payments.CardExpirationDate(DateTime.UtcNow.AddYears(3).Year,
                            DateTime.UtcNow.AddMonths(3).Month))),
                new Domain.Payments.Payment(5000.0m, "USD"),
                new Domain.Payments.TransactionTimeStamp(DateTime.UtcNow),
                Guid.NewGuid()
            );
        }
    }
}

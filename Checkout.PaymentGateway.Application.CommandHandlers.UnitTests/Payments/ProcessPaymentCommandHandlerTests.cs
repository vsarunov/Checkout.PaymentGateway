﻿using Checkout.PaymentGateway.Application.CommandHandlers.Payments;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
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
        private readonly IPaymentSearchService paymentSearchService;
        private readonly IBankService bankService;

        public ProcessPaymentCommandHandlerTests()
        {
            paymentSearchService = Substitute.For<IPaymentSearchService>();
            bankService = Substitute.For<IBankService>();

            loggerMock = Substitute.For<MockLogger<ProcessPaymentCommandHandler>>();
            loggerMock.IsEnabled(Arg.Any<LogLevel>()).ReturnsForAnyArgs(true);

            sut = new ProcessPaymentCommandHandler(paymentSearchService, bankService, loggerMock);
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
        }

        [Fact]
        public async Task Handle_GivenPaymentExists_ShouldReturnFailure()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
            bankService.ProcessPayment(Arg.Any<PaymentRoot>()).Returns(bankProcessingResult);

            var result = await sut.Handle(command, CancellationToken.None);

            result.ShouldBeSome();
        }

        [Fact]
        public async Task Handle_GivenPaymentExists_ShouldLogAnError()
        {
            var command = CreateCommand();

            var bankProcessingResult = new Domain.Payments.PaymentProcessingResult(Domain.Payments.Status.Successful);

            PaymentRoot repositoryResponse = null;

            paymentSearchService.SearchPayment(Arg.Any<PaymentRoot>()).Returns(repositoryResponse);
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
    }
}

using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Infrastructure.Services.Payments;
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using FluentAssertions;
using LanguageExt.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Checkout.PaymentGateway.Infrastructure.UnitTests.Services.Payments;

[Trait("TestType", "UnitTest")]
public class PaymentSearchServiceTests
{
    private readonly PaymentSearchService sut;
    private readonly Dictionary<Guid, PaymentRoot> storage;
    private readonly MockLogger<PaymentSearchService> paymentSearchServiceLoggerMock;

    public PaymentSearchServiceTests()
    {
        storage = new Dictionary<Guid, PaymentRoot>();
        paymentSearchServiceLoggerMock = Substitute.For<MockLogger<PaymentSearchService>>();

        sut = new PaymentSearchService(storage, paymentSearchServiceLoggerMock);
    }

    [Fact]
    public void SearchPayment_WhenExistingPayment_ShouldReturnPayment()
    {
        var existingPayment = CreateDomainPayment();

        storage.Add(existingPayment.Id.Value, existingPayment);

        var result = sut.SearchPayment(existingPayment);
        
        result.ShouldBeSome(
            x=>
                x.Should().BeEquivalentTo(existingPayment)
            );
    }

    [Fact]
    public void SearchPayment_WhenNonExistingPayment_ShouldNotReturnPayment()
    {
        var nonExistingPayment = CreateDomainPayment();

        var result = sut.SearchPayment(nonExistingPayment);
        
        result.ShouldBeNone();
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
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Infrastructure.Services.Payments;

public class PaymentSearchService : IPaymentSearchService
{
    private readonly Dictionary<Guid, PaymentRoot> storage;
    private readonly ILogger<PaymentSearchService> logger;

    public PaymentSearchService(Dictionary<Guid, PaymentRoot> storage, ILogger<PaymentSearchService> logger)
    {
        this.logger = logger;
        this.storage = storage;
    }

    public Option<PaymentRoot> SearchPayment(PaymentRoot payment)
    {
        logger.SearchingForPayment();

        var paymentRoot = storage.FirstOrDefault(x => x.Value.Equals(payment));

        return paymentRoot.Value;
    }
}
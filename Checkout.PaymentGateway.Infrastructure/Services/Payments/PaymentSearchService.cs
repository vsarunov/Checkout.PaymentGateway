using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
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

    public Task<PaymentRoot> SearchPayment(PaymentRoot payment)
    {
        
        
        throw new NotImplementedException();
    }
}
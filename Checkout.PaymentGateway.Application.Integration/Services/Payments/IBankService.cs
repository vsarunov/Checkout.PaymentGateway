using Checkout.PaymentGateway.Domain.Payments;

namespace Checkout.PaymentGateway.Application.Integration.Payments.Services;

public interface IBankService
{
    Task<PaymentProcessingResult> ProcessPayment(Domain.Payments.Aggregates.Payment payment);
}

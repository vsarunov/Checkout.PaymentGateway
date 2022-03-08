using Checkout.PaymentGateway.Domain.Payments;

namespace Checkout.PaymentGateway.Application.Integration.Repositories.Payments;

public interface IPaymentRepository
{
    Task<Domain.Payments.Aggregates.PaymentRoot> GetByIdAsync(PaymentId id);
    Task<bool> SaveAsync(Domain.Payments.Aggregates.PaymentRoot payment);
}

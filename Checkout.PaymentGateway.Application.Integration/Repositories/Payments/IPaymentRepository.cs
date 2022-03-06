using Checkout.PaymentGateway.Domain.Payments;

namespace Checkout.PaymentGateway.Application.Integration.Repositories.Payments;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(PaymentId id);
    Task<bool> SaveAsync(Payment entity);
}

using Checkout.PaymentGateway.Domain.Payments;
using LanguageExt;

namespace Checkout.PaymentGateway.Application.Integration.Repositories.Payments;

public interface IPaymentRepository
{
    Task<Option<Domain.Payments.Aggregates.PaymentRoot>> GetByIdAsync(PaymentId id);
    void SaveAsync(Domain.Payments.Aggregates.PaymentRoot payment);
    void UpdatePayment(Domain.Payments.Aggregates.PaymentRoot payment);
}

using Checkout.PaymentGateway.Domain.Payments;
using LanguageExt;

namespace Checkout.PaymentGateway.Application.Integration.Repositories.Payments;

public interface IPaymentRepository
{
    Task<Option<Domain.Payments.Aggregates.PaymentRoot>> GetByIdAsync(PaymentId id);
    void UpsertPayment(Domain.Payments.Aggregates.PaymentRoot payment);
}

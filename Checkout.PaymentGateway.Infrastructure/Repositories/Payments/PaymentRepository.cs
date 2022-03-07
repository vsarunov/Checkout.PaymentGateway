using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Domain.Payments;

namespace Checkout.PaymentGateway.Infrastructure.Repositories.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        private static readonly Object @lock = new Object();
        private static readonly Dictionary<Guid, Payment> storage = new Dictionary<Guid, Payment>();

        public Task<Domain.Payments.Aggregates.Payment> GetByIdAsync(PaymentId id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync(Domain.Payments.Aggregates.Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}

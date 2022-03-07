using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Domain.Payments;

namespace Checkout.PaymentGateway.Infrastructure.Repositories.Payments
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private static readonly Object @lock = new Object();
        private static readonly Dictionary<Guid, Domain.Payments.Aggregates.Payment> storage = new Dictionary<Guid, Domain.Payments.Aggregates.Payment>();

        public async Task<Domain.Payments.Aggregates.Payment> GetByIdAsync(PaymentId id)
        {
            if (storage.ContainsKey(id.Value))
                return await Task.FromResult(storage[id.Value]);
            else
                return await Task.FromResult((Domain.Payments.Aggregates.Payment)null);
        }

        public async Task<bool> SaveAsync(Domain.Payments.Aggregates.Payment payment)
        {
            if (payment == null)
                return await Task.FromResult(false);

            lock (@lock)
            {
                if (storage.ContainsKey(payment.Id.Value))
                    storage[payment.Id.Value] = payment;
                else
                    storage.Add(payment.Id.Value, payment);
            }

            return await Task.FromResult(true);
        }
    }
}

﻿using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using Checkout.PaymentGateway.Domain.Payments;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Infrastructure.Repositories.Payments;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private static readonly Object @lock = new Object();
    private readonly Dictionary<Guid, Domain.Payments.Aggregates.PaymentRoot> storage;
    private readonly ILogger<InMemoryPaymentRepository> logger;

    public InMemoryPaymentRepository(Dictionary<Guid, Domain.Payments.Aggregates.PaymentRoot> storage,
        ILogger<InMemoryPaymentRepository> logger)
    {
        this.storage = storage;
        this.logger = logger;
    }

    public async Task<Domain.Payments.Aggregates.PaymentRoot> GetByIdAsync(PaymentId id)
    {
        logger.GettingPaymentByIdFromInMemoryRepository();

        if (storage.ContainsKey(id.Value))
            return await Task.FromResult(storage[id.Value]);
        else
            return await Task.FromResult((Domain.Payments.Aggregates.PaymentRoot) null);
    }

    public async Task<bool> SaveAsync(Domain.Payments.Aggregates.PaymentRoot payment)
    {
        logger.SavingPaymentByIdFromInMemoryRepository();

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
using Amido.Stacks.Domain;
using Checkout.PaymentGateway.Common.Exceptions;
using Checkout.PaymentGateway.Domain.Payments.Events;
using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Domain.Payments.Aggregates;

public class Payment : AggregateRoot<PaymentId>
{
    [JsonConstructor]
    private Payment(PaymentId id, Payments.Payment value, Card card, TransactionTimeStamp timeStamp,Guid correlationId)
    {
        Value = value;
        Id = id;
        Card = card;
        TimeStamp = timeStamp;
        CorrelationId = correlationId;
    }

    public static Payment Create(PaymentId id, Payments.Payment value, Card card, TransactionTimeStamp timeStamp, Guid correlationId)
    {
        return new Payment(id, value, card, timeStamp, correlationId);
    }

    public Payments.Payment Value { get; init; }
    public PaymentId Id { get; init; }
    public Card Card { get; init; }
    public TransactionTimeStamp TimeStamp { get; init; }
    public Status Status { get; private set; } = Status.ProcessingStarted;
    public Guid CorrelationId { get; init; }

    public void UpdateStatus(Status status)
    {
        Status = status;

        switch (status)
        {
            case Status.Failed:
                Emit(new PaymentFailed());
                break;
            case Status.Rejected:
                Emit(new PaymentRejected());
                break;
            case Status.Successful:
                Emit(new PaymentProcessed());
                break;
            default: InvalidPaymentStatusException.Raise(); break;
        }
    }
}


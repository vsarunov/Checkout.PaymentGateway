using Amido.Stacks.Domain;
using Checkout.PaymentGateway.Common.Exceptions;
using Checkout.PaymentGateway.Domain.Payments.Events;

namespace Checkout.PaymentGateway.Domain.Payments.Aggregates;

public class Payment : AggregateRoot<PaymentId>
{
    public Payment(Payments.Payment value, PaymentId id, Card card, TransactionTimeStamp timeStamp)
    {
        Value = value;
        Id = id;
        Card = card;
        TimeStamp = timeStamp;
    }

    public Payments.Payment Value { get; init; }
    public PaymentId Id { get; init; }
    public Card Card { get; init; }
    public TransactionTimeStamp TimeStamp { get; init; }

    public Status Status { get; private set; } = Status.ProcessingStarted;

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
            default: throw new InvalidPaymentStatusException();
        }
    }
}


using Amido.Stacks.Domain;
using Checkout.PaymentGateway.Common.Exceptions;
using Checkout.PaymentGateway.Domain.Payments.Events;
using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Domain.Payments.Aggregates;

public class PaymentRoot : AggregateRoot<PaymentId>
{
    [JsonConstructor]
    private PaymentRoot(PaymentId id, Payer payer, Merchant merchant, Payment value, TransactionTimeStamp timeStamp, Guid correlationId)
    {
        Value = value;
        Id = id;
        TimeStamp = timeStamp;
        Payer = payer;
        Merchant = merchant;
        CorrelationId = correlationId;
    }

    public static PaymentRoot Create(PaymentId id, Payer payer, Merchant merchant, Payment value, TransactionTimeStamp timeStamp, Guid correlationId)
    {
        return new PaymentRoot(id, payer, merchant, value, timeStamp, correlationId);
    }

    public Payment Value { get; init; }
    public Payer Payer { get; init; }
    public Merchant Merchant { get; init; }
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

    public bool IsProcessedPayment() => Status == Status.Successful;

    public override bool Equals(object obj)
    {
        var payment = obj as PaymentRoot;

        if (payment == null)
        {
            return false;
        }

        return Value == payment.Value && Payer == payment.Payer && Merchant == payment.Merchant && TimeStamp == payment.TimeStamp;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() + Payer.GetHashCode() + Merchant.GetHashCode() + TimeStamp.GetHashCode();
    }
}


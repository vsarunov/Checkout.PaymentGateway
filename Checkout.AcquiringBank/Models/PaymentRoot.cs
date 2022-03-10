using System.Text.Json.Serialization;

namespace Checkout.AcquiringBank.Models;

internal class PaymentRoot
{
    [JsonConstructor]
    private PaymentRoot(Guid id, Payer payer, Merchant merchant, Payment value, TransactionTimeStamp timeStamp, Guid correlationId)
    {
        Value = value;
        Id = id;
        TimeStamp = timeStamp;
        Payer = payer;
        Merchant = merchant;
        CorrelationId = correlationId;
    }

    public static PaymentRoot Create(Guid id, Payer payer, Merchant merchant, Payment value, TransactionTimeStamp timeStamp, Guid correlationId)
    {
        return new PaymentRoot(id, payer, merchant, value, timeStamp, correlationId);
    }

    public Guid Id { get; init; }
    public Payment Value { get; init; }
    public Payer Payer { get; init; }
    public Merchant Merchant { get; init; }
    public TransactionTimeStamp TimeStamp { get; init; }
    public Guid CorrelationId { get; init; }
}

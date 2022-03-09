using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Domain.Payments.Entities
{
    public class PaymentItem
    {
        [JsonConstructor]
        private PaymentItem(PaymentId id, Payer payer, Merchant merchant, Payment value, TransactionTimeStamp timeStamp, string status, Guid correlationId)
        {
            Value = value;
            Id = id;
            TimeStamp = timeStamp;
            Payer = payer;
            Merchant = merchant;
            CorrelationId = correlationId;
        }

        public static PaymentItem Create(PaymentId id, Payer payer, Merchant merchant, Payment value, TransactionTimeStamp timeStamp, string status, Guid correlationId)
        {
            return new PaymentItem(id, payer, merchant, value, timeStamp, status, correlationId);
        }

        public Payment Value { get; init; }
        public PaymentId Id { get; init; }
        public Payer Payer { get; init; }
        public Merchant Merchant { get; init; }
        public TransactionTimeStamp TimeStamp { get; init; }
        public string Status { get; init; }
        public Guid CorrelationId { get; init; }
    }
}

using System.Text.Json.Serialization;

namespace Checkout.AcquiringBank.Models
{
    internal class PaymentProcessingResult
    {
        [JsonConstructor]
        public PaymentProcessingResult(string paymentStatus)
        {
            PaymentStatus = paymentStatus;
        }

        public string PaymentStatus { get; init; }
    }
}

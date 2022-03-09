using System.Text.Json.Serialization;

namespace Checkout.AcquiringBank.Emulator.Models
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

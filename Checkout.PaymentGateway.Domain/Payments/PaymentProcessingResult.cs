﻿using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Domain.Payments
{
    public record PaymentProcessingResult
    {
        [JsonConstructor]
        public PaymentProcessingResult(string paymentStatus)
        {
            PaymentStatus = paymentStatus;
        }

        public string PaymentStatus { get; init; }
    }
}

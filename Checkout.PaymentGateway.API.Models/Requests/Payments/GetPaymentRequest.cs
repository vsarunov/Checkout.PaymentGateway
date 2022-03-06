using Checkout.PaymentGateway.API.Models.Shared.Payments;

namespace Checkout.PaymentGateway.API.Models.Requests.Payments;

public record struct GetPaymentRequest(PaymentId Id);


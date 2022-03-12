using Checkout.PaymentGateway.API.Models.Shared.Payments;

namespace Checkout.PaymentGateway.API.Models.Responses.Payments;

public record PaymentResponse(CardDto Card, PaymentDto Payment, PaymentStatusDto PaymentStatus);


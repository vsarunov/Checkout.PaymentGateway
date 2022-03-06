namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record struct CardDto(string Number, int CVV, ExpirationDateDto Expiration);


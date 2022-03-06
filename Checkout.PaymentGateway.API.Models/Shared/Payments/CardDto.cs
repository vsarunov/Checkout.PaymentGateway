namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record struct CardDto(string number, int CVV, ExpirationDateDto Expiration);


namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record CardDto(string Number, int CVV, CardExpirationDateDto Expiration);


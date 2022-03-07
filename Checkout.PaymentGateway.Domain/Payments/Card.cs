namespace Checkout.PaymentGateway.Domain.Payments;

public record struct Card(string Number, int CVV, CardExpirationDate Expiration);

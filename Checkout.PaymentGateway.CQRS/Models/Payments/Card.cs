namespace Checkout.PaymentGateway.CQRS.Models.Payments;

public record struct Card(string Number, int CVV, CardExpirationDate Expiration);


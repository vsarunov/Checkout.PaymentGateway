namespace Checkout.PaymentGateway.Application.CommandHandlers.Models.Payments;

public record struct Card(string Number, int CVV, CardExpirationDate Expiration);


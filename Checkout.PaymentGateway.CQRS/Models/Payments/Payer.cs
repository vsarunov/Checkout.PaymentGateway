namespace Checkout.PaymentGateway.CQRS.Models.Payments;

public record struct Payer(Address Address, Name Name, Card Card);

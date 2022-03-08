namespace Checkout.PaymentGateway.Domain.Payments;

public record struct Payer(Address Address, Name Name, Card Card);



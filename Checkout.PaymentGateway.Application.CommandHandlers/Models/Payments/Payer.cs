namespace Checkout.PaymentGateway.Application.CommandHandlers.Models.Payments;

public record struct Payer(Address Address, Name Name, Card Card);

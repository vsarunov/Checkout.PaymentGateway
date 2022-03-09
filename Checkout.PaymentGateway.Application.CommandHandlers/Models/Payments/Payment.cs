namespace Checkout.PaymentGateway.Application.CommandHandlers.Models.Payments;

public record struct Payment(decimal Amount, string ISOCurrencyCode);

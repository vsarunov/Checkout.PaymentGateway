namespace Checkout.PaymentGateway.CQRS.Models.Payments;

public record struct Payment(decimal Amount, string ISOCurrencyCode);

namespace Checkout.PaymentGateway.Domain.Payments;

public record struct Payment(decimal Amount, string ISOCurrencyCode);

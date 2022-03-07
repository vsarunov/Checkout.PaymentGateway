namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record struct PaymentDto(decimal Amount, string ISOCurrencyCode);

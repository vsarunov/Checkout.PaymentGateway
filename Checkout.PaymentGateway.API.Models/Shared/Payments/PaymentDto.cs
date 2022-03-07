namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record PaymentDto(decimal Amount, string ISOCurrencyCode);

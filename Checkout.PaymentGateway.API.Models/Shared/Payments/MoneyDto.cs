namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record struct MoneyDto(decimal Amount, string ISOCurrencyCode);

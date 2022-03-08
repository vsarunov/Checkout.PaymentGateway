namespace Checkout.PaymentGateway.Domain.Payments;

public record struct Merchant(MerchantId Id, Card card);

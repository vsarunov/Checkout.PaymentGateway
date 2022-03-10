namespace Checkout.PaymentGateway.CQRS.Models.Payments;

public record struct Merchant(MerchantId Id,Card Card);
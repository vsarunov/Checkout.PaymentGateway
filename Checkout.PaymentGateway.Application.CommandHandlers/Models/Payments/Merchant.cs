namespace Checkout.PaymentGateway.Application.CommandHandlers.Models.Payments;

public record struct Merchant(MerchantId Id,Card Card);
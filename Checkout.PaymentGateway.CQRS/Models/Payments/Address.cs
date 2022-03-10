namespace Checkout.PaymentGateway.CQRS.Models.Payments;

public record struct Address(string AddressLine, string PostCode);

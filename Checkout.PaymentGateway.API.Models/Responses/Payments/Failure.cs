namespace Checkout.PaymentGateway.API.Models.Responses.Payments;

public record struct Failure(string Id, Error Error);

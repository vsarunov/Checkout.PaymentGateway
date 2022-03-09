namespace Checkout.PaymentGateway.API.Models.Shared.Payments;

public record PayerDto(AddressDto Address, NameDto Name, CardDto Card);

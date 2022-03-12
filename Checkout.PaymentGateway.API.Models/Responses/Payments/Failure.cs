namespace Checkout.PaymentGateway.API.Models.Responses.Payments;

public class Failure
{
    public string Id { get; set; }
    public Error Error { get; set; }
}

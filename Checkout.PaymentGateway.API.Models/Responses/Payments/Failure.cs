namespace Checkout.PaymentGateway.API.Models.Responses.Payments
{
    public class Failure
    {
        public string Id { get; init; }
        public Error Error { get; init; }
    }
}

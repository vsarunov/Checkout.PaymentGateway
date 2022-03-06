namespace Checkout.PaymentGateway.Domain.Payments;

public enum Status
{
    ProcessingStarted = 0,
    Successful = 1,
    Rejected = 2,
    Failed = 3
}

namespace Checkout.PaymentGateway.Application.Integration.Payments.Services;

public interface IPaymentSearchService
{
    Task<Domain.Payments.Aggregates.PaymentRoot> SearchPayment(Domain.Payments.Aggregates.PaymentRoot payment);
}
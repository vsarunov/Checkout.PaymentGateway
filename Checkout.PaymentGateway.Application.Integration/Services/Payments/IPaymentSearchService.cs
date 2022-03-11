using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using LanguageExt;

namespace Checkout.PaymentGateway.Application.Integration.Payments.Services;

public interface IPaymentSearchService
{
    Option<PaymentRoot> SearchPayment(PaymentRoot payment);
}
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Responses.Payments;
using Checkout.PaymentGateway.Application.CommandHandlers.Payments;

namespace Checkout.PaymentGateway.API.Features.Payments.Mappings
{
    public class ProcessPaymentMapper : Mapper<ProcessPaymentRequest, Failure, ProcessPaymentCommand>
    {
    }
}

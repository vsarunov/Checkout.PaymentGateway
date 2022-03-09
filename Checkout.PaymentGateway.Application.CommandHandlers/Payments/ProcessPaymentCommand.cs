using Checkout.PaymentGateway.Common.Operations;
using MediatR;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments;

public class ProcessPaymentCommand : IRequest
{
    public OperationCode OperationCode { get; init; } = OperationCode.ProcessPayment;

    public Guid CorrelationId { get; init; }
}

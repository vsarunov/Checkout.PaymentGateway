using Checkout.PaymentGateway.Common.Operations;
using Checkout.PaymentGateway.CQRS.Models.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using LanguageExt;
using MediatR;

namespace Checkout.PaymentGateway.Application.QueryHandlers.Payments;

public class GetPaymentByIdQuery : IRequest<Option<PaymentRoot>>
{
    private GetPaymentByIdQuery(PaymentId id, Guid correlationId)
    {
        Id = id;
        CorrelationId = correlationId;
    }

    public static GetPaymentByIdQuery Create(PaymentId id, Guid correlationId) => new GetPaymentByIdQuery(id, correlationId);

    public PaymentId Id { get; init; }

    public OperationCode OperationCode { get; init; } = OperationCode.GetPaymentById;

    public Guid CorrelationId { get; init; }
}

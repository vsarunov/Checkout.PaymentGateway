using Checkout.PaymentGateway.Common.Operations;
using Checkout.PaymentGateway.CQRS.Models.Payments;
using Checkout.PaymentGateway.Domain.Shared;
using LanguageExt;
using MediatR;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments;

public class ProcessPaymentCommand : IRequest<Option<Failure>>
{
    private ProcessPaymentCommand(Guid correlationId, PaymentId id, Payer payer, Payment value, Merchant merchant, TransactionTimeStamp transactionTimeStamp)
    {
        CorrelationId = correlationId;
        Id = id;
        Payer = payer;
        Value = value;
        Merchant = merchant;
        TransactionTimeStamp = transactionTimeStamp;
    }

    public static ProcessPaymentCommand Create(Guid correlationId, PaymentId id, Payer payer, Payment value, Merchant merchant, TransactionTimeStamp transactionTimeStamp) => 
        new ProcessPaymentCommand(correlationId, id, payer, value, merchant, transactionTimeStamp);

    public OperationCode OperationCode { get; init; } = OperationCode.ProcessPayment;
    public Guid CorrelationId { get; init; }
    public PaymentId Id { get; init; }
    public Payer Payer { get; init; }
    public Payment Value { get; init; }
    public Merchant Merchant { get; init; }
    public TransactionTimeStamp TransactionTimeStamp { get; init; }
}

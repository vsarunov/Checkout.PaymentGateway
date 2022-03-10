using Checkout.PaymentGateway.Domain.Shared;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Option<Failure>>
{
    private readonly ILogger<ProcessPaymentCommandHandler> logger;

    public ProcessPaymentCommandHandler(ILogger<ProcessPaymentCommandHandler> logger)
    {
        logger = logger;
    }

    public Task<Option<Failure>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

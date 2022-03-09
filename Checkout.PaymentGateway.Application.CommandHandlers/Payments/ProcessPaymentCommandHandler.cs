using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand>
{
    private readonly ILogger<ProcessPaymentCommandHandler> logger;

    public ProcessPaymentCommandHandler(ILogger<ProcessPaymentCommandHandler> logger)
    {
        logger = logger;
    }

    public async Task<Unit> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

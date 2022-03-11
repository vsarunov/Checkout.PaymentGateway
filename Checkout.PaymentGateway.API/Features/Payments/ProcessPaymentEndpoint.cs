using Checkout.PaymentGateway.API.Features.Payments.Mappings;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using MediatR;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class ProcessPaymentEndpoint : Endpoint<ProcessPaymentRequest>
{
    private readonly ILogger<ProcessPaymentEndpoint> logger;
    private readonly IMediator mediator;

    public ProcessPaymentEndpoint(IMediator mediator, ILogger<ProcessPaymentEndpoint> logger)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/v1/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ProcessPaymentRequest req, CancellationToken ct)
    {
        logger.ReceivedProcessPaymentRequest();

        var command = req.ToCommand();

        var paymentProcessingResult = await mediator.Send(command);

        await paymentProcessingResult.MatchAsync(async failure =>
        {
            await SendAsync(failure.ToResponseFailure(), failure.ErrorCode.ToStatusCode());
        },
        async () => await SendAsync(StatusCodes.Status200OK));
    }
}


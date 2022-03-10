using Checkout.PaymentGateway.API.Features.Payments.Mappings;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Common.LogDefinitions;
using MediatR;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class ProcessPaymentEndpoint : Endpoint<ProcessPaymentRequest>
{
    public ILogger<ProcessPaymentEndpoint> logger { get; set; }
    public IMediator mediator { get; set; }

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

        await SendAsync(StatusCodes.Status200OK);
    }
}


using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Common.LogDefinitions;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class ProcessPaymentEndpoint : Endpoint<ProcessPaymentRequest>
{

    public ILogger<ProcessPaymentEndpoint> logger { get; set; }
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/v1/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ProcessPaymentRequest req, CancellationToken ct)
    {
        logger.ReceivedProcessPaymentRequest();

        await SendAsync(StatusCodes.Status200OK);
    }
}


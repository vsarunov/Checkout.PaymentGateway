using Checkout.PaymentGateway.API.Models.Requests.Payments;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class ProcessPaymentEndpoint : Endpoint<ProcessPaymentRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/v1/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ProcessPaymentRequest req, CancellationToken ct)
    {
        await SendAsync(StatusCodes.Status200OK);
    }
}


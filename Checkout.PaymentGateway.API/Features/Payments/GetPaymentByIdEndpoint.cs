using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Responses.Payments;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class GetPaymentByIdEndpoint : Endpoint<GetPaymentRequest, PaymentResponse>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/v1/payment/{id}");
        AllowAnonymous();
    }

    public override Task HandleAsync(GetPaymentRequest req, CancellationToken ct)
    {
        return base.HandleAsync(req, ct);
    }
}


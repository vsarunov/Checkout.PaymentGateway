using Checkout.PaymentGateway.API.Features.Payments.Mappings;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Responses.Payments;
using Checkout.PaymentGateway.Common.LogDefinitions;
using MediatR;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class GetPaymentByIdEndpoint : Endpoint<GetPaymentRequest, PaymentResponse>
{
    public ILogger<GetPaymentByIdEndpoint> logger { get; set; }
    public IMediator mediator { get; set; }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/v1/payments/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPaymentRequest req, CancellationToken ct)
    {
        logger.ReceivedGetPaymentByIdRequest();

        var query = req.ToQuery();

        var payment = await mediator.Send(query);

        await SendAsync(new PaymentResponse(), StatusCodes.Status200OK);
    }
}


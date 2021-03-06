using Checkout.PaymentGateway.API.Features.Payments.Mappings;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using MediatR;

namespace Checkout.PaymentGateway.API.Features.Payments;

public class GetPaymentByIdEndpoint : Endpoint<GetPaymentRequest>
{
    private readonly ILogger<GetPaymentByIdEndpoint> logger;
    private readonly IMediator mediator;
    
    public GetPaymentByIdEndpoint(IMediator mediator,ILogger<GetPaymentByIdEndpoint> logger)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

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

        await payment.Match(
            f => SendAsync(f.ToResponseFailure(), f.ErrorCode.ToStatusCode()),
            p => SendAsync(p.ToResponse(), StatusCodes.Status200OK));
    }
}


using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Common.Enums;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using Checkout.PaymentGateway.Domain.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Domain.Shared;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.QueryHandlers.Payments;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Either<PaymentRoot, Failure>>
{
    private readonly ILogger<GetPaymentByIdQueryHandler> logger;
    private readonly IPaymentRepository paymentRepository;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository, ILogger<GetPaymentByIdQueryHandler> logger)
    {
        this.logger = logger;
        this.paymentRepository = paymentRepository;
    }

    public async Task<Either<PaymentRoot, Failure>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var existingPayment = await paymentRepository.GetByIdAsync(new PaymentId(request.Id.Value));

        return existingPayment.Match<Either<PaymentRoot, Failure>>(
            x => x,
            () =>
            {
                logger.PaymentNotFound(request.Id.Value);
                return Failure.Of(request.Id.Value, ErrorCode.PaymentNotFound);
            });
    }
}

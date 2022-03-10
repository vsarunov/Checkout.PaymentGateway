using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Domain.Shared;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.QueryHandlers.Payments;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Option<PaymentRoot>>
{
    private readonly ILogger<GetPaymentByIdQueryHandler> logger;
    private readonly IPaymentRepository paymentRepository;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository, ILogger<GetPaymentByIdQueryHandler> logger)
    {
        this.logger = logger;
        this.paymentRepository = paymentRepository;
    }

    public Task<Option<PaymentRoot>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

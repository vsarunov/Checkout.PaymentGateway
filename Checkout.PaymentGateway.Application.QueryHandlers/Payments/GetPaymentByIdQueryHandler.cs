using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Application.QueryHandlers.Payments;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentRoot>
{
    private readonly ILogger<GetPaymentByIdQueryHandler> logger;

    public GetPaymentByIdQueryHandler(ILogger<GetPaymentByIdQueryHandler> logger)
    {
        logger = logger;
    }

    public async Task<PaymentRoot> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

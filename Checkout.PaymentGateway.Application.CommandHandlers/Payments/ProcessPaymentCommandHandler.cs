using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Domain.Shared;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Option<Failure>>
{
    private readonly ILogger<ProcessPaymentCommandHandler> logger;
    private readonly IPaymentSearchService paymentSearchService;
    private readonly IBankService bankService;

    public ProcessPaymentCommandHandler(IPaymentSearchService paymentSearchService, IBankService bankService, ILogger<ProcessPaymentCommandHandler> logger)
    {
        this.logger = logger;
        this.paymentSearchService = paymentSearchService;
        this.bankService = bankService;
    }

    public Task<Option<Failure>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

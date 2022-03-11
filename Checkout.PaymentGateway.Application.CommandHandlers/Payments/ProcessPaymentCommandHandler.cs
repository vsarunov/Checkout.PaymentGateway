using Checkout.PaymentGateway.Application.CommandHandlers.Payments.Mappers;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Common.Enums;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
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

    public async Task<Option<Failure>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = request.ToDomainPayment();

        var paymentSearchResult = paymentSearchService.SearchPayment(payment);

        return await paymentSearchResult.MatchAsync(x =>
        {
            logger.PaymentAlreadyExists(payment.Id.Value);
            return Failure.Of(payment.Id.Value, ErrorCode.PaymentAlreadyExists);
        },
        async () =>
        {

            var processPaymentResult = await bankService.ProcessPayment(payment);

            if (processPaymentResult.IsRejected())
            {
                logger.PaymentRejected(payment.Id.Value);
                return Failure.Of(payment.Id.Value, ErrorCode.PaymentRejected);
            }

            if (processPaymentResult.IsFailed())
            {
                logger.PaymentFailed(payment.Id.Value);
                return Failure.Of(payment.Id.Value, ErrorCode.PaymentFailed);
            }

            return Option<Failure>.None;
        });     
    }
}

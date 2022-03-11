using Checkout.PaymentGateway.Application.CommandHandlers.Payments.Mappers;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Common.Enums;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Domain.Shared;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.CommandHandlers.Payments;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Option<Failure>>
{
    private readonly ILogger<ProcessPaymentCommandHandler> logger;
    private readonly IPaymentSearchService paymentSearchService;
    private readonly IPaymentRepository paymentRepository;
    private readonly IBankService bankService;

    public ProcessPaymentCommandHandler(IPaymentSearchService paymentSearchService, IPaymentRepository paymentRepository, IBankService bankService, ILogger<ProcessPaymentCommandHandler> logger)
    {
        this.logger = logger;
        this.paymentSearchService = paymentSearchService;
        this.bankService = bankService;
        this.paymentRepository = paymentRepository;
    }

    public async Task<Option<Failure>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = request.ToDomainPayment();

        var paymentSearchResult = paymentSearchService.SearchPayment(payment);

        return await paymentSearchResult.MatchAsync(async x =>
        {
            if (x.IsProcessedPayment())
            {
                logger.PaymentHasBeenProcessed(payment.Id.Value);
                return CreateFailure(payment.Id.Value, ErrorCode.PaymentAlreadyExists);
            }

            logger.ProcessingPaymentPreviouslyProcessedUnsuccessful(payment.Id.Value);
            return await ProcessPayment(payment);
        },
        async () =>
        {

            var processPaymentResult = await bankService.ProcessPayment(payment);

            if (processPaymentResult.IsRejected())
            {
                logger.PaymentRejected(payment.Id.Value);
                return CreateFailure(payment.Id.Value, ErrorCode.PaymentRejected);
            }

            if (processPaymentResult.IsFailed())
            {
                logger.PaymentFailed(payment.Id.Value);
                return CreateFailure(payment.Id.Value, ErrorCode.PaymentFailed);
            }



            return Option<Failure>.None;
        });
    }

    private async Task<Option<Failure>> ProcessPayment(PaymentRoot payment)
    {

    }

    private void SavePayment(PaymentRoot payment) => paymentRepository.SaveAsync(payment);
    private void UpdatePayment(PaymentRoot payment) => paymentRepository.UpdatePayment(payment);

    private static Failure CreateFailure(Guid id, ErrorCode errorCode) => Failure.Of(id, errorCode);
}

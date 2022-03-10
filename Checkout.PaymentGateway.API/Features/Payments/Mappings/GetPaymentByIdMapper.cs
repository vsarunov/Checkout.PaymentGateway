using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.Application.QueryHandlers.Payments;

namespace Checkout.PaymentGateway.API.Features.Payments.Mappings
{
    internal static class GetPaymentByIdMapper
    {
        internal static GetPaymentByIdQuery ToQuery(this GetPaymentRequest request) =>
            GetPaymentByIdQuery.Create(new CQRS.Models.Payments.PaymentId(request.Id.Value), request.CorrelationId);
    }

    //internal static PaymentResponse ToResponse(this PaymentRoot payment)
    //{

    //}
}

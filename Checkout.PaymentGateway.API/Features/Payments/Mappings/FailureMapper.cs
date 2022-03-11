using Checkout.PaymentGateway.API.Models.Responses.Payments;
using Checkout.PaymentGateway.Common.Enums;

namespace Checkout.PaymentGateway.API.Features.Payments.Mappings
{
    internal static class FailureMapper
    {
        internal static Failure ToResponseFailure(this Domain.Shared.Failure failure)
        {
            return new Failure(failure.Id, new Error(failure.ErrorCode.ToString()));
        }

        internal static int ToStatusCode(this ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.PaymentRejected: return StatusCodes.Status400BadRequest;
                case ErrorCode.PaymentFailed: return StatusCodes.Status500InternalServerError;
                case ErrorCode.PaymentAlreadyExists: return StatusCodes.Status409Conflict;
                case ErrorCode.PaymentNotFound: return StatusCodes.Status404NotFound;
                default: throw new ArgumentOutOfRangeException(nameof(errorCode));
            }
        }
    }
}

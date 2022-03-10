using System.Net;

namespace Checkout.PaymentGateway.Common.Exceptions;

public static class ExceptionCodeToHttpStatusCodeConverter
{
    internal static HttpStatusCode GetHttpStatusCode(ExceptionCode exceptionCode)
    {
        switch (exceptionCode)
        {
            case ExceptionCode.InvalidPaymentStatus:
                return HttpStatusCode.BadRequest;
            case ExceptionCode.PaymentAlreadyExists:
                return HttpStatusCode.BadRequest;
            default: return HttpStatusCode.InternalServerError;
        }
    }
}

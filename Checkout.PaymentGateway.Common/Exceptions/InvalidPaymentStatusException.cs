using Amido.Stacks.Core.Exceptions;

namespace Checkout.PaymentGateway.Common.Exceptions;

public class InvalidPaymentStatusException : ApplicationExceptionBase
{
    private InvalidPaymentStatusException(Exceptions.ExceptionCode exceptionCode, string message) : base(message)
    {
        HttpStatusCode = (int)ExceptionCodeToHttpStatusCodeConverter.GetHttpStatusCode(exceptionCode);
    }

    public override int ExceptionCode { get; protected set; }

    public static void Raise()
    {
        var exception = new InvalidPaymentStatusException(
            Exceptions.ExceptionCode.InvalidPaymentStatus,
            "Payment status supplied is not defined."
            );

        throw exception;
    }
}

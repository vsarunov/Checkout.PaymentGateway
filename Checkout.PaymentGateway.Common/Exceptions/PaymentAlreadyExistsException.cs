using Amido.Stacks.Core.Exceptions;

namespace Checkout.PaymentGateway.Common.Exceptions;

public class PaymentAlreadyExistsException : ApplicationExceptionBase
{
    private PaymentAlreadyExistsException(Exceptions.ExceptionCode exceptionCode, string message) : base(message)
    {
        HttpStatusCode = (int)ExceptionCodeToHttpStatusCodeConverter.GetHttpStatusCode(exceptionCode);
    }

    public override int ExceptionCode { get; protected set; }

    public static void Raise()
    {
        var exception = new PaymentAlreadyExistsException(
            Exceptions.ExceptionCode.PaymentAlreadyExists,
            "Payment already exists."
            );

        throw exception;
    }
}

using Checkout.PaymentGateway.Common.Enums;

namespace Checkout.PaymentGateway.Domain.Shared;

public class Failure
{
    public string Id { get; }
    public ErrorCode ErrorCode { get; }

    private Failure(object id, ErrorCode errorCode)
    {
        Id = id.ToString();
        ErrorCode = errorCode;
    }

    public static Failure Of(object id, ErrorCode errorCode) => new Failure(id, errorCode);
}

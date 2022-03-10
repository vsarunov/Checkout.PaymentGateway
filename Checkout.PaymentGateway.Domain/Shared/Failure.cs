using LanguageExt.Common;

namespace Checkout.PaymentGateway.Domain.Shared;

public class Failure
{
    public string Id { get; }
    public Error Error { get; }

    private Failure(object id, Error error)
    {
        Id = id.ToString();
        Error = error;
    }

    public static Failure Of(object id, Error error) => new Failure(id, error);

    public static Failure Of(object id, string error) => new Failure(id, Error.New($"{error}"));

    public static Func<Error, Failure> Of(object id) => error => new Failure(id, error);
}

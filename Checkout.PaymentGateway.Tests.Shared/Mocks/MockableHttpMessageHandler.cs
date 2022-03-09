namespace Checkout.PaymentGateway.Tests.Shared.Mocks;

public class MockableHttpMessageHandler : DelegatingHandler
{
    public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        throw new NotImplementedException("This method should only ever be mocked, not consumed directly");
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return SendAsync(request);
    }
}

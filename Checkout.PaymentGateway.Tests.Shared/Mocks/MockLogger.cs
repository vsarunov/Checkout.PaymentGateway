using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Tests.Shared.Mocks;

public abstract class MockLogger<T> : ILogger<T>
{
    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) =>
 Log(logLevel, eventId, formatter(state, exception));

    public abstract void Log(LogLevel logLevel, EventId eventId, string message);

    public virtual bool IsEnabled(LogLevel logLevel) => true;

    public abstract IDisposable BeginScope<TState>(TState state);
}

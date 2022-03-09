using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Common.LogDefinitions;

public static class LoggingExtensions
{
    // Verbose/debug

    public static void ReceivedGetPaymentByIdRequest(this ILogger logger) =>
        Definitions.ReceivedGetPaymentByIdRequest(logger, null);

    public static void ReceivedProcessPaymentRequest(this ILogger logger) =>
        Definitions.ReceivedProcessPaymentRequest(logger, null);

    public static void GettingPaymentByIdFromInMemoryRepository(this ILogger logger) =>
        Definitions.GettingPaymentByIdFromInMemoryRepository(logger, null);

    public static void SavingPaymentByIdFromInMemoryRepository(this ILogger logger) =>
        Definitions.SavingPaymentByIdFromInMemoryRepository(logger, null);

    // Information

    // Warning

    // Error
    public static void FailedBankPaymentProcessing(this ILogger logger, int httpStatusCode) =>
        Definitions.FailedBankPaymentProcessing(logger, httpStatusCode, null);

    // Critical
}

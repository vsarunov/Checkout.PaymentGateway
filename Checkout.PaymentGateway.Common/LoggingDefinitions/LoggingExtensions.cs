using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Common.LoggingDefinitions;

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
    
    public static void SearchingForPayment(this ILogger logger) =>
        Definitions.SearchingForPayment(logger, null);

    // Information

    // Warning

    // Error
    public static void FailedBankPaymentProcessing(this ILogger logger, int httpStatusCode) =>
        Definitions.FailedBankPaymentProcessing(logger, httpStatusCode, null);
    
    public static void PaymentAlreadyExists(this ILogger logger, Guid id) =>
        Definitions.PaymentAlreadyExists(logger, id, null);

    public static void PaymentRejected(this ILogger logger, Guid id) =>
        Definitions.PaymentRejected(logger, id, null);

    public static void PaymentFailed(this ILogger logger, Guid id) =>
        Definitions.PaymentFailed(logger, id, null);

    // Critical
}

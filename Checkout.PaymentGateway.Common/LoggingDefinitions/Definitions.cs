using Microsoft.Extensions.Logging;
using Checkout.PaymentGateway.Common.Events;

namespace Checkout.PaymentGateway.Common.LoggingDefinitions;

internal static class Definitions
{
    // Verbose/debug
    internal static readonly Action<ILogger, Exception> ReceivedGetPaymentByIdRequest = LoggerMessage.Define(
        LogLevel.Debug,
        EventIds.ReceivedGetPaymentByIdRequest,
        "Received GetPaymentByIdRequest");

    internal static readonly Action<ILogger, Exception> ReceivedProcessPaymentRequest = LoggerMessage.Define(
        LogLevel.Debug,
        EventIds.ReceivedProcessPaymentRequest,
        "Received ProcessPaymentRequest");

    internal static readonly Action<ILogger, Exception> GettingPaymentByIdFromInMemoryRepository = LoggerMessage.Define(
        LogLevel.Debug,
        EventIds.GettingPaymentByIdFromInMemoryRepository,
        "Getting Payment By Id From In Memory Repository");

    internal static readonly Action<ILogger, Exception> SavingPaymentByIdFromInMemoryRepository = LoggerMessage.Define(
        LogLevel.Debug,
        EventIds.SavingPaymentByIdFromInMemoryRepository,
        "Saving Payment.");

    internal static readonly Action<ILogger, Exception> UpdatingPaymentByIdFromInMemoryRepository = LoggerMessage.Define(
        LogLevel.Debug,
        EventIds.UpdatingPaymentByIdFromInMemoryRepository,
        "Updating Payment.");

    internal static readonly Action<ILogger, Exception> SearchingForPayment = LoggerMessage.Define(
        LogLevel.Debug,
        EventIds.SearchingForPayment,
        "Searching for payment");

    // Information

    internal static readonly Action<ILogger, Guid, Exception> PaymentNotFound = LoggerMessage.Define<Guid>(
        LogLevel.Information,
        EventIds.PaymentNotFound,
        "Payment Not Found. PaymentId: {Id}");

    // Warning

    internal static readonly Action<ILogger, Guid, Exception> ProcessingPaymentPreviouslyProcessedUnsuccessful = LoggerMessage.Define<Guid>(
    LogLevel.Warning,
    EventIds.ProcessingPaymentPreviouslyProcessedUnsuccessful,
    "Processing Payment Previously Processed Unsuccessful. PaymentId: {Id}");

    // Error
    internal static readonly Action<ILogger, int, Exception> FailedBankPaymentProcessing = LoggerMessage.Define<int>(
        LogLevel.Error,
        EventIds.FailedBankPaymentProcessing,
        "Bank processing failed. HttpStatusCode: {HttpStatusCode}");

    internal static readonly Action<ILogger, Guid, Exception> PaymentHasBeenProcessed = LoggerMessage.Define<Guid>(
        LogLevel.Error,
        EventIds.PaymentHasBeenProcessed,
        "Payment Already Exists. PaymentId: {Id}");

    internal static readonly Action<ILogger, Guid, Exception> PaymentRejected = LoggerMessage.Define<Guid>(
        LogLevel.Error,
        EventIds.PaymentRejected,
        "Payment Rejected. PaymentId: {Id}");

    internal static readonly Action<ILogger, Guid, Exception> PaymentFailed = LoggerMessage.Define<Guid>(
        LogLevel.Error,
        EventIds.PaymentFailed,
        "PaymentFailed. PaymentId: {Id}");

    // Critical
}

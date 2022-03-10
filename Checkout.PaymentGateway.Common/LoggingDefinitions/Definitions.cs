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
        "Saving Payment By Id From In Memory Repository");

    // Information

    // Warning

    // Error
    internal static readonly Action<ILogger, int, Exception> FailedBankPaymentProcessing = LoggerMessage.Define<int>(
        LogLevel.Error,
        EventIds.FailedBankPaymentProcessing,
        "Bank processing failed. HttpStatusCode: {HttpStatusCode}");

    // Critical
}

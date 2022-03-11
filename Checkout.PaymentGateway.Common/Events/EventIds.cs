using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Common.Events;

public static class EventIds
{
    // Verbose/debug 6000-6099
    public static readonly EventId ReceivedGetPaymentByIdRequest = new EventId(6000, nameof(ReceivedGetPaymentByIdRequest));
    public static readonly EventId ReceivedProcessPaymentRequest = new EventId(6001, nameof(ReceivedProcessPaymentRequest));
    public static readonly EventId GettingPaymentByIdFromInMemoryRepository = new EventId(6002, nameof(GettingPaymentByIdFromInMemoryRepository));
    public static readonly EventId SavingPaymentByIdFromInMemoryRepository = new EventId(6003, nameof(SavingPaymentByIdFromInMemoryRepository));
    public static readonly EventId SearchingForPayment = new EventId(6004, nameof(SearchingForPayment));

    // Information 6100-6199
    public static readonly EventId PaymentNotFound = new EventId(6100, nameof(PaymentNotFound));

    // Warning 6200-6299

    // Error 6300-6399
    public static readonly EventId FailedBankPaymentProcessing = new EventId(6300, nameof(FailedBankPaymentProcessing));
    public static readonly EventId PaymentAlreadyExists = new EventId(6301, nameof(PaymentAlreadyExists));
    public static readonly EventId PaymentRejected = new EventId(6302, nameof(PaymentRejected));
    public static readonly EventId PaymentFailed = new EventId(6303, nameof(PaymentFailed));

    // Critical 6400-6499
}

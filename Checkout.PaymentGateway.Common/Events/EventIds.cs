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

    // Warning 6200-6299

    // Error 6300-6399
    public static readonly EventId FailedBankPaymentProcessing = new EventId(6300, nameof(FailedBankPaymentProcessing));

    // Critical 6400-6499
}

namespace Checkout.AcquiringBank.Models;

internal record struct Payment(decimal Amount, string ISOCurrencyCode);

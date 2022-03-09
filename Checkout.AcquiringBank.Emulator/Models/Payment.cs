namespace Checkout.AcquiringBank.Emulator.Models;

internal record struct Payment(decimal Amount, string ISOCurrencyCode);

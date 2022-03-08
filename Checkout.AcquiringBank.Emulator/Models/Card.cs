namespace Checkout.AcquiringBank.Emulator.Models;

internal record struct Card(string Number, int CVV, CardExpirationDate Expiration);

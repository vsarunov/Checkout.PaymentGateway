namespace Checkout.AcquiringBank.Models;

internal record struct Card(string Number, int CVV, CardExpirationDate Expiration);

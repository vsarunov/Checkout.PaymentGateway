using Checkout.AcquiringBank.Configuration;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Domain.Payments;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Checkout.AcquiringBank.Services
{
    public class AcquiringBankService : IBankService
    {
        private readonly HttpClient httpClient;
        private readonly BankDetails bankDetails;
        private readonly ILogger<AcquiringBankService> logger;

        public AcquiringBankService(HttpClient httpClient, ILogger<AcquiringBankService> logger, IOptions<BankDetails> bankDetails)
        {
            this.httpClient = httpClient;
            this.bankDetails = bankDetails.Value;
            this.logger = logger;
        }

        public Task<PaymentProcessingResult> ProcessPayment(PaymentGateway.Domain.Payments.Aggregates.PaymentRoot payment)
        {
            throw new NotImplementedException();
        }
    }
}

using Checkout.AcquiringBank.Configuration;
using Checkout.AcquiringBank.Mappers;
using Checkout.AcquiringBank.Models;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Common.LoggingDefinitions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Domain = Checkout.PaymentGateway.Domain.Payments;

namespace Checkout.AcquiringBank.Services;

public class AcquiringBankService : IBankService
{
    private const string ContentType = "application/json";

    private readonly HttpClient httpClient;
    private readonly BankDetails bankDetails;
    private readonly ILogger<AcquiringBankService> logger;

    public AcquiringBankService(HttpClient httpClient, ILogger<AcquiringBankService> logger, IOptions<BankDetails> bankDetails)
    {
        this.httpClient = httpClient;
        this.bankDetails = bankDetails.Value;
        this.logger = logger;
    }

    public async Task<Domain.PaymentProcessingResult> ProcessPayment(Domain.Aggregates.PaymentRoot payment)
    {
        var paymentRequest = payment.ToBankPayment();

        var uri = $"{bankDetails.Url}/v1/process-payment";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, ContentType);

        var response = await httpClient.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            logger.FailedBankPaymentProcessing((int)response.StatusCode);
            return new Domain.PaymentProcessingResult(Domain.Status.Failed);
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        var paymentProcessingResult = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentProcessingResult>(responseBody);

        return paymentProcessingResult.MapToDomain();
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Checkout.PaymentGateway.API.Models.Shared.Payments;
using Shouldly;

namespace Checkout.PaymentGateway.API.ComponentTests.Fixtures
{
    public abstract class ApiClientFixture : ApiFixture
    {

        protected ApiClientFixture()
        {
        }

        public async Task<HttpResponseMessage> CreatePaymentAsync(ProcessPaymentRequest payment)
        {
            return await SendAsync(HttpMethod.Post, "/v1/payments", payment);
        }

        public async Task<HttpResponseMessage> GetPaymentByIdAsync(PaymentId paymentId)
        {
            return await SendAsync(HttpMethod.Post, $"/v1/payments/{paymentId}");
        }

        internal void ThenASuccessfulResponseIsReturned()
        {
            LastResponse.IsSuccessStatusCode.ShouldBeTrue();
        }

        internal void ThenAFailureResponseIsReturned()
        {
            LastResponse.IsSuccessStatusCode.ShouldBeFalse();
        }

        internal void ThenAServiceUnavailableIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        }

        internal void ThenABadRequestResponseIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        internal void ThenAForbiddenResponseIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        internal void ThenACreatedResponseIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        internal void ThenANotFoundResponseIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        internal void ThenAConflictResponseIsReturned()
        {
            LastResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        }
    }
}

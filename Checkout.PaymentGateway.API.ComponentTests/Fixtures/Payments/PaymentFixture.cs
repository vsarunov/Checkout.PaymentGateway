using Checkout.PaymentGateway.API.Models.Requests.Payments;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments
{
    public class PaymentFixture : ApiClientFixture
    {
        private readonly ProcessPaymentRequest payment;

        public PaymentFixture(ProcessPaymentRequest payment) : base()
        {
            this.payment = payment;
        }

        protected override void RegisterDependencies(IServiceCollection collection)
        {
            
        }

        internal async Task WhenISubmitThePayment()
        {
            await CreatePaymentAsync(payment);
        }

        internal void GivenAValidPayment()
        {

        }
    }
}

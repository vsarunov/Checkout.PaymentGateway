using Checkout.PaymentGateway.API.ComponentTests.Fixtures.Payments;
using Checkout.PaymentGateway.API.ComponentTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbehave;
using Xunit;

namespace Checkout.PaymentGateway.API.ComponentTests.Features.Payments
{
    [Trait("TestType", "ComponentTests")]
    public class ProcessPaymentFeature
    {
        [Scenario, CustomAutoData]
        public void ProcessValidPayment(PaymentFixture fixture)
        {
            "Given a valid payment".x(() => fixture.GivenAValidPayment());
            "When I submit the payment".x(() => fixture.WhenISubmitThePayment());
            "Then a successful response is returned".x(fixture.ThenASuccessfulResponseIsReturned);
        }

        //[Scenario, CustomAutoData]
        //public void ProcessInvalidPayment(PaymentFixture fixture)
        //{

        //}

        //[Scenario, CustomAutoData]
        //public void PaymentRejectedByBank(PaymentFixture fixture)
        //{

        //}
    }
}

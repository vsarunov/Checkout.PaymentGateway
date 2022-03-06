using AutoFixture;
using AutoFixture.Xunit2;
using Checkout.PaymentGateway.API.ComponentTests.Specimens;

namespace Checkout.PaymentGateway.API.ComponentTests.Setup
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(Customizations)
        {
        }

        public static IFixture Customizations()
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(new ProcessPaymentRequestSpecimenBuilder());

            return fixture;
        }
    }
}

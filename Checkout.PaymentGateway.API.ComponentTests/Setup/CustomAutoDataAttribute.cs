using AutoFixture;
using AutoFixture.Xunit2;

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

            return fixture;
        }
    }
}

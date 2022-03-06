using AutoFixture.Xunit2;

namespace Checkout.PaymentGateway.API.ComponentTests.Setup
{
    public class CustomInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public CustomInlineAutoDataAttribute(params object[] values)
            : base(new CustomAutoDataAttribute(), values)
        {
        }
    }
}

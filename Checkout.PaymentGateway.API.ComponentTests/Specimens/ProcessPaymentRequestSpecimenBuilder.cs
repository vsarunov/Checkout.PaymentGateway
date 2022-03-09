using AutoFixture.Kernel;
using Checkout.PaymentGateway.API.Models.Requests.Payments;
using System;
using Checkout.PaymentGateway.Tests.Shared.ModelFactories;

namespace Checkout.PaymentGateway.API.ComponentTests.Specimens;

public class ProcessPaymentRequestSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var type = request as Type;

        if (type == null || type != typeof(ProcessPaymentRequest))
        {
            return new NoSpecimen();
        }

        return ProcessPaymentRequestFactory.CreateDefaultPaymentRequest();
    }
}

using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Application.QueryHandlers.Payments;
using Checkout.PaymentGateway.Tests.Shared.Mocks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Application.QueryHandlers.UnitTests.Payments
{
    public class GetPaymentByIdQueryHandlerTests
    {
        private readonly GetPaymentByIdQueryHandler sut;
        private readonly MockLogger<GetPaymentByIdQueryHandler> mockLogger;
        private readonly IPaymentRepository paymentRepository;

        public GetPaymentByIdQueryHandlerTests()
        {
            paymentRepository = Substitute.For<IPaymentRepository>();

            mockLogger = Substitute.For<MockLogger<GetPaymentByIdQueryHandler>>();
            mockLogger.IsEnabled(Arg.Any<LogLevel>()).ReturnsForAnyArgs(true);

            sut = new GetPaymentByIdQueryHandler(paymentRepository, mockLogger);
        }
    }
}

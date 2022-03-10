global using FastEndpoints;
global using FastEndpoints.Validation;
using Amido.Stacks.API.Middleware;
using Checkout.AcquiringBank.Configuration;
using Checkout.AcquiringBank.Services;
using Checkout.PaymentGateway.Application.CommandHandlers.Payments;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Application.QueryHandlers.Payments;
using Checkout.PaymentGateway.Domain.Payments.Aggregates;
using Checkout.PaymentGateway.Infrastructure.Repositories.Payments;
using Checkout.PaymentGateway.Infrastructure.Services.Payments;
using FastEndpoints.Swagger;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDoc();

builder.Services.AddMediatR(typeof(ProcessPaymentCommandHandler).Assembly, typeof(GetPaymentByIdQueryHandler).Assembly);
builder.Services.Configure<BankDetails>(builder.Configuration.GetSection("BankDetails"));
builder.Services.AddSingleton<Dictionary<Guid, PaymentRoot>>(new Dictionary<Guid, PaymentRoot>());
builder.Services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
builder.Services.AddSingleton<IPaymentSearchService, PaymentSearchService>();
builder.Services.AddHttpClient<IBankService, AcquiringBankService>();

var app = builder.Build();

app.UseCorrelationId();
app.UseCustomExceptionHandler(app.Logger);
app.UseAuthorization();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());

await app.RunAsync();

public partial class Program
{
}
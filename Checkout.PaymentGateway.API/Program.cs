global using FastEndpoints;
global using FastEndpoints.Validation;
using Amido.Stacks.API.Middleware;
using Checkout.AcquiringBank.Configuration;
using Checkout.AcquiringBank.Services;
using Checkout.PaymentGateway.Application.Integration.Payments.Services;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Infrastructure.Repositories.Payments;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDoc();

builder.Services.Configure<BankDetails>(builder.Configuration.GetSection("BankDetails"));
builder.Services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
builder.Services.AddHttpClient<IBankService, AcquiringBankService>();

var app = builder.Build();

app.UseCorrelationId();
app.UseCustomExceptionHandler(app.Logger);
app.UseAuthorization();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());

await app.RunAsync();

public partial class Program { }
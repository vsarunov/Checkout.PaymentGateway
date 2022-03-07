global using FastEndpoints;
global using FastEndpoints.Validation;
using Checkout.PaymentGateway.Application.Integration.Repositories.Payments;
using Checkout.PaymentGateway.Infrastructure.Repositories.Payments;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDoc();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();

var app = builder.Build();

app.UseAuthorization();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());

await app.RunAsync();

public partial class Program { }
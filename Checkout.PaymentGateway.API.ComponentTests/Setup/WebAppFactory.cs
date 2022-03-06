using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.API.ComponentTests.Setup
{
    public class WebAppFactory : WebApplicationFactory<Program>
    {
        private readonly Action<IServiceCollection> configureTestServices;
        private readonly Action<IWebHostBuilder> configureBuilder;

        public WebAppFactory(Action<IServiceCollection> configureTestServices, Action<IWebHostBuilder> configureBuilder)
        {
            this.configureTestServices = configureTestServices;
            this.configureBuilder = configureBuilder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(configureTestServices);
        }
    }
}

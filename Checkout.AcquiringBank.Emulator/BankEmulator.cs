using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Checkout.AcquiringBank.Emulator.Models;
using System.Text.Json;

namespace Checkout.AcquiringBank.Emulator
{
    public static class BankEmulator
    {
        [FunctionName("BankEmulator")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/process-payment")] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var requestModel = JsonSerializer.Deserialize<PaymentRoot>(requestBody);
            
            if (requestModel.Payer.Name.FirstName == "Mike")
            {
                return new OkObjectResult(new PaymentProcessingResult("Rejected"));
            }
            
            if (requestModel.Payer.Name.FirstName == "Olivia")
            {
                return new BadRequestObjectResult(new PaymentProcessingResult("Failed"));
            }
            
            return new OkObjectResult(new PaymentProcessingResult("Successful"));
        }
    }
}

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Checkout.AcquiringBank.Emulator.Models;

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
            var requestMoel = JsonConvert.DeserializeObject<PaymentRoot>(requestBody);

            return new OkResult();
        }
    }
}

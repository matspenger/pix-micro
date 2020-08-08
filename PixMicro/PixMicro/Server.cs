using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PixMicro.Transforms;
using PixMicro.Core;

namespace PixMicro
{
    public static class Server
    {
        [FunctionName("Flip")]
        public static async Task<IActionResult> Flip(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using (var reader = new StreamReader(req.Body))
            {
                dynamic content = await reader.ReadToEndAsync();

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new Base64ImageJsonConverter());
                var flip = JsonConvert.DeserializeObject<Flip>(content, settings);

                var outputImg = flip.Apply();
                var response = new ImageResponse(outputImg);
                return new OkObjectResult(JsonConvert.SerializeObject(response, settings));
            }
        }
    }
}

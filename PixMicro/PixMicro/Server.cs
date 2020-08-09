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
    /// <summary>
    /// Class that defines all endpoints on API.
    /// </summary>
    /// <remarks>
    /// All endpoints expect POST requests with a JSON payload containing
    /// transform/edit parameters and a base64-encoded input image (always under the
    /// InputImage attribute).
    /// </remarks>
    public static class Server
    {
        /// <summary>
        /// Performs a flip, horizontal, vertical, or both.
        /// </summary>
        /// <remarks>
        /// Request format:
        /// {
        ///     "Mode": // "X" for horizontal flip, "Y" for vertical flip, "XY" for both,
        ///     "InputImage": // Image encoded as a base64 string
        ///     "OutputEncoding": // Optional, default is ".jpg"
        /// }
        /// 
        /// Response format:
        /// {
        ///     "OutputImage": // Image as a result of flip, again, base64 encoded.
        /// }
        /// </remarks>
        /// <param name="req">Request object</param>
        /// <param name="log">Log provider</param>
        /// <returns>Response message, containing Base64 encoded image with transform applied</returns>
        [FunctionName("Flip")]
        public static async Task<IActionResult> Flip(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using (var reader = new StreamReader(req.Body))
            {
                string content = await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(content))
                {
                    return new BadRequestObjectResult("Request body was empty");
                }
                else
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new Base64ImageJsonConverter());
                    var flip = JsonConvert.DeserializeObject<Flip>(content, settings);

                    var outputImg = flip.Apply();
                    var response = new ImageResponse(outputImg);
                    return new OkObjectResult(JsonConvert.SerializeObject(response, settings));
                }
            }
        }

        /// <summary>
        /// Rotates an image around its centre.
        /// </summary>
        /// <remarks>
        /// Request format:
        /// {
        ///     "RotateByDeg": // Angle in degrees to rotate by. Positive values rotate counter-clockwise.
        ///     "InputImage": // Image encoded as a base64 string
        ///     "OutputEncoding": // Optional, default is ".jpg"
        /// }
        /// 
        /// Response format:
        /// {
        ///     "OutputImage": // Image as a result of flip, again, base64 encoded.
        /// }
        /// </remarks>
        /// <param name="req">Request object</param>
        /// <param name="log">Log provider</param>
        /// <returns>Response message, containing Base64 encoded image with transform applied</returns>
        [FunctionName("Rotate")]
        public static async Task<IActionResult> Rotate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using (var reader = new StreamReader(req.Body))
            {
                string content = await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(content))
                {
                    return new BadRequestObjectResult("Request body was empty");
                }
                else
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.Converters.Add(new Base64ImageJsonConverter());
                    var rotate = JsonConvert.DeserializeObject<Rotate>(content, settings);

                    var outputImg = rotate.Apply();
                    var response = new ImageResponse(outputImg);
                    return new OkObjectResult(JsonConvert.SerializeObject(response, settings));
                }
            }
        }
    }
}

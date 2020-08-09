using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PixMicro.Core;
using PixMicro.Transforms;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

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
            try
            {
                var flip = await DeserializeMessageBody<Flip>(req);
                var outputImg = flip.Apply();
                return CreateResponse(outputImg);
            }
            catch (ArgumentException e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new InternalServerErrorResult();
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
            try
            {
                var rotate = await DeserializeMessageBody<Rotate>(req);
                var outputImg = rotate.Apply();
                return CreateResponse(outputImg);
            }
            catch (ArgumentException e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new InternalServerErrorResult();
            }
        }

        /// <summary>
        /// Scales/resizes an image.
        /// </summary>
        /// <remarks>
        /// Request format:
        /// {
        ///     "Mode": // "Pixels" or "Factor".
        ///     "Width": // Width of output image, when mode is "Pixels".
        ///     "Height": // Height of output image, when mode is "Pixels".
        ///     "Factor": // Scale factor - when mode is "Factor". 0.5 for half size, 2.0 for double size.
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
        [FunctionName("Scale")]
        public static async Task<IActionResult> Scale(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var scale = await DeserializeMessageBody<Scale>(req);
                var outputImg = scale.Apply();
                return CreateResponse(outputImg);
            }
            catch (ArgumentException e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new InternalServerErrorResult();
            }
        }

        // Utility methods
        private static async Task<T> DeserializeMessageBody<T>(HttpRequest req)
        {
            using (var reader = new StreamReader(req.Body))
            {
                string content = await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentException("Request body was empty", nameof(req));
                }

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new Base64ImageJsonConverter());
                return JsonConvert.DeserializeObject<T>(content, settings);
            }
        }

        private static ObjectResult CreateResponse(Base64Image outputImg)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new Base64ImageJsonConverter());
            return new OkObjectResult(JsonConvert.SerializeObject(new ImageResponse(outputImg), settings));
        }
    }
}

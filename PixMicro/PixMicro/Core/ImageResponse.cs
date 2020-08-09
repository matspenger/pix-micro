using System;
using System.Collections.Generic;
using System.Text;

namespace PixMicro.Core
{
    /// <summary>
    /// Strongly typed representation of a response message.
    /// </summary>
    public class ImageResponse
    {
        /// <summary>
        /// Base64 encoded output image.
        /// </summary>
        public Base64Image OutputImage { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="outputImage">Output image to capture</param>
        public ImageResponse(Base64Image outputImage)
        {
            this.OutputImage = outputImage;
        }
    }
}

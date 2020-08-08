using System;
using System.Collections.Generic;
using System.Text;

namespace PixMicro.Core
{
    public class ImageResponse
    {
        public Base64Image OutputImage { get; }

        public ImageResponse(Base64Image outputImage)
        {
            this.OutputImage = outputImage;
        }
    }
}

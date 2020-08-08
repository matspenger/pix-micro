using OpenCvSharp;
using PixMicro.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PixMicro.Transforms
{
    public class Flip : IImageOperation
    {
        private readonly Mat outputMat = new Mat();

        public FlipMode Mode { get; set; }

        public Base64Image InputImage { get; set; }

        public Base64Image Apply()
        {
            var inputMat = Cv2.ImDecode(this.InputImage.Bytes, ImreadModes.Color);
            Cv2.Flip(inputMat, this.outputMat, this.Mode);
            Cv2.ImEncode(".jpg", this.outputMat, out byte[] output);
            return new Base64Image(output);
        }
    }
}

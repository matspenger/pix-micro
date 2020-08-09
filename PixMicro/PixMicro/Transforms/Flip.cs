using OpenCvSharp;
using PixMicro.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PixMicro.Transforms
{
    /// <summary>
    /// Flips an image, horizontally or vertically (or both).
    /// </summary>
    public class Flip : AbstractImageOperation
    {
        private readonly Mat outputMat = new Mat();
        /// <summary>
        /// Specifies flip mode: horizontal, vertical or both axes.
        /// </summary>
        public FlipMode Mode { get; set; }

        /// <inheritdoc />
        public override Base64Image Apply()
        {
            var inputMat = Cv2.ImDecode(this.InputImage.Bytes, ImreadModes.Color);
            Cv2.Flip(inputMat, this.outputMat, this.Mode);
            Cv2.ImEncode(this.OutputEncoding, this.outputMat, out byte[] output);
            return new Base64Image(output);
        }
    }
}

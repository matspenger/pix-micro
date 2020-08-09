using OpenCvSharp;
using PixMicro.Core;
using System;

namespace PixMicro.Transforms
{
    /// <summary>
    /// Scales an image, either by a factor or to a size
    /// specified by pixel dimensions.
    /// </summary>
    public class Scale : AbstractImageOperation
    {
        private readonly Mat outputMat = new Mat();

        /// <summary>
        /// Specifies scale mode: Uniform factor or pixel
        /// dimensions.
        /// </summary>
        public ScaleMode Mode { get; set; }

        /// <summary>
        /// Scale width in pixels. Only used if Mode = Pixels
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Scale height in pixels. Only used if Mode = Pixels
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Factor of resize. Only used if Mode = Factor
        /// </summary>
        public double? Factor { get; set; }

        /// <inheritdoc />
        public override Base64Image Apply()
        {
            var inputMat = Cv2.ImDecode(this.InputImage.Bytes, ImreadModes.Color);
            switch (this.Mode)
            {
                case ScaleMode.Pixels:
                    return this.ResizeToPixels(inputMat, this.Width, this.Height);
                case ScaleMode.Factor:
                    return this.ResizeByFactor(inputMat, this.Factor);
                default:
                    throw new UnknownEnumMemberException(this.Mode);
            }
        }

        private Base64Image ResizeToPixels(Mat inputMat, int? newWidth, int? newHeight)
        {
            if (!newWidth.HasValue || !newHeight.HasValue)
            {
                throw new ArgumentException("Width and height values must be provided");
            }

            // Compute factor
            double fx = (double)newWidth.Value / inputMat.Rows;
            double fy = (double)newHeight.Value / inputMat.Cols;

            Cv2.Resize(inputMat, this.outputMat, new Size(newWidth.Value, newHeight.Value), fx, fy);
            Cv2.ImEncode(this.OutputEncoding, this.outputMat, out byte[] output);
            return new Base64Image(output);
        }

        private Base64Image ResizeByFactor(Mat inputMat, double? factor)
        {
            if (!factor.HasValue)
            {
                throw new ArgumentException("Factor of scale must be provided");
            }
            // Compute new size
            double newWidth = inputMat.Rows * factor.Value;
            double newHeight = inputMat.Cols * factor.Value;
            Cv2.Resize(inputMat, this.outputMat, new Size(newWidth, newHeight), factor.Value, factor.Value);
            Cv2.ImEncode(this.OutputEncoding, this.outputMat, out byte[] output);
            return new Base64Image(output);
        }
    }

    /// <summary>
    /// Available modes for scaling.
    /// </summary>
    public enum ScaleMode
    {
        /// <summary>
        /// Scale to a specified pixel size (width, height)
        /// </summary>
        Pixels,
        /// <summary>
        /// Scale by a factor.
        /// </summary>
        Factor
    }
}

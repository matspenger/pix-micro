using OpenCvSharp;
using PixMicro.Core;
using System;

namespace PixMicro.Transforms
{
    /// <summary>
    /// Rotates an image.
    /// </summary>
    /// <remarks>
    /// Multiples of 90deg result in a straightforward rotation, resulting
    /// in a rectangular image. Arbitrary angles will rotate the image around
    /// its centre point and expand the canvas size sufficiently so that
    /// no cropping occurs.
    /// </remarks>
    public class Rotate : AbstractImageOperation
    {
        private readonly Mat outputMat = new Mat();

        /// <summary>
        /// Angle of rotation, in degrees.
        /// </summary>
        /// <remarks>
        /// +ve values: rotate counter-clockwise
        /// -ve values: rotate clockwise
        /// </remarks>
        public int RotateByDeg { get; set; }

        /// <inheritdoc />
        public override Base64Image InputImage { get; set; }

        /// <inheritdoc />
        public override Base64Image Apply()
        {
            var inputMat = Cv2.ImDecode(this.InputImage.Bytes, ImreadModes.Color);
            if (this.RotateByDeg % 90 == 0)
            {
                return this.RotateByMultipleOf90Deg(inputMat, this.RotateByDeg);
            }
            else
            {
                return this.RotateByArbitraryAngleAroundCentre(inputMat, this.RotateByDeg);
            }
        }

        private Base64Image RotateByMultipleOf90Deg(Mat inputMat, int rotateByDeg)
        {
            int rotateDegNormalised = rotateByDeg % 360; // Any angle greater than 360 is the same as it % 360
            RotateFlags flags;
            bool rotateAtAll = rotateDegNormalised > 0;
            Mat exportMat = inputMat;
            if (rotateAtAll)
            {
                switch (rotateDegNormalised)
                {
                    case 90:
                        flags = RotateFlags.Rotate90Counterclockwise;
                        break;
                    case 180:
                        flags = RotateFlags.Rotate180;
                        break;
                    case 270:
                        flags = RotateFlags.Rotate90Clockwise;
                        break;
                    default:
                        throw new ArgumentException($"Invalid rotation angle: {rotateByDeg}");
                }
                Cv2.Rotate(inputMat, this.outputMat, flags);
                exportMat = this.outputMat;
            }
            Cv2.ImEncode(this.OutputEncoding, exportMat, out byte[] output);
            return new Base64Image(output);
        }

        private Base64Image RotateByArbitraryAngleAroundCentre(Mat inputMat, int rotateByDeg)
        {
            int rotateDegNormalised = rotateByDeg % 360; // Any angle greater than 360 is the same as it % 360
            var centre = new Point2f(inputMat.Rows / 2, inputMat.Cols / 2);
            var rotMat = Cv2.GetRotationMatrix2D(centre, rotateDegNormalised, 1.0);

            // Ensure output is sized with rotation - otherwise edges would get cropped
            double cos = Math.Abs(rotMat.Get<double>(0, 0));
            double sin = Math.Abs(rotMat.Get<double>(0, 1));

            double newWidth = (int)((inputMat.Cols * sin) + (inputMat.Rows * cos));
            double newHeight = (int)((inputMat.Cols * cos) + (inputMat.Rows * sin));

            rotMat.Set(0, 2, rotMat.Get<double>(0, 2) + ((newWidth / 2) - (inputMat.Rows / 2)));
            rotMat.Set(1, 2, rotMat.Get<double>(1, 2) + ((newHeight / 2) - (inputMat.Cols / 2)));

            Cv2.WarpAffine(inputMat, this.outputMat, rotMat, new Size(newWidth, newHeight));

            Cv2.ImEncode(this.OutputEncoding, this.outputMat, out byte[] output);
            return new Base64Image(output);
        }
    }
}

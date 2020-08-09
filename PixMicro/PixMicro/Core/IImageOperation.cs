using System;
using System.Collections.Generic;
using System.Text;

namespace PixMicro.Core
{
    /// <summary>
    /// A common interface for all operations to carry out on images.
    /// All operations will have an input image, and an output image (as a result of "Apply")
    /// </summary>
    public interface IImageOperation
    {
        /// <summary>
        /// Image on which to apply the operation/transform
        /// </summary>
        Base64Image InputImage { get; set; }

        /// <summary>
        /// Apply this encoding to the output image.
        /// </summary>
        string OutputEncoding { get; }

        /// <summary>
        /// Carry out the operation/transform, returning a new image
        /// </summary>
        /// <returns>Base64 encoded image with transform/edit applied</returns>
        Base64Image Apply();
    }
}

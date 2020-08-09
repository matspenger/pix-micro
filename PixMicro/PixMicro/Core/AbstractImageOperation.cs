using System;
using System.Collections.Generic;
using System.Text;

namespace PixMicro.Core
{
    /// <summary>
    /// Abstract implementation of <see cref="IImageOperation"/> that applies
    /// a set of defaults.
    /// </summary>
    public abstract class AbstractImageOperation : IImageOperation
    {
        /// <inheritdoc />
        public abstract Base64Image InputImage { get; set; }

        /// <summary>
        /// Apply this encoding to the output image. Default value is ".jpg".
        /// </summary>
        public string OutputEncoding { get; set; } = ".jpg";

        /// <inheritdoc />
        public abstract Base64Image Apply();
    }
}

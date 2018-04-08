using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public interface IImageServiceModal
    {
        /// <summary>
        /// Add image to out_put_dir directory.
        /// </summary>
        /// <param name="args"> Args of the command including name and path of the image. </param>
        /// <param name="result"> Result of success or failure. </param>
        /// <returns> Return exception message if was throwed or success message. </returns>
        string AddFile(string[] args, out bool result);
    }
}

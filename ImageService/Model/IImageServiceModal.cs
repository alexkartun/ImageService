using ImageService.Logging.Model;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface IImageServiceModal
    {
        /// <summary>
        /// Add image to out_put_dir directory.
        /// </summary>
        /// <param name="args"> Args of the command including name and path of the image. </param>
        /// <param name="result"> Result of success or failure. </param>
        /// <returns> Return exception message if was throwed or success message. </returns>
        string AddFile(string[] args, out MessageTypeEnum result);
    }
}

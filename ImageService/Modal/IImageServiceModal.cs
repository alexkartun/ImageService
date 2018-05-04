using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Modal.Event;

namespace ImageService.Modal
{
    public interface IImageServiceModal
    {
        event EventHandler<LogEventArgs> LogRecieved;
        event EventHandler<DirectoryCloseEventArgs> CloseResieved;
        /// <summary>
        /// Add image to out_put_dir directory.
        /// </summary>
        /// <param name="args"> Args of the command including name and path of the image. </param>
        /// <param name="result"> Result of success or failure. </param>
        /// <returns> Return exception message if was throwed or success message. </returns>
        string AddFile(string[] args, out MessageTypeEnum result);
        string CloseDirectory(string[] args, out MessageTypeEnum result);
        string GetConfig(out MessageTypeEnum result, TcpClient client = null);
        string GetAllLog(out MessageTypeEnum result, TcpClient client = null);
        void SendLogsToClient(TcpClient client, EventLogEntryCollection entries);
    }
}

using ImageService.Logging.Model;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface ILogsServiceModal
    {
        List<string> ServiceLogs { get; set; }
		/// <summary>
		/// Gets all logs that has been received since last restart of ImageService.
		/// Reads all logs via TCP socket to a client (given via arg).
		/// </summary>
		/// <param name="result">  Sets to INFO unless an error occured. </param>
		/// <param name="client"> Client's socket. </param>
		/// <returns> Return exception message if was throwed or success message. </returns>
		string GetAllLog(out MessageTypeEnum result, TcpClient client);
    }
}

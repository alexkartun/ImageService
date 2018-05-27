using ImageService.Logging.Model;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ImageService.Model
{
    public interface ISettingsModal
    {
        List<string> Directory_Paths { get; set; }
		/// <summary>
		/// Collects settings and all directory handler's paths of ImageService.
		/// Sends all settings data via TCP socket to a given client (via arg).
		/// </summary>
		/// <param name="result">  Sets to INFO unless an error occured. </param>
		/// <param name="client"> Client's socket. </param>
		/// <returns> Return exception message if was throwed or success message. </returns>
		string GetSettings(out MessageTypeEnum result, TcpClient client);
    }
}

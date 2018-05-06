using ImageService.Logging.Modal;
using ImageService.Modal.Event;
using System;
using System.Net.Sockets;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        string Path { get; set; }
        /// <summary>
        /// Start handler specific directory.
        /// </summary>
		void StartHandleDirectory();
        void StopHandleDirectory();
        /// <summary>
        /// Handler asked to execute command by service/server.
        /// </summary>
        /// <param name="sender"> Server. </param>
        /// <param name="command_args"> Arguments for command to be executed. </param>
    }
}
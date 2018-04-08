using ImageService.Logging.Modal;
using ImageService.Modal.Event;
using System;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        // Event for closing the handler.
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        // Event for updating the event logger after every command execution.
		event EventHandler<MessageRecievedEventArgs> DirectoryAction;
        /// <summary>
        /// Start handler specific directory.
        /// </summary>
		void StartHandleDirectory();
        /// <summary>
        /// Handler asked to execute command by service/server.
        /// </summary>
        /// <param name="sender"> Server. </param>
        /// <param name="command_args"> Arguments for command to be executed. </param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs command_args);
    }
}
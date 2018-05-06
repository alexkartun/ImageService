using ImageService.Logging.Model;
using System.Net.Sockets;

namespace ImageService.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Command execution.
        /// </summary>
        /// <param name="args"> Arguments of the command. </param>
        /// <param name="result"> Result of the execution. </param>
        /// <returns> 
        /// Return string representation of execution, including exception data if throwed during the process.
        /// </returns>
        string Execute(string[] args, out MessageTypeEnum result, TcpClient client = null);
    }
}
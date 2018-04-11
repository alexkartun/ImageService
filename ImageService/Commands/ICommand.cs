using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

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
        string Execute(string[] args, out MessageTypeEnum result);
    }
}
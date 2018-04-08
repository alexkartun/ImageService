namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// Get the command from the map via commandID. Execute command.
        /// </summary>
        /// <param name="commandID"> Command ID to be executed. </param>
        /// <param name="args"> Arguments of the command. </param>
        /// <param name="result"> Result of the execution. </param>
        /// <returns> Return string representation of of succession or failure. </returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}


using ImageService.Infastructure;
using ImageService.Infastructure.Enums;

namespace ImageService.Controller
{
    public interface IImageController
    {
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}


using ImageService.Infastructure;
using ImageService.Infastructure.Enums;

namespace ImageService.Controller
{
    public interface IImageController
    {
        void ExecuteCommand(int commandID, string[] args);
    }
}


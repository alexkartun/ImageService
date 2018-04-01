using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;
        }

        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result); //TODO: args[0] switch to other object.
        }
    }
}

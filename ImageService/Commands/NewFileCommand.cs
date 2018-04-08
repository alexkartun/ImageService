using ImageService.Modal;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;
        }

        /// <summary>
        /// New file command execution. Add file called.
        /// </summary>
        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args, out result);
        }
    }
}

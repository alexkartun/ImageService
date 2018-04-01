using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public interface IImageServiceModal
    {
        string AddFile(string path, out bool result);
    }
}

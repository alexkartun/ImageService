using System;
using System.IO;
using System.Text;
using System.Drawing;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        private string m_OutputFolder;
        private string m_ThumbnailFolder;
        private int m_thumbnailSize;

        public ImageServiceModal(string output_folder, int thumbnail_size)
        {
            // Creates output file on construction.
            m_OutputFolder = Path.Combine(output_folder, "OutputDir");
            Directory.CreateDirectory(m_OutputFolder);
            // Creates Thumbnails subdir on construction.
            m_ThumbnailFolder = Path.Combine(m_OutputFolder, "Thumbnails");
            Directory.CreateDirectory(m_ThumbnailFolder);
            m_thumbnailSize = thumbnail_size;
        }

        // Creates date directories for picture and thumbnail,
        // Copies picture to output_dir,
        // Creates thumbnail out of picture and saves it inside thumbnail dir.
        public string AddFile(string[] args, out bool result)
        {
            if (args.Length != 2)
            {
				result = false;
				return "2 Args must be given to Addfile()";
            }
            string fullPath = args[0];
            string fileName = args[1];
            try
            {
                DateTime creation = File.GetCreationTime(fullPath);
                string picPathDir = CreateDateDirectory(m_OutputFolder, creation);
                string thumbnailPathDir = CreateDateDirectory(m_ThumbnailFolder, creation);
				string destFilePath = Path.Combine(picPathDir, fileName);

				while (File.Exists(destFilePath)) // Exists a picture with same name. Renaming current pic.
				{
					fileName = "cpy_" + fileName;
					destFilePath = Path.Combine(picPathDir, fileName);
				}
				File.Move(fullPath, destFilePath);
				CreateThumbnail(destFilePath, thumbnailPathDir, fileName);
            }
            catch (Exception e)
            {
				result = false;
				return "Error occured moving picture: " + fileName + ". Details: " + e.Data.ToString();
            }
			result = true;
			return "File added successfully. Picture name: " + fileName;
        }

        // Creates year and month directories in scrPath according to DateTime d.
        private static string CreateDateDirectory(string srcPath, DateTime d)
        {
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string datePathDir = Path.Combine(srcPath, year, month);
            Directory.CreateDirectory(datePathDir);
            return datePathDir;
        }

        // Creates thumbnail out of picture and saves it in thumbDest directory.
        // Using thumbnail size class member.
        private void CreateThumbnail(string picPath, string thumbDest, string name)
        {
            Image image = Image.FromFile(picPath);
            Image thumb = image.GetThumbnailImage(
                m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.Combine(thumbDest, name));
        }
    }
}


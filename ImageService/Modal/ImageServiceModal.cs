using System;
using System.IO;
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

        /// <summary>
        /// Creates date directories for picture and thumbnail,
        /// Copies picture to output_dir,
        /// Creates thumbnail out of picture and saves it inside thumbnail dir.
        /// </summary>
        public string AddFile(string[] args, out bool result)
        {
            // args length should be 2.
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
                // Exists a picture with same name. Renaming current pic.
                while (File.Exists(destFilePath))
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

        /// <summary>
        /// Creates year and month directories in scrPath according to DateTime d.
        /// </summary>
        /// <param name="srcPath"> Source path of the image. </param>
        /// <param name="d"> Date time of image creation. </param>
        /// <returns> Return the date path directory representation. </returns>
        private static string CreateDateDirectory(string srcPath, DateTime d)
        {
            string month = d.Month.ToString();
            string year = d.Year.ToString();
            string datePathDir = Path.Combine(srcPath, year, month);
            Directory.CreateDirectory(datePathDir);
            return datePathDir;
        }

        /// <summary>
        /// Creates thumbnail out of picture and saves it in thumbDest directory.
        /// Using thumbnail size class member.
        /// </summary>
        /// <param name="picPath"> Image path. </param>
        /// <param name="thumbDest"> Thumbnail path destination. </param>
        /// <param name="name"> Name of the thumbnail. </param>
        private void CreateThumbnail(string picPath, string thumbDest, string name)
        {
            Image image = Image.FromFile(picPath);
            Image thumb = image.GetThumbnailImage(
                m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(Path.Combine(thumbDest, name));
        }
    }
}


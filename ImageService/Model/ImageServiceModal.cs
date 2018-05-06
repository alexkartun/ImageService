using System;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Text;
using ImageService.Infastructure.Enums;
using ImageService.Logging.Model;

namespace ImageService.Model
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
			// Creates Thumbnails subdir on construction.
			m_ThumbnailFolder = Path.Combine(m_OutputFolder, "Thumbnails");
			m_thumbnailSize = thumbnail_size;
		}

		/// <summary>
		/// Creates date directories for picture and thumbnail,
		/// Copies picture to output_dir,
		/// Creates thumbnail out of picture and saves it inside thumbnail dir.
		/// </summary>
		public string AddFile(string[] args, out MessageTypeEnum result)
		{
			result = MessageTypeEnum.INFO;
			string fullPath = args[0];
			string fileName = args[1];
			try
			{
				if (!Directory.Exists(m_OutputFolder))
				{
					DirectoryInfo dir = Directory.CreateDirectory(m_OutputFolder);
					// Hide the directory.
					dir.Attributes = FileAttributes.Hidden;
					// Create thumbnail folder.
					Directory.CreateDirectory(m_ThumbnailFolder);
				}
				DateTime creation = GetDateTakenFromImage(fullPath, ref result);
				string picPathDir = CreateDateDirectory(m_OutputFolder, creation);
				string thumbnailPathDir = CreateDateDirectory(m_ThumbnailFolder, creation);
				string destFilePath = Path.Combine(picPathDir, fileName);
				// Exists a picture with same name. Renaming current pic.
				while (File.Exists(destFilePath))
				{
                    result = MessageTypeEnum.WARNING;
                    fileName = "cpy_" + fileName;
					destFilePath = Path.Combine(picPathDir, fileName);
				}
				File.Move(fullPath, destFilePath);
				CreateThumbnail(destFilePath, thumbnailPathDir, fileName);
			}
			catch (Exception)
			{
				result = MessageTypeEnum.FAIL;
			}
            return "Got command ID: " + ((int) CommandEnum.NewFileCommand).ToString() + " Args: " + args[0] + ", " + args[1];
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

		private static Regex r = new Regex(":");
		/// <summary>
		///Return DateTime object represents the time in which the photo was taken.
		///If that property is not set in this picture, function returns the date in which photo was created.
		/// </summary>
		/// <param name="path"> Image path. </param>
		/// <param name="warning_flag"> Indicates if taken date exists. </param>
		/// <returns> Return the day given photo was taken. </returns>
		public static DateTime GetDateTakenFromImage(string path, ref MessageTypeEnum status)
		{
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
				using (Image myImage = Image.FromStream(fs, false, false))
				{
					PropertyItem propItem = myImage.GetPropertyItem(36867);
					string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
					return DateTime.Parse(dateTaken);
				}
			}
			catch (Exception) {
                status = MessageTypeEnum.WARNING;
			}
			return File.GetCreationTime(path);
		}
    }
}


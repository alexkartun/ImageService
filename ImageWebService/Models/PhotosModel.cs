using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageWebService.Communication;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ImageWebService.Models
{
    public class PhotosModel
    {

        public List<Photo> ThumbnailPhotos { get; set; }
        /// <summary>
        /// Set all OutputDir photos.
        /// </summary>
        /// <param name="channel"> Web channel for requests. </param>
        public void SetServicePhotos(WebChannel channel)
        {
            ThumbnailPhotos = new List<Photo>();
            if (channel.IsConnected())
            {
                try
                {
                    // Get full physical path of outputdir.
                    string outputDirPath = HttpContext.Current.Server.MapPath(@"\OutputDir");
                    // Concat 'thumbnails'.
                    string thumbnailsPath = outputDirPath + @"\Thumbnails";
                    // Start crawling all the photos.
                    StartCrawlingAllPhotos(thumbnailsPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Start crawling all the photos in thumbnail directory.
        /// </summary>
        /// <param name="thumbnailsPath">Full thumbnail directory path.</param>
        private void StartCrawlingAllPhotos(string thumbnailsPath)
        {
            // Get all years directories.
            string[] yearPaths = Directory.GetDirectories(thumbnailsPath);
            // Continue on size greater than 0.
            if (yearPaths.Length > 0)
            {
                // Iterate over each year directory.
                foreach(string yearPath in yearPaths)
                {
                    // Get year name.
                    string yearName = Path.GetFileName(yearPath);
                    // Get all months directories.
                    string[] monthPaths = Directory.GetDirectories(yearPath);
                    // Continue on size greater than 0.
                    if (monthPaths.Length > 0)
                    {
                        // Iterate over each month directory.
                        foreach (string monthPath in monthPaths)
                        {
                            // Get month name.
                            string monthName = Path.GetFileName(monthPath);
                            // Get all thumbnail's path.
                            string[] imagePaths = Directory.GetFiles(monthPath);
                            // Continue on size greater than 0.
                            if (imagePaths.Length > 0)
                            {
                                // Iterate over each thumbnail file.
                                foreach (string imageFullPath in imagePaths)
                                {
                                    // Get thumbnail name.
                                    string imageName = Path.GetFileName(imageFullPath);
                                    // Get thumbnail relative path.
                                    string thumbnailRelativePath = 
                                      @"\OutputDir\Thumbnails" + @"\" + yearName + @"\" + monthName + @"\" + imageName;
                                    // Get image relative path.
                                    string imageRelativePath = 
                                      @"\OutputDir" + @"\" + yearName + @"\" + monthName + @"\" + imageName;
                                    // Get all image details.
                                    string details = GetImageDetails(imageRelativePath);
                                    ThumbnailPhotos.Add(
                                        new Photo(imageRelativePath, thumbnailRelativePath, 
                                                  imageName, yearName, monthName, details));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove image and thumbnail from pc.
        /// </summary>
        /// <param name="iPath"> Image path. </param>
        /// <param name="tPath"> Thumbnail path. </param>
        /// <param name="channel">Web channel.</param>
        public void RemoveImage(string iPath, string tPath, WebChannel channel)
        {
            if (channel.IsConnected())
            {
                // Get full path to image.
                string imagePath = HttpContext.Current.Server.MapPath(iPath);
                // Get full path to thumbnail.
                string thumbnailPath = HttpContext.Current.Server.MapPath(tPath);
                try
                {
                    File.Delete(imagePath);
                    File.Delete(thumbnailPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Get details of image via access to "comments" property.
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        /// <returns>Image Details.</returns>
        private string GetImageDetails(string imageRelativePath)
        {
            string details;
            try
            {
                string fullPath  = HttpContext.Current.Server.MapPath(imageRelativePath);
                using (Image myImage = Image.FromFile(fullPath))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(0x9286);
                    details = Encoding.UTF8.GetString(propItem.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                details = "";
            }
            return details;
        }
    }
}
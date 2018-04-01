﻿using System;
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
        public void AddFile(string[] args)
        {
            if (args.Length != 2)
            {
                throw new IOException("Error: Two args must be given.");
            }
            string fullPath = args[0];
            string fileName = args[1];
            try
            {
                DateTime creation = File.GetCreationTime(fullPath);
                string picPathDir = CreateDateDirectory(m_OutputFolder, creation);
                string thumbnailPathDir = CreateDateDirectory(m_ThumbnailFolder, creation);
                // TODO: check if copy/move.
                File.Copy(fullPath, Path.Combine(picPathDir, fileName), true);
                CreateThumbnail(fullPath, thumbnailPathDir, fileName);
            }
            catch (Exception)
            {
				// PLEASEEE ALEX PLEASE MY NAME IS SAPIR KIKOZ
                throw;
            }
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


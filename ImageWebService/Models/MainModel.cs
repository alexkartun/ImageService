using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageWebService.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ImageWebService.Models
{
    public class MainModel
    {
        /// <summary>
        /// Set all main window data.
        /// </summary>
        /// <param name="channel">Web channel for requests.</param>
        public void SetData(WebChannel channel)
        {
            SetServiceStatus(channel);
            SetStudents(channel);
            SetNumberOfPhotos(channel);
        }

        public string Active { get; set; }
        /// <summary>
        /// Check client-server connection.
        /// </summary>
        /// <param name="channel">Channel to check his cnnection.</param>
        private void SetServiceStatus(WebChannel channel)
        {
            if (channel.IsConnected())
            {
                Active = "Active";
            }
            else
            {
                Active = "Not Active";
            }
        }

        public List<Student> Students { get; set; }
        /// <summary>
        /// Set all students.
        /// </summary>
        /// <param name="channel">Web channel for requesting config data.</param>
        private void SetStudents(WebChannel channel)
        {
            Students = new List<Student>();
            if (channel.IsConnected())
            {
                // Request for config data.
                CommandMessage req = new CommandMessage((int)CommandEnum.GetConfigCommand);
                channel.Write(req);
                CommandMessage answer = channel.Read();

                // As 4 parameter in args is hard coded the student.txt path.
                String rootPath = answer.Args[4];
                string[] lines = File.ReadAllLines(rootPath);

                foreach (string line in lines)
                {
                    // Every line is deserialized by json.
                    Student student = JsonConvert.DeserializeObject<Student>(line);
                    Students.Add(student);
                }
            }
        }

        public string NumberOfPhotos { get; set; }
        /// <summary>
        /// Set number of photos in OutputDir.
        /// </summary>
        /// <param name="channel">Web channel for request.</param>
        private void SetNumberOfPhotos(WebChannel channel)
        {
            NumberOfPhotos = "";
            if (channel.IsConnected())
            {
                try
                {
                    // Get all physical path of thumbnails directory.
                    String rootPath = HttpContext.Current.Server.MapPath(@"\OutputDir\Thumbnails");
                    // Get number of thumbnails.
                    NumberOfPhotos = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories).Length.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
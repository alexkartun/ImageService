using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageWebService.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageWebService.Models
{
    public class MainModel
    {
        public MainModel()
        {
            SetServiceStatus();
            SetStudents();
            SetNumberOfPhotos();
        }

        public WebChannel ClientConnection
        {
            get
            {
                return WebChannel.Instance;
            }
        }

        public string Active { get; set; }
        public void SetServiceStatus()
        {
            if (ClientConnection.IsConnected())
            {
                Active = "Active";
            }
            else
            {
                Active = "Not Active";
            }
        }

        public List<Student> Students { get; set; }
        public void SetStudents()
        {
            Students = new List<Student>();
            if (ClientConnection.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.GetConfigCommand);
                ClientConnection.Write(req);
                CommandMessage answer = ClientConnection.Read();

                String rootPath = answer.Args[4];
                string[] lines = File.ReadAllLines(rootPath);

                foreach (string line in lines)
                {
                    Student student = JsonConvert.DeserializeObject<Student>(line);
                    Students.Add(student);
                }
            }
        }

        public string NumberOfPhotos { get; set; }
        public void SetNumberOfPhotos()
        {
            NumberOfPhotos = "";
            if (ClientConnection.IsConnected())
            {
                CommandMessage req = new CommandMessage((int)CommandEnum.GetConfigCommand);
                ClientConnection.Write(req);
                CommandMessage answer = ClientConnection.Read();

                String rootPath = answer.Args[0] + @"\OutputDir";
                NumberOfPhotos = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories).Length.ToString();
            }
        }
    }
}
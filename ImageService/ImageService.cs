using ImageService.Logging;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Controller;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        private ILoggingService image_logger;
        private ImageServer image_server;

        public ImageService()
        {
            InitializeComponent();
            string source_name = ConfigurationManager.AppSettings["SourceName"];
            string log_name = ConfigurationManager.AppSettings["LogName"];
            eventLog1.Source = source_name;
            eventLog1.Log = log_name;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("OnStart");
            string directories = ConfigurationManager.AppSettings["Handler"];
            string output_dir_path = ConfigurationManager.AppSettings["OutputDir"];
            string thumbnail_size = ConfigurationManager.AppSettings["ThumbnailSize"];
            image_logger = new LoggingService();
            image_logger.MessageRecieved += OnMsg;
            IImageServiceModal image_modal = new ImageServiceModal(output_dir_path, int.Parse(thumbnail_size));
            IImageController controller = new ImageController(image_modal);
            image_server = new ImageServer(controller, image_logger);
            image_server.CreateHandlers(directories);
        }

        private void OnMsg(object sender, MessageRecievedEventArgs message)
        {
            eventLog1.WriteEntry(message.Message);    //TODO: Check, about status.
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("OnStop");
            string directories = ConfigurationManager.AppSettings["Handler"];
            image_server.StopHandlers(directories);
        }
    }
}

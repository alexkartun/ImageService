using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Modal.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class ImageServer : IServer
    {
        private string ip;
        private string port;
        private TcpListener server;
        private List<TcpClient> clients;
        private ILoggingService logging_service;
        private IImageController image_controller;
        private List<IDirectoryHandler> directory_handlers;

        //public event EventHandler<CommandRecievedEventArgs> CommandRecieved;


        public ImageServer(string ip, string port, ILoggingService logger, IImageController controller)
        {
            this.ip = ip;
            this.port = port;
            clients = new List<TcpClient>();
            image_controller = controller;
            logging_service = logger;
            directory_handlers = new List<IDirectoryHandler>();
        }

        public void Start()
        {
            CreateHandlers();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            server = new TcpListener(ep);
            Task task = new Task(() =>
            {
                server.Start();
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        clients.Add(client);
                        HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }

                }
            });
            task.Start();
        }

        private void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.AutoFlush = true;
                    while (true)
                    {
                        string commandLine = reader.ReadLine();
                        CommandRecievedEventArgs cmd = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        string output = image_controller.ExecuteCommand(cmd.CommandID, cmd.Args,
                            out MessageTypeEnum status, client);
                        logging_service.Log(output, status);
                    }
                }
                //client.Close();
                //clients.Remove(client);
            }).Start();
        }

        public void Stop()
        {
            StopHandlers();
            server.Stop();
        }

        /// <summary>
        /// Update the event logger via logger with specific message.
        /// </summary>
        /// <param name="sender"> Handler that executed the command. </param>
        /// <param name="msg_args"> Message handler passing after execution. </param>
		private void OnMessageRecieved(object sender, MessageRecievedEventArgs msg_args)
        {
            logging_service.Log(msg_args.Message, msg_args.Status);       
        }

        /// <summary>
        /// Close sender handler. Remove from events.
        /// </summary>
        /// <param name="sender"> Handler to be closed. </param>
        /// <param name="close_args"> Closing args of specific handler. </param>
        private void OnCloseHandler(object sender, DirectoryCloseEventArgs close_args)
        {
            foreach (IDirectoryHandler dir in directory_handlers)
            {
                if (dir.Path.CompareTo(close_args.DirectoryPath) == 0)
                {
                    dir.StopHandleDirectory();
                    break;
                }
            }
            directory_handlers.RemoveAll(dir => dir.Path == close_args.DirectoryPath);
        }

        /// <summary>
        /// Stop The handlers.
        /// </summary>
        /// <param name="directories"> Directory paths we want to close handling. </param>
		private void StopHandlers()
        {
            foreach (IDirectoryHandler directory in directory_handlers)
            {
                directory.StopHandleDirectory();
            }
            //TODO: Send to clients that service stopped. Close all clients sockets.
        }

        /// <summary>
        /// Create and start handling the directories.
        /// </summary>
        private void CreateHandlers()
        {
            // Bind event for closing directory from image modal.
            image_controller.ImageModal.CloseResieved += OnCloseHandler;

            string directories = ConfigurationManager.AppSettings["Handler"];
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoryHandler(path, image_controller);
                directory_handlers.Add(handler);
                handler.MessageLogger += OnMessageRecieved;
                handler.StartHandleDirectory();
            }
        }
    }
}

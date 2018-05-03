using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Modal.Event;
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
        private TcpListener listener;
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
            listener = new TcpListener(ep);
            listener.Start();
            logging_service.Log("Waiting for client connections...", MessageTypeEnum.INFO);
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        logging_service.Log("Got new connection", MessageTypeEnum.INFO);
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
                    try
                    {
                        while (true)
                        {
                            string commandLine = reader.ReadLine();
                            SendCommand(commandLine, client);
                        }
                    }
                    catch (Exception)
                    {
                        logging_service.Log("Exception occured when writing/reading to/from socket.", MessageTypeEnum.FAIL);
                    }
                }
                client.Close();
                clients.Remove(client);
                logging_service.Log("Closing connection to client..", MessageTypeEnum.INFO);
            }).Start();
        }

        public void Stop()
        {
            logging_service.Log("Stopping for client connections...", MessageTypeEnum.INFO);
            StopHandlers();
            listener.Stop();
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
        /// Send command to handlers.
        /// </summary>
        /// <param name="command_args"> Command args for execution of the command. </param>
        private void SendCommand(string command_line, TcpClient client)
        {
            //CommandRecieved(this, command_args);
            string[] arr = command_line.Split(' ');
            string commandKey = arr[0];
            string[] args = arr.Skip(1).ToArray();
            string output = image_controller.ExecuteCommand(int.Parse(commandKey), args, out MessageTypeEnum status, client);
            logging_service.Log(output, status);
        }

        /// <summary>
        /// Close sender handler. Remove from events.
        /// </summary>
        /// <param name="sender"> Handler to be closed. </param>
        /// <param name="close_args"> Closing args of specific handler. </param>
        private void OnCloseHandler(object sender, DirectoryCloseEventArgs close_args)
        {
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            directory_handlers.Remove(handler);
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
            string directories = ConfigurationManager.AppSettings["Handler"];
            string[] paths = directories.Split(';');
            foreach (string path in paths)
            {
                IDirectoryHandler handler = new DirectoryHandler(path, image_controller);
                directory_handlers.Add(handler);
                handler.DirectoryClose += OnCloseHandler;
                handler.MessageLogger += OnMessageRecieved;
                handler.StartHandleDirectory();
            }
        }
    }
}

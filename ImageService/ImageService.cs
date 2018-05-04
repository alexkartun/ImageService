using ImageService.Logging;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;
using System.Runtime.InteropServices;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Controller;
using ImageService.Infastructure.Enums;
using ImageService.Modal.Event;

namespace ImageService
{
	public enum ServiceState
	{
		SERVICE_STOPPED = 0x00000001,
		SERVICE_START_PENDING = 0x00000002,
		SERVICE_STOP_PENDING = 0x00000003,
		SERVICE_RUNNING = 0x00000004,
		SERVICE_CONTINUE_PENDING = 0x00000005,
		SERVICE_PAUSE_PENDING = 0x00000006,
		SERVICE_PAUSED = 0x00000007,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ServiceStatus
	{
		public int dwServiceType;
		public ServiceState dwCurrentState;
		public int dwControlsAccepted;
		public int dwWin32ExitCode;
		public int dwServiceSpecificExitCode;
		public int dwCheckPoint;
		public int dwWaitHint;
	};
	public partial class ImageService : ServiceBase
    {
		private ILoggingService image_logger;
        private IServer image_server;

		public ImageService()
        {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
			string logName = ConfigurationManager.AppSettings["LogName"];
            string ip = ConfigurationManager.AppSettings["Ip"];
            string output_dir_path = ConfigurationManager.AppSettings["OutputDir"];
            string thumbnail_size = ConfigurationManager.AppSettings["ThumbnailSize"];
            string port = ConfigurationManager.AppSettings["Port"];
            eventLogger = new EventLog
            {
                Source = eventSourceName,
                Log = logName
            };
            image_logger = new LoggingService();
            image_logger.MessageRecieved += OnMsg;
            IImageServiceModal image_modal = new ImageServiceModal(output_dir_path, int.Parse(thumbnail_size));
            image_modal.LogRecieved += OnLogRecieved;
            IImageController controller = new ImageController(image_modal);
            image_server = new ImageServer(ip, port, image_logger, controller);
		}

        private void OnLogRecieved(object sender, LogEventArgs log_args)
        {
            IImageServiceModal modal = (IImageServiceModal) sender;
            modal.SendLogsToClient(log_args.CLIENT, eventLogger.Entries);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

		protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus
			{
				dwCurrentState = ServiceState.SERVICE_START_PENDING,
				dwWaitHint = 100000
			};
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            image_server.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);
		}

        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus
			{
				dwCurrentState = ServiceState.SERVICE_STOP_PENDING,
				dwWaitHint = 100000
			};
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            image_server.Stop();

			// Update the service state to Running.  
			serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);
		}

        /// <summary>
        /// Write entry to event log. Specific message will be written.
        /// </summary>
        /// <param name="sender"> Sender object requesting write entry. </param>
        /// <param name="message"> Message that will be written. </param>
        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {
            eventLogger.WriteEntry(message.Message, ConvertStatToEventLogEntry(message.Status));
        }

        /// <summary>
        /// Converts status enum of message to a built-in EventLogger entry type.
        /// </summary>
        /// <param name="msg"> Message args recieved. </param>
        /// <returns> Return event log entry type. </returns>
        private static EventLogEntryType ConvertStatToEventLogEntry(MessageTypeEnum status)
        {
            if (status == MessageTypeEnum.INFO) return EventLogEntryType.Information;
            else if (status == MessageTypeEnum.WARNING) return EventLogEntryType.Warning;
            else return EventLogEntryType.Error;
        }
    }
}

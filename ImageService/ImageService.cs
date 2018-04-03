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
using System.Runtime.InteropServices;
using ImageService.Logging.Modal;
using ImageService.Modal;
using ImageService.Controller;

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
        private ImageServer image_server;
		private int eventId = 1;

		public ImageService(string[] args)
        {
            InitializeComponent();
			eventLogger = new System.Diagnostics.EventLog();
			string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
			string logName = ConfigurationManager.AppSettings["LogName"];
			if (args.Count() > 0)
			{
				eventSourceName = args[0];
			}
			if (args.Count() > 1)
			{
				logName = args[1];
			}
			if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
			{
				System.Diagnostics.EventLog.CreateEventSource(
					eventSourceName, logName);
			}
			eventLogger.Source = eventSourceName;
			eventLogger.Log = logName;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

		protected override void OnStart(string[] args)
        {
			eventLogger.WriteEntry("pending start");
			// Update the service state to Start Pending.  
			ServiceStatus serviceStatus = new ServiceStatus
			{
				dwCurrentState = ServiceState.SERVICE_START_PENDING,
				dwWaitHint = 100000
			};
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            string directories = ConfigurationManager.AppSettings["Handler"];
            string output_dir_path = ConfigurationManager.AppSettings["OutputDir"];
            string thumbnail_size = ConfigurationManager.AppSettings["ThumbnailSize"];
            image_logger = new LoggingService();
            image_logger.MessageRecieved += OnMsg;
            IImageServiceModal image_modal = new ImageServiceModal(output_dir_path, int.Parse(thumbnail_size));
            IImageController controller = new ImageController(image_modal);
            image_server = new ImageServer(controller, image_logger);
			// TODO: Service constructs all except HANDLER.
            image_server.CreateHandlers(directories);

			// Update the service state to Running.  
			serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);
			eventLogger.WriteEntry("started");
		}


        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {
			eventLogger.WriteEntry("OnMsg test");
			eventLogger.WriteEntry(message.Message, ConvertStatToEventLogEntry(message), eventId++);
        }

		// Converts status enum of message to a built-in EventLogger entry type.
		private EventLogEntryType ConvertStatToEventLogEntry(MessageRecievedEventArgs msg)
		{
			switch ((int) msg.Status)
			{
				case 0:
					return EventLogEntryType.Information;
				case 1:
					return EventLogEntryType.Warning;
				case 2:
					return EventLogEntryType.Error;
			}
			throw new Exception("Not valid msg given");
		}


		protected override void OnStop()
        {
			eventLogger.WriteEntry("pending close");
			ServiceStatus serviceStatus = new ServiceStatus
			{
				dwCurrentState = ServiceState.SERVICE_STOP_PENDING,
				dwWaitHint = 100000
			};
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);

			// What happens here? stop handlers? not on server?
			//eventLogger.WriteEntry("OnStop");
            string directories = ConfigurationManager.AppSettings["Handler"];
            image_server.StopHandlers(directories);

			// Update the service state to Running.  
			serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);
			eventLogger.WriteEntry("pending closed");
		}
    }
}

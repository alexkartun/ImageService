using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ImageService.Server
{
	class Server
	{
		private IPEndPoint ep;
		private TcpListener listener;
		private List<TcpClient> clients;
		private Boolean running;
		private HandlersManager hm;

		public Server(string ip, int port, HandlersManager manager)
		{
			ep = new IPEndPoint(IPAddress.Parse(ip), port);
			listener = new TcpListener(ep);
			clients = new List<TcpClient>();
			hm = manager;
		}
		public void OnStart()
		{
			listener.Start();
			running = true;
			listenToClients();
		}

		public void listenToClients()
		{
			while (running)
			{
				Console.WriteLine("Waiting for client connections...");
				TcpClient client = listener.AcceptTcpClient();
				Console.WriteLine("Client connected");
				ThreadPool.QueueUserWorkItem(HandleClient, client);
			}
		}
		public void HandleClient(object state)
		{
			TcpClient client = (TcpClient) state;
			while (true)
			{
				Console.WriteLine("Client {0} is handled", client.ToString());
				using (NetworkStream stream = client.GetStream())
				using (BinaryReader reader = new BinaryReader(stream))
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					string input = reader.ReadString();
					string output = "";
					writer.Write(output);
				}
			}
		}
	}
}

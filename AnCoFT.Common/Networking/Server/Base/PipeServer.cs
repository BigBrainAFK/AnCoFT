using AnCoFT.Database.Networking.CommandHandler;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace AnCoFT.Database.Networking.Server.Base
{
	class PipeServer
	{
		private string PipeName;
		public PipeServer(string name)
		{
			this.PipeName = name;
		}

		void Start()
		{
			Console.WriteLine("Starting Server");

			var pipe = new NamedPipeServerStream(this.PipeName, PipeDirection.InOut);
			Console.WriteLine("Waiting for connection....");
			pipe.WaitForConnection();

			Console.WriteLine("Connected");

			Serializer.Deserialize(typeof(Command), pipe);
			pipe.Disconnect();
		}
	}
}

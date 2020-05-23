namespace AnCoFT.Database.Networking.CommandHandler
{
	class Command
	{
		short commandId;
		string arguments;

		public Command(short commandId, string arguments)
		{
			this.commandId = commandId;
			this.arguments = arguments;
		}
	}
}

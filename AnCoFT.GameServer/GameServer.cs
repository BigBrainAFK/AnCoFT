namespace AnCoFT.Networking.Server
{
    using System;
    using System.Threading;

    using AnCoFT.Database;
    using AnCoFT.Networking.Packet;
    using AnCoFT.Networking.Server.Base;

    public class GameServer : TcpServer
    {
        private readonly PacketHandlerBase _packetHandlerBase;

        public readonly GameHandler _gameHandler;

        private readonly string _name;

		private readonly DatabaseContext databaseContext;

        public GameServer(string ipAddress, int port, Configuration configuration, string name, GameHandler gameHandler = null) : base(ipAddress, port, configuration)
        {
			this.databaseContext = new DatabaseContext(configuration.dbConfig);

            if (gameHandler == null)
                this._gameHandler = new GameHandler(databaseContext);
            else
                this._gameHandler = gameHandler;

            this._packetHandlerBase = new PacketHandlerBase(this._gameHandler);
            this._name = name;
        }

        public override void ListenerThread()
        {
            this.Listener.Start();
            while (!this.Stopped)
            {
                try
                {
                    Client client = new Client(this.Listener.AcceptTcpClient(), this.ServerConfiguration);
                    this._gameHandler.AddClient(client);

                    Thread receivingThread = new Thread(this.ReceivingThread);
                    receivingThread.Start(client);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public override void ReceivingThread(object o)
        {
            Client client = (Client)o;
            PacketStream clientStream = client.PacketStream;
            byte[] clientBuffer = new byte[4096];

            this._packetHandlerBase.SendWelcomePacket(client);

            while ((!this.Stopped) && (clientStream.Read(clientBuffer, 0, 8) != 0))
            {
                if (BitConverter.ToInt16(clientBuffer, 6) > 0)
                {
                    clientStream.Read(clientBuffer, 8, BitConverter.ToInt16(clientBuffer, 6));
                }

                Packet packet = new Packet(clientBuffer);
                Console.WriteLine($"{this._name}-RECV [{packet.PacketId:X4}] {BitConverter.ToString(packet.GetRawPacket(), 0, packet.DataLength + 8)}");
                this._packetHandlerBase.HandlePacket(client, packet);
            }
        }
    }
}

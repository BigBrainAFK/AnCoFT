namespace AnCoFT.Networking
{
    using System.Collections.Generic;
    using System.Net.Sockets;

    using AnCoFT.Database;
    using AnCoFT.Database.Models;
    using AnCoFT.Game.SinglePlay.Challenge;
    using AnCoFT.Networking.Packet;
    using AnCoFT.Game.MatchPlay.Room;

    public class Client
    {
        public Client(TcpClient tcpClient, DatabaseContext databaseContextbCtx)
        {
            tcpClient.NoDelay = true;
            this.TcpClient = tcpClient;
            this.DatabaseContext = databaseContextbCtx;

            this.PacketStream = new PacketStream(tcpClient.GetStream(), new byte[4], new byte[4]);
        }

        public TcpClient TcpClient { get; set; }

        public PacketStream PacketStream { get; set; }

        public DatabaseContext DatabaseContext { get; set; }

        public Account Account { get; set; }

        public Character ActiveCharacter { get; set; }

        public ChallengeGame ActiveChallengeGame { get; set; }
        public Room ActiveRoom { get; set; }
        public bool InLobby { get; set; }
    }
}

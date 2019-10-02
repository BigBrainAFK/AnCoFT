namespace AnCoFT.Database.Models
{
    public enum GameServerType : byte
    {
        Chat = 0,
        Free = 1,
        Rookie = 2
    }

    public class GameServer
    {
        public short GameServerId { get; set; }

        public GameServerType ServerType { get; set; }

        public string Host { get; set; }

        public short Port { get; set; }
    }
}
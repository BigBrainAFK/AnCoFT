using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class S2CRoomInformationPacket : Packet
    {
        public S2CRoomInformationPacket(Game.MatchPlay.Room.Room room)
            : base(Networking.Packet.PacketId.S2CRoomInformation)
        {
            this.Write(room.Id);
            this.Write(room.Name);
            this.Write((byte)0); // Battlemon
            this.Write(room.GameMode);
            this.Write(room.Betting);
            this.Write(room.BettingMode);
            this.Write(room.BettingCoins);
            this.Write(room.BettingGold);
            this.Write(room.MaxPlayer);
            this.Write(room.Private);
            this.Write(room.Level);
            this.Write(room.LevelRange);
            this.Write((byte)0); // Unknown
            this.Write(room.Map);
            this.Write((byte)0); // Unknown
            this.Write((byte)0); // Unknown
            this.Write(room.Ball); // Unknown
        }
    }
}

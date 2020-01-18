using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Game.MatchPlay.Room;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class S2CRoomPlayerInformationPacket : Packet
    {
        public S2CRoomPlayerInformationPacket(List<RoomPlayer> roomPlayers)
            : base(Networking.Packet.PacketId.S2CRoomPlayerInformation)
        {
            this.Write((short)roomPlayers.Count);
            foreach (RoomPlayer rp in roomPlayers)
            {
                this.Write((short)rp.Position);
                this.Write(rp.Character.Name);
                this.Write((byte)0);
                this.Write((byte)0);
                this.Write(Convert.ToByte(rp.Master)); // Master
                this.Write(Convert.ToByte(rp.Ready)); // Ready
                this.Write((byte)0); // Fitting
                this.Write((byte)rp.Character.Type); // Type
                this.Write((byte)0);
                this.Write((byte)0);
                this.Write("");
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write((byte)0);

                this.Write("");

                this.Write(0);
                this.Write((byte)0);
                this.Write((short)0);
                this.Write((short)0);
                this.Write((short)0);
                this.Write((short)0);
                this.Write(0);

                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write((short)0);
            }
        }
    }
}

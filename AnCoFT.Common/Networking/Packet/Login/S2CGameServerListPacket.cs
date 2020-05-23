using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Login
{
    public class S2CGameServerListPacket : Packet
    {
        public S2CGameServerListPacket(List<Database.Models.GameServer> gameServerList)
            : base(Networking.Packet.PacketId.S2CGameServerList)
        {
            this.Write((short)gameServerList.Count);
            for (int x = 0; x < gameServerList.Count; x++)
            {
                this.Write(gameServerList[x].GameServerId);
                this.Write(gameServerList[x].ServerType);
                this.Write(gameServerList[x].Host);
                this.Write(gameServerList[x].Port);
                this.Write(0);
            }
        }
    }
}

using System.Collections.Generic;
using AnCoFT.Database.Models;
using AnCoFT.Networking.Packet.Home;

namespace AnCoFT.Networking.PacketHandler
{
    public static class HomePacketHandler
    {
        public static void HandleHomeDataRequest(Client client, Packet.Packet packet)
        {
            Database.Models.Home tempHome = new Database.Models.Home();
            tempHome.Inventory = new List<HomeInventory>();

            S2CHousingDataPacket housingDataPacket = new S2CHousingDataPacket(tempHome);
            client.PacketStream.Write(housingDataPacket);
        }

        public static void HandleHomeInventoryRequest(Client client, Packet.Packet packet)
        {
            Database.Models.Home tempHome = new Database.Models.Home();
            tempHome.Inventory = new List<HomeInventory>();

            S2CHousingInventoryDataPacket housingInventoryDataPacket = new S2CHousingInventoryDataPacket(tempHome);
            client.PacketStream.Write(housingInventoryDataPacket);
        }
    }
}

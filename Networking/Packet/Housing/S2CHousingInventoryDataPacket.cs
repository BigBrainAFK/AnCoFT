using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Home
{
    using AnCoFT.Database.Models;

    public class S2CHousingInventoryDataPacket : Packet
    {
        public S2CHousingInventoryDataPacket(Home home)
            : base(Networking.Packet.PacketId.S2CHomeItemsLoadAnswer)
        {
            if (home.Inventory.Count > 0)
            {
                this.Write((byte)home.Inventory.Count);
                foreach (HomeInventory homeInventory in home.Inventory)
                {
                    this.Write(homeInventory.ID);
                    this.Write(homeInventory.ItemID);
                    this.Write(homeInventory.Unk0);
                    this.Write(homeInventory.Unk1);
                    this.Write(homeInventory.XPos);
                    this.Write(homeInventory.YPos);
                }
            }
        }
    }
}

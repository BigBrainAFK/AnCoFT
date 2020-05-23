using AnCoFT.Networking.Packet.Shop;

namespace AnCoFT.Networking.PacketHandler
{
    public static class ShopPacketHandler
    {
        public static void HandleShopDataPrepare(Client client, Packet.Packet packet)
        {
            C2SShopDataPrepareRequestPacket shopDataPrepareRequestPacket = new C2SShopDataPrepareRequestPacket(packet);
            S2CShopDataPrepareAnswerPacket shopAnswerDataPreparePacket = new S2CShopDataPrepareAnswerPacket(
                shopDataPrepareRequestPacket.Category, shopDataPrepareRequestPacket.Part, shopDataPrepareRequestPacket.Character, 0);
            client.PacketStream.Write(shopAnswerDataPreparePacket);
        }
        public static void HandleShopData(Client client, Packet.Packet packet)
        {
            C2SShopDataRequestPacket shopDataRequestPacket = new C2SShopDataRequestPacket(packet);

            byte category = shopDataRequestPacket.Category;
            byte part = shopDataRequestPacket.Part;
            byte character = shopDataRequestPacket.Character;
            byte page = shopDataRequestPacket.Page;

            S2CShopDataAnswerPacket shopAnswerDataPacket = new S2CShopDataAnswerPacket();
            client.PacketStream.Write(shopAnswerDataPacket);
        }
    }
}

namespace AnCoFT.Networking.PacketHandler
{
    public static class QuestPacketHandler
    {
        public static void HandleEmblemListRequestPacket(Client client, Packet.Packet packet)
        {
            Packet.Packet testAnswer = new Packet.Packet(0x226B);

            short emblemQuestCount = 4;

            testAnswer.Write((short)0);
            testAnswer.Write(emblemQuestCount);
            for (int i = 0; i < emblemQuestCount; i++)
            {
                testAnswer.Write((short)i);
                testAnswer.Write((byte)1);
                testAnswer.Write((short)0);
            }
            testAnswer.Write((byte)1);
            testAnswer.Write((short)1);
            testAnswer.Write((byte)1);
            testAnswer.Write((short)1);
            testAnswer.Write((byte)1);
            testAnswer.Write((short)1);
            testAnswer.Write((byte)1);
            testAnswer.Write((short)1);

            client.PacketStream.Write(testAnswer);
        }

        public static void HandleQuestAcceptRequest(Client client, Packet.Packet packet)
        {
            
        }
    }
}

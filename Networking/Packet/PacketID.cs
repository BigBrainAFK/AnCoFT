namespace AnCoFT.Networking.Packet
{
    public static class PacketId
    {
        public const ushort S2CLoginWelcomePacket = 0xFF9A;
        public const ushort C2SLoginRequest = 0x0FA1;
        public const ushort S2CLoginAnswerPacket = 0x0FA2;
        public const ushort C2SHeartbeat = 0x0FA3;
        public const ushort C2SDisconnectRequest = 0x0FA7;
        public const ushort S2CDisconnectAnswer = 0xFA8;
        public const ushort C2SCharacterNameCheck = 0x1019;
        public const ushort S2CCharacterNameCheckAnswer = 0x101A;
        public const ushort C2SCharacterCreate = 0x101B;
        public const ushort S2CCharacterCreateAnswer = 0x101C;
        public const ushort C2SLoginFirstCharacterRequest = 0x101E;
        public const ushort S2CLoginFirstCharacterAnswer = 0x101F;
        public const ushort C2SLoginAliveClient = 0x100F;
        public const ushort S2CCharacterList = 0x1005;
        public const ushort S2CGameserverList = 0x1010;

        public const ushort C2SGameReceiveData = 0x105E;
        public const ushort S2CGameAnswerData = 0x105F;
        public const ushort C2SGameLoginData = 0x1069;
        public const ushort S2CGameLoginData = 0x106A;

        public const ushort C2SShopApReq = 0x1B60;

        public const ushort C2SCharacterDelete = 0x1B6B;
        public const ushort S2CCharacterDelete = 0x1B6C;

        public const ushort C2SChallengeProgressReq = 0x2206;
        public const ushort S2CChallengeProgressAck = 0x2207;

        public const ushort C2SChallengeBeginReq = 0x2208;

        public const ushort C2SChallengePoint = 0x220A;
        public const ushort C2SChallengeSet = 0x220B;
        public const ushort S2CChallengeEnd = 0x220C;

        public const ushort C2STutorialProgressReq = 0x220F;
        public const ushort S2CTutorialProgressAck = 0x2210;
    }
}
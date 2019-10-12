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
        public const ushort S2CGameServerList = 0x1010;

        public const ushort C2SGameReceiveData = 0x105E;
        public const ushort S2CGameAnswerData = 0x105F;
        public const ushort C2SGameLoginData = 0x1069;
        public const ushort S2CGameLoginData = 0x106A;

        public const ushort C2SRoomCreate = 0x1389;
        public const ushort S2CRoomCreateAnswer = 0x138A;
        public const ushort C2SRoomJoin = 0x138B;
        public const ushort S2CRoomJoinAnswer = 0x138C;

        public const ushort S2CRoomListAnswer = 0x138E;
        public const ushort S2CRoomPlayerInformation = 0x1394;
        public const ushort C2SRoomListReq = 0x13EC;

        public const ushort C2SLobbyUserListRequest = 0x1707;
        public const ushort S2CLobbyUserListAnswer = 0x1708;

        public const ushort S2CRoomInformation = 0x177A;

        public const ushort C2SRoomReadyChange = 0x1775;
        public const ushort C2SRoomStartGame = 0x177B;
        public const ushort C2SRoomPositionChange = 0x1785;
        public const ushort S2CRoomPositionChangeAnswer = 0x1786;
        public const ushort C2SRoomMapChange = 0x1788;
        public const ushort S2CRoomMapChangeAnswer = 0x1789;

        public const ushort S2CUnknownRoomJoin = 0x189D;

        public const ushort C2SShopApReq = 0x1B60;

        public const ushort C2SCharacterDelete = 0x1B6B;
        public const ushort S2CCharacterDelete = 0x1B6C;

        public const ushort C2SChallengeProgressReq = 0x2206;
        public const ushort S2CChallengeProgressAck = 0x2207;

        public const ushort C2SChallengeBeginReq = 0x2208;
        public const ushort C2SChallengeHp = 0x2209;

        public const ushort C2SChallengePoint = 0x220A;
        public const ushort C2SChallengeSet = 0x220B;
        public const ushort S2CChallengeEnd = 0x220C;

        public const ushort C2STutorialBegin = 0x220D;
        public const ushort C2STutorialEnd = 0x220E;
        public const ushort C2STutorialProgressReq = 0x220F;
        public const ushort S2CTutorialProgressAck = 0x2210;
        public const ushort C2SChallengeDamage = 0x2211;

        public const ushort C2SEmblemListRequest = 0x226A;
        public const ushort S2CEmblemListAnswer = 0x226B;

        public const ushort C2SLobbyJoin = 0x237A;
        public const ushort C2SLobbyLeave = 0x2379;
    }
}
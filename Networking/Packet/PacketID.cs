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

        public const ushort C2SHomeData = 0x1518;
        public const ushort S2CHomeData = 0x1519;

        public const ushort C2SWhisper = 0x1702;
        public const ushort S2CWhisperAnswer = 0x1703;
        public const ushort C2SLobbyChat = 0x1705;
        public const ushort S2CLobbyChatAnswer = 0x1706;
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
        
        public const ushort C2SCharacterMoneyRequest = 0x1B60;
        public const ushort S2CCharacterMoneyAnswer = 0x1B61;
        public const ushort C2SInventoryWearClothRequest = 0x1B63;
        public const ushort S2CInventoryWearClothAnswer = 0x1B64;
        public const ushort C2SShopBuyReq = 0x1B67;
        public const ushort S2CShopBuyAnswer = 0x1B68;
        public const ushort S2CInventoryData = 0x1B69;
        public const ushort C2SCharacterDelete = 0x1B6B;
        public const ushort S2CCharacterDelete = 0x1B6C;
        public const ushort C2SCharacterChangeStatsRequest = 0x1B6D;
        public const ushort S2CCharacterChangeStatsAnswer = 0x1B6E;
        public const ushort S2CCharacterStatsData = 0x1B6F;
        public const ushort C2SInventoryWearQuickRequest = 0x1BD8;
        public const ushort S2CInventoryWearQuickAnswer = 0x1BD9;

        public const ushort C2SInventorySellReq = 0x1D06;
        public const ushort S2CInventorySellAnswer = 0x1D07;
        public const ushort C2SInventorySellItemCheckReq = 0x1D08;
        public const ushort S2CInventorySellItemCheckAnswer = 0x1D09;
        public const ushort S2CInventorySellItemAnswer = 0x1D0A;

        // Messenger
        public const ushort C2SMessengerFriendAddRequest = 0x1F41;
        public const ushort S2CMessengerFriendAddAnswer = 0x1F42;
        public const ushort C2SMessengerFriendDataRequest = 0x1F49;
        public const ushort S2CMessengerFriendDataAnswer = 0x1F4A;
        public const ushort C2SMessengerFriendDeleteRequest = 0x1F55;
        public const ushort C2SMessengerMessageSendRequest = 0x1F5F;
        public const ushort S2CMessengerMessageSendAnswer = 0x1F60; // ?
        public const ushort C2SMessengerMessageDeleteRequest = 0x1F62;
        public const ushort C2SMessengerMessageDataRequest = 0x1F63;
        public const ushort S2CMessengerMessageDataAnswer = 0x1F64;
        public const ushort C2SMessengerMessageReadRequest = 0x1F67;
        public const ushort S2CMessengerMessageReadAnswer = 0x1F68;
        public const ushort C2SMessengerParcelDataRequest = 0x219C;
        public const ushort S2CMessengerParcelDataAnswer = 0x219D;
        public const ushort C2SMessengerProposalDataRequest = 0x2526;
        public const ushort S2CMessengerProposalDataAnswer = 0x2527;

        // Guild
        public const ushort C2SGuildNameCheckRequest = 0x2009;
        public const ushort S2CGuildNameCheckAnswer = 0x200A;
        public const ushort C2SGuildCreateRequest = 0x200B;
        public const ushort S2CGuildCreateAnswer = 0x200C;
        public const ushort C2SGuildDataRequest = 0x200D;
        public const ushort S2CGuildDataAnswer = 0x200E;
        public const ushort C2SGuildListRequest = 0x200F;
        public const ushort S2CGuildListAnswer = 0x2010;
        public const ushort C2SGuildJoinRequest = 0x2011;
        public const ushort S2CGuildJoinAnswer = 0x2012;
        public const ushort C2SGuildLeaveRequest = 0x2014;
        public const ushort S2CGuildLeaveAnswer = 0x2015; // ?
        public const ushort C2SGuildChangeInformationRequest = 0x2017;
        public const ushort C2SGuildReserveMemberDataRequest = 0x2018;
        public const ushort S2CGuildReserveMemberDataAnswer = 0x2019;
        public const ushort C2SGuildMemberDataRequest = 0x201A;
        public const ushort S2CGuildMemberDataAnswer = 0x201B;
        public const ushort C2SGuildChangeMasterRequest = 0x201F;
        public const ushort S2CGuildChangeMasterAnswer = 0x2020;
        public const ushort C2SGuildChangeSubMasterRequest = 0x2021;
        public const ushort S2CGuildChangeSubMasterAnswer = 0x2022;
        public const ushort C2SGuildDismissMemberRequest = 0x2023;
        public const ushort S2CGuildDismissMemberAnswer = 0x2024;
        public const ushort S2CGuildDismissInfo = 0x2025; // ?
        public const ushort C2SGuildDeleteRequest = 0x2026;
        public const ushort S2CGuildDeleteAnswer = 0x2027;
        public const ushort C2SGuildGoldWithdrawalRequest = 0x2029; // ?
        public const ushort S2CGuildGoldWithdrawalAnswer = 0x202A; // ?
        public const ushort C2SGuildGoldDataRequest = 0x202C;
        public const ushort S2CGuildGoldDataAnswer = 0x202D;
        public const ushort C2SGuildChangeNoticeRequest = 0x202E;
        public const ushort S2CGuildChangeNoticeAnswer = 0x202F;
        public const ushort C2SGuildChatRequest = 0x2030;
        public const ushort S2CGuildChatAnswer = 0x2031;
        public const ushort C2SGuildSearchRequest = 0x203A;
        public const ushort S2CGuildSearchAnswer = 0x203B;
        public const ushort C2SGuildChangeReverseMemberRequest = 0x203F;
        public const ushort S2CGuildChangeReverseMemberAnswer = 0x2040;
        public const ushort S2CGuildCastleChangeInfoAnswer = 0x2047;

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

        public const ushort C2SQuestListRequest = 0x226A;
        public const ushort S2CQuestListAnswer = 0x226B;
        public const ushort C2SQuestAcceptRequest = 0x2274;

        public const ushort S2CCharacterLevelExpData = 0x22B8;

        public const ushort C2SLobbyJoin = 0x237A;
        public const ushort C2SLobbyLeave = 0x2379;

        public const ushort C2SShopRequestDataPrepare = 0x2389;
        public const ushort S2CShopAnswerDataPrepare = 0x238A;
        public const ushort C2SShopRequestData = 0x2387;
        public const ushort S2CShopAnswerData = 0x2388;

        public const ushort C2SHomeItemsLoadReq = 0x254E;
        public const ushort S2CHomeItemsLoadAnswer = 0x254F;
        public const ushort C2SHomeItemsClearReq = 0x2552;
        public const ushort C2SHomeItemsPlaceReq = 0x2550;

        public const ushort C2SLeagueRankingRequest = 0x26F2;
        public const ushort S2CGuildLeagueRankingAnswer = 0x26F3;
    }
}
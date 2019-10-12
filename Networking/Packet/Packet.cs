using AnCoFT.Game.MatchPlay.Room;

namespace AnCoFT.Networking.Packet
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AnCoFT.Database.Models;
    using AnCoFT.Game.Item;

    public class Packet
    {
        private int _readPosition = 0;

        public Packet(Packet packet)
        {
            this.CheckSerial = packet.CheckSerial;
            this.CheckSum = packet.CheckSum;
            this.PacketId = packet.PacketId;
            this.DataLength = packet.DataLength;

            this.Data = new byte[this.DataLength];
            Buffer.BlockCopy(packet.Data, 0, this.Data, 0, this.DataLength);
        }

        public Packet(byte[] rawData)
        {
            this.CheckSerial = BitConverter.ToUInt16(rawData, 0);
            this.CheckSum = BitConverter.ToUInt16(rawData, 2);
            this.PacketId = BitConverter.ToUInt16(rawData, 4);
            this.DataLength = BitConverter.ToUInt16(rawData, 6);

            this.Data = new byte[this.DataLength];
            Buffer.BlockCopy(rawData, 8, this.Data, 0, this.DataLength);
        }

        public Packet(ushort packetId)
        {
            this.PacketId = packetId;
            this.DataLength = 0;
            this.Data = new byte[4096];
        }

        public ushort CheckSerial { get; set; }

        public ushort CheckSum { get; set; }

        public ushort PacketId { get; set; }

        public ushort DataLength { get; set; }

        public byte[] Data { get; set; }

        public static int IndexOf(byte[] array, byte[] pattern, int offset)
        {
            int success = 0;
            for (int i = offset; i < array.Length; i++)
            {
                if (array[i] == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    return i - pattern.Length + 1;
                }
            }
            return -1;
        }

        public void Write(params object[] dataList)
        {
            foreach (object o in dataList)
            {
                this.Write(o);
            }
        }

        public byte[] AddByteToArray(byte[] byteArray, byte newByte)
        {
            byte[] newArray = new byte[byteArray.Length + 1];
            byteArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }

        public void Write(object element)
        {
            byte[] dataElement;
            switch (Type.GetTypeCode(element.GetType()))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    dataElement = BitConverter.GetBytes(Convert.ToInt16(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 2);
                    this.DataLength += 2;
                    break;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    dataElement = BitConverter.GetBytes(Convert.ToInt32(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 4);
                    this.DataLength += 4;
                    break;

                case TypeCode.String:
                    dataElement = Encoding.Unicode.GetBytes(Convert.ToString(element));
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, dataElement.Length);
                    this.DataLength += Convert.ToUInt16(dataElement.Length);
                    break;

                case TypeCode.Byte:
                    dataElement = BitConverter.GetBytes((byte)element);
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 1);
                    this.DataLength += 1;
                    break;

                case TypeCode.Boolean:
                    dataElement = BitConverter.GetBytes((bool)element);
                    Buffer.BlockCopy(dataElement, 0, this.Data, this.DataLength, 1);
                    this.DataLength += 1;
                    break;

                default:
                    break;
            }
        }

        public int ReadInteger()
        {
            int element = BitConverter.ToInt32(this.Data, this._readPosition);
            this._readPosition += 4;
            return element;
        }

        public byte ReadByte()
        {
            byte result = this.Data[this._readPosition];
            this._readPosition += 1;
            return result;
        }

        public void ReadByte(out byte element)
        {
            element = this.Data[this._readPosition];
            this._readPosition += 1;
        }

        public short ReadShort()
        {
            short result = BitConverter.ToInt16(this.Data, this._readPosition);
            this._readPosition += 2;
            return result;
        }

        public string ReadUnicodeString()
        {
            string result = string.Empty;
            int stringLength = IndexOf(this.Data, new byte[] { 0x00, 0x00 }, this._readPosition) + 1 - this._readPosition;
            if (stringLength > 0)
            {
                result = Encoding.Unicode.GetString(this.Data, this._readPosition, stringLength);
                this._readPosition += stringLength + 2;
            }

            return result;
        }

        public string ReadString()
        {
            string result = string.Empty;
            int stringLength = IndexOf(this.Data, new byte[] { 0x00 }, this._readPosition) - this._readPosition;
            if (stringLength > 0)
            {
                result = Encoding.ASCII.GetString(this.Data, this._readPosition, stringLength);
                this._readPosition += stringLength + 1;
            }

            return result;
        }

        public byte[] GetRawPacket()
        {
            byte[] p = new byte[8 + this.DataLength];

            byte[] serial = BitConverter.GetBytes(this.CheckSerial);
            byte[] check = BitConverter.GetBytes(this.CheckSum);
            byte[] id = BitConverter.GetBytes(this.PacketId);
            byte[] dataLength = BitConverter.GetBytes(this.DataLength);

            Buffer.BlockCopy(serial, 0, p, 0, 2);
            Buffer.BlockCopy(check, 0, p, 2, 2);
            Buffer.BlockCopy(id, 0, p, 4, 2);
            Buffer.BlockCopy(dataLength, 0, p, 6, 2);
            Buffer.BlockCopy(this.Data, 0, p, 8, this.DataLength);

            return p;
        }
    }

    public class S2CWelcomePacket : Packet
    {
        public S2CWelcomePacket(int decKey, int encKey, int decTblIdx, int encTblIdx)
            : base(Networking.Packet.PacketId.S2CLoginWelcomePacket)
        {
            this.Write(decKey);
            this.Write(encKey);
            this.Write(decTblIdx);
            this.Write(encTblIdx);
        }
    }

    public class C2SLoginPacket : Packet
    {
        public C2SLoginPacket(Packet packet)
            : base(packet)
        {
            this.Username = this.ReadUnicodeString();
            this.Password = this.ReadString();
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    public enum LoginResult : short
    {
        Success = 0,
        PasswordIncorrect = -1,
        AlreadyLoggedIn = -2,
        AccountExpired = -3,
        AccountInvalid = -4
    }

    public class S2CLoginAnswerPacket : Packet
    {
        public S2CLoginAnswerPacket(LoginResult result)
            : base(Networking.Packet.PacketId.S2CLoginAnswerPacket)
        {
            this.Write(result);
        }
    }

    public class S2CDisconnectAnswerPacket : Packet
    {
        public S2CDisconnectAnswerPacket()
            : base(Networking.Packet.PacketId.S2CDisconnectAnswer)
        {
            this.Write((byte)0);
        }
    }

    public class S2CCharacterListPacket : Packet
    {
        public S2CCharacterListPacket(int accountId, List<Character> characterList)
            : base(Networking.Packet.PacketId.S2CCharacterList)
        {
            this.Write(0);
            this.Write(0);
            this.Write(accountId);
            this.Write((byte)0);
            this.Write((byte)0);

            if (characterList != null)
            {
                this.Write((byte) characterList.Count);

                for (int i = 0; i < characterList.Count; i++)
                {
                    this.Write(characterList[i].CharacterId);
                    this.Write(characterList[i].Name);
                    this.Write((short) 0);
                    this.Write(characterList[i].Level);
                    this.Write(characterList[i].AlreadyCreated);
                    this.Write((byte) 0);
                    this.Write(characterList[i].Gold);
                    this.Write(characterList[i].Type);
                    this.Write(characterList[i].Strength);
                    this.Write(characterList[i].Stamina);
                    this.Write(characterList[i].Dexterity);
                    this.Write(characterList[i].Willpower);
                    this.Write(characterList[i].StatusPoints);
                    this.Write(characterList[i].NameChangeAllowed);
                    this.Write((byte) (characterList[i].NameChangeAllowed ? 1 : 0));
                    this.Write(characterList[i].Hair);
                    this.Write(characterList[i].Face);
                    this.Write(characterList[i].Dress);
                    this.Write(characterList[i].Pants);
                    this.Write(characterList[i].Socks);
                    this.Write(characterList[i].Shoes);
                    this.Write(characterList[i].Gloves);
                    this.Write(characterList[i].Racket);
                    this.Write(characterList[i].Glasses);
                    this.Write(characterList[i].Bag);
                    this.Write(characterList[i].Hat);
                    this.Write(characterList[i].Dye);
                }
            }
        }
    }

    public class C2SFirstCharacterPacket : Packet
    {
        public C2SFirstCharacterPacket(Packet packet)
            : base(packet)
        {
            this.CharacterType = this.ReadByte();
        }

        public byte CharacterType { get; set; }
    }

    public class S2CFirstCharacterAnswerPacket : Packet
    {
        public S2CFirstCharacterAnswerPacket(short result, int characterId = 0, byte characterType = 0)
            : base(Networking.Packet.PacketId.S2CLoginFirstCharacterAnswer)
        {
            this.Write(result);
            this.Write(characterId);
            this.Write(characterType);
        }
    }

    public class C2SCharacterNameCheckPacket : Packet
    {
        public C2SCharacterNameCheckPacket(Packet packet)
            : base(packet)
        {
            this.Nickname = this.ReadUnicodeString();
        }

        public string Nickname { get; }
    }

    public class C2SCharacterCreatePacket : Packet
    {
        public C2SCharacterCreatePacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
            this.Nickname = this.ReadUnicodeString();
            this.Strength = this.ReadByte();
            this.Stamina = this.ReadByte();
            this.Dexterity = this.ReadByte();
            this.Willpower = this.ReadByte();
            this.StatusPoints = this.ReadByte();
            this.CharacterType = this.ReadByte();
        }

        public int CharacterId { get; }
        public string Nickname { get; }
        public byte Strength { get; }
        public byte Stamina { get; }
        public byte Dexterity { get; }
        public byte Willpower { get; }
        public byte StatusPoints { get; }
        public byte CharacterType { get; }
    }

    public class S2CCharacterCreateAnswerPacket : Packet
    {
        public S2CCharacterCreateAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CCharacterCreateAnswer)
        {
            this.Write(result);
        }
    }

    public class S2CCharacterNameCheckAnswerPacket : Packet
    {
        public S2CCharacterNameCheckAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CCharacterNameCheckAnswer)
        {
            this.Write(result);
        }
    }

    public class C2SCharacterDeletePacket : Packet
    {
        public C2SCharacterDeletePacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
        }

        public int CharacterId { get; set; }
    }

    public class S2CCharacterDeleteAnswerPacket : Packet
    {
        public S2CCharacterDeleteAnswerPacket(short result)

            : base(Networking.Packet.PacketId.S2CCharacterDelete)
        {
            this.Write(result);
        }
    }

    public class S2CGameServerListPacket : Packet
    {
        public S2CGameServerListPacket(List<GameServer> gameServerList)
            : base(Networking.Packet.PacketId.S2CGameServerList)
        {
            this.Write((short)gameServerList.Count);
            for (int x = 0; x < gameServerList.Count; x++)
            {
                this.Write(gameServerList[x].GameServerId);
                this.Write(gameServerList[x].ServerType);
                this.Write(gameServerList[x].Host);
                this.Write((short)0);
                this.Write(gameServerList[x].Port);
                this.Write(0);
            }
        }
    }

    public class C2SGameServerRequestPacket : Packet
    {
        public C2SGameServerRequestPacket(Packet packet)
            : base(packet)
        {
            this.RequestType = this.ReadByte();
        }

        public byte RequestType { get; set; }
    }

    public class S2CGameServerAnswerPacket : Packet
    {
        public S2CGameServerAnswerPacket(byte requestType)
            : base(Networking.Packet.PacketId.S2CGameAnswerData)
        {
            this.Write(requestType);
            this.Write((byte)0);
        }
    }

    public class C2SGameServerLoginPacket : Packet
    {
        public C2SGameServerLoginPacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
        }

        public int CharacterId { get; set; }
    }

    public class S2CChallengeProgressAnswerPacket : Packet
    {
        public S2CChallengeProgressAnswerPacket(List<ChallengeProgress> challengeProgressList)
            : base(Networking.Packet.PacketId.S2CChallengeProgressAck)
        {
            this.Write((ushort)challengeProgressList.Count);
            for (int i = 0; i < challengeProgressList.Count; i++)
            {
                this.Write(challengeProgressList[i].ChallengeId);
                this.Write(challengeProgressList[i].Success);
                this.Write(challengeProgressList[i].Attempts);
            }
        }
    }

    public class S2CTutorialProgressAnswerPacket : Packet
    {
        public S2CTutorialProgressAnswerPacket(List<TutorialProgress> tutorialProgress)
            : base(Networking.Packet.PacketId.S2CTutorialProgressAck)
        {
            this.Write((ushort)tutorialProgress.Count);
            for (int i = 0; i < tutorialProgress.Count; i++)
            {
                this.Write((ushort)tutorialProgress[i].TutorialId);
                this.Write(Convert.ToUInt16(tutorialProgress[i].Success));
                this.Write(Convert.ToUInt16(tutorialProgress[i].Attempts));
            }
        }
    }

    public class C2SChallengeBeginRequestPacket : Packet
    {
        public C2SChallengeBeginRequestPacket(Packet packet)
            : base(packet)
        {
            this.ChallengeId = packet.ReadShort();
        }

        public short ChallengeId { get; set; }
    }

    public class C2SChallengeHpPacket : Packet
    {
        public C2SChallengeHpPacket(Packet packet)
            : base(packet)
        {
            this.NpcHp = this.ReadShort();
            this.PlayerHp = this.ReadShort();
        }
        
        public short NpcHp { get; set; }
        public short PlayerHp { get; set; }
    }

    public class C2SChallengePointPacket : Packet
    {
        public C2SChallengePointPacket(Packet packet)
            : base(packet)
        {
            this.PointsPlayer = packet.ReadByte();
            this.PointsNpc = packet.ReadByte();
        }

        public byte PointsPlayer { get; set; }

        public byte PointsNpc { get; set; }
    }

    public class C2SChallengeDamagePacket : Packet
    {
        public C2SChallengeDamagePacket(Packet packet)
            : base(packet)
        {
            this.Player = packet.ReadByte();
            this.Hp = packet.ReadInteger();
        }

        public byte Player { get; set; }

        public int Hp { get; set; }
    }

    public class S2CChallengeFinishPacket : Packet
    {
        public S2CChallengeFinishPacket(bool win, byte newLevel, int exp, int gold, int secondsNeeded, List<ItemReward> itemReward)
            : base(Networking.Packet.PacketId.S2CChallengeEnd)
        {
            this.Write(Convert.ToByte(win));  
            this.Write(newLevel);   
            this.Write(exp);  
            this.Write(gold);  
            this.Write(secondsNeeded);  
            this.Write(Convert.ToInt16(itemReward.Count));    

            // To Do:
            foreach (ItemReward reward in itemReward)
            {
            }
        }
    }

    public class C2STutorialEndPacket : Packet
    {
        public C2STutorialEndPacket(Packet packet)
            : base(packet)
        {
            this.TutorialId = this.ReadByte();
        }

        public byte TutorialId { get; set; }
    }

    public class C2SRoomCreatePacket : Packet
    {
        public C2SRoomCreatePacket(Packet packet)
            : base(packet)
        {
            this.Name = this.ReadUnicodeString();
            this.Type = this.ReadByte();
            this.GameMode = this.ReadByte();
            this.Rule = this.ReadByte();
            this.Players = this.ReadByte();
            this.Private = Convert.ToBoolean(this.ReadByte());
            this.Unknown = this.ReadByte();
            this.SkillFree = Convert.ToBoolean(this.ReadByte());
            this.QuickSlot = Convert.ToBoolean(this.ReadByte());
            this.LevelRange = this.ReadByte();
            this.BettingType = this.ReadShort();
            this.BettingAmount = this.ReadInteger();
            this.Ball = this.ReadInteger();

            if (this.Private)
                this.Password = this.ReadUnicodeString();
        }

        public string Name { get; set; }
        public byte Type { get; set; }
        public byte GameMode { get; set; }
        public byte Rule { get; set; }
        public byte Players { get; set; }
        public bool Private { get; set; }
        public byte Unknown { get; set; }
        public bool SkillFree { get; set; }
        public bool QuickSlot { get; set; }
        public byte LevelRange { get; set; }
        public short BettingType { get; set; }
        public int BettingAmount { get; set; }
        public int Ball { get; set; }
        public string Password { get; set; }
    }

    public class C2SRoomJoinPacket : Packet
    {
        public C2SRoomJoinPacket(Packet packet)
            : base(packet)
        {
            this.RoomId = this.ReadShort();
        }

        public short RoomId { get; set; }
    }

    public class S2CRoomJoinAnswer : Packet
    {
        public S2CRoomJoinAnswer(short result, byte roomType, byte unknown, byte unknown2)
            : base(Networking.Packet.PacketId.S2CRoomJoinAnswer)
        {
            this.Write(result);
            this.Write(roomType);
            this.Write(unknown);
            this.Write(unknown2);
        }
    }

    public class S2CRoomInformation : Packet
    {
        public S2CRoomInformation(Room room)
            : base(Networking.Packet.PacketId.S2CRoomInformation)
        {
            this.Write(room.Id);
            this.Write(room.Name);
            this.Write((short) 0);
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

    public class C2SRoomReadyChangePacket : Packet
    {
        public C2SRoomReadyChangePacket(Packet packet)
            : base(packet)
        {
            this.Ready = this.ReadByte();
        }

        public byte Ready { get; set; }
    }

    public class C2SRoomPositionChange : Packet
    {
        public C2SRoomPositionChange(Packet packet)
            : base(packet)
        {
            this.NewPosition = this.ReadShort();
        }

        public short NewPosition { get; set; }
    }

    public class S2CRoomPositionChangeAnswer : Packet
    {
        public S2CRoomPositionChangeAnswer(short result, short oldPosition, short newPosition)
            : base(Networking.Packet.PacketId.S2CRoomPositionChangeAnswer)
        {
            this.Write(result);
            this.Write(oldPosition);
            this.Write(newPosition);
        }
    }
    public class S2CRoomPlayerInformation : Packet
    {
        public S2CRoomPlayerInformation(List<RoomPlayer> roomPlayers)
            : base(Networking.Packet.PacketId.S2CRoomPlayerInformation)
        {
            this.Write((short)roomPlayers.Count);
            foreach (RoomPlayer rp in roomPlayers)
            {
                this.Write((short) rp.Position);
                this.Write(rp.Character.Name);
                this.Write((short) 0);
                this.Write((byte) 0);
                this.Write((byte) 0);
                this.Write(Convert.ToByte(rp.Master)); // Master
                this.Write(Convert.ToByte(rp.Ready)); // Ready
                this.Write((byte) 0); // Fitting
                this.Write((byte) rp.Character.Type); // Type
                this.Write((byte) 0);
                this.Write((byte) 0);

                this.Write("");
                this.Write((short) 0);

                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write(0);
                this.Write((byte) 0);

                this.Write("");
                this.Write((short)0);

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

    public class C2SRoomMapChangePacket : Packet
    {
        public C2SRoomMapChangePacket(Packet packet)
            : base(packet)
        {
            this.Map = this.ReadByte();
        }

        public byte Map { get; set; }
    }

    public class S2CRoomMapChangeAnswerPacket : Packet
    {
        public S2CRoomMapChangeAnswerPacket(byte map)
            : base(Networking.Packet.PacketId.S2CRoomMapChangeAnswer)
        {
            this.Write(map);
        }
    }

    public class S2CRoomListAnswerPacket : Packet
    {
        public S2CRoomListAnswerPacket(List<Room> roomList)
            : base(Networking.Packet.PacketId.S2CRoomListAnswer)
        {
            this.Write((short)roomList.Count);

            for (int i = 0; i < roomList.Count; i++)
            {
                this.Write((short)i);
                this.Write(roomList[i].Name);
                this.Write((short)0);
                this.Write(roomList[i].GameMode);
                this.Write(roomList[i].BattleMode);
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
                this.Write(0); // Unknown
                this.Write((int)roomList[i].Ball);
                this.Write(roomList[i].MaxPlayer);
                this.Write(roomList[i].Private);
                this.Write(roomList[i].Level);
                this.Write(roomList[i].LevelRange);
                this.Write((byte) 0); // Unknown
                this.Write(roomList[i].Map);
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
                this.Write((byte)roomList[i].CurrentPlayers.Count); // Unknown
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
            }
        }
    }

    public class C2SLobbyUserListRequestPacket : Packet
    {
        public C2SLobbyUserListRequestPacket(Packet packet)
            : base(packet)
        {
            this.Page = this.ReadByte();
        }

        public byte Page { get; set; }
    }

    public class S2CLobbyUserListAnswerPacket : Packet
    {
        public S2CLobbyUserListAnswerPacket(List<Character> characterList)
            : base(Networking.Packet.PacketId.S2CLobbyUserListAnswer)
        {
            this.Write((byte)characterList.Count);
            for (int i = 0; i < characterList.Count; i++)
            {
                this.Write((short)i);
                this.Write(characterList[i].Name);
                this.Write((short)0);
                this.Write(characterList[i].CharacterId);
                this.Write(characterList[i].Type);
            }
        }
    }
}
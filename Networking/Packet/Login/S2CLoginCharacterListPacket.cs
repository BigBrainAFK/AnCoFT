using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Login
{
    public class S2CLoginCharacterListPacket : Packet
    {
        public S2CLoginCharacterListPacket(int accountId, List<Database.Models.Character> characterList)
            : base(Networking.Packet.PacketId.S2CCharacterList)
        {
            this.Write(0);
            this.Write(0);
            this.Write(accountId);
            this.Write((byte)0);
            this.Write((byte)0);

            if (characterList != null)
            {
                this.Write((byte)characterList.Count);

                for (int i = 0; i < characterList.Count; i++)
                {
                    this.Write(characterList[i].CharacterId);
                    this.Write(characterList[i].Name);
                    this.Write(characterList[i].Level);
                    this.Write(characterList[i].AlreadyCreated);
                    this.Write((byte)0);
                    this.Write(characterList[i].Gold);
                    this.Write(characterList[i].Type);
                    this.Write(characterList[i].Strength);
                    this.Write(characterList[i].Stamina);
                    this.Write(characterList[i].Dexterity);
                    this.Write(characterList[i].Willpower);
                    this.Write(characterList[i].StatusPoints);
                    this.Write(characterList[i].NameChangeAllowed);
                    this.Write((byte)(characterList[i].NameChangeAllowed ? 1 : 0));
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
}

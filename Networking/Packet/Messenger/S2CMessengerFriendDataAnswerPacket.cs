using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class S2CMessengerFriendDataAnswerPacket : Packet
    {
        public S2CMessengerFriendDataAnswerPacket(List<MessengerFriend> friendList)
            : base(Networking.Packet.PacketId.S2CMessengerFriendDataAnswer)
        {
            this.Write((byte)friendList.Count);
            foreach (MessengerFriend friend in friendList)
            {
                this.Write(friend.FriendCharacter.CharacterId);
                this.Write(friend.FriendCharacter.Name);
                this.Write(friend.FriendCharacter.Type);
                this.Write((short) -1); // ServerID - To Do
            }
        }
    }
}

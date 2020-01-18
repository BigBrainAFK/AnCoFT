using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AnCoFT.Database.Models;
using AnCoFT.Game.Messenger;
using AnCoFT.Networking.Packet.Messenger;

namespace AnCoFT.Networking.PacketHandler
{
    public static class MessengerPacketHandler
    {
        public static void HandleMessengerFriendDataRequest(Client client, Packet.Packet packet)
        {
            List<MessengerFriend> friendList =
                client.ActiveCharacter.MessengerFriendList ?? new List<MessengerFriend>();

            S2CMessengerFriendDataAnswerPacket friendDataAnswerPacket = new S2CMessengerFriendDataAnswerPacket(friendList);
            client.PacketStream.Write(friendDataAnswerPacket);
        }

        public static void HandleMessengerFriendAddRequest(Client client, Packet.Packet packet)
        {
            C2SMessengerFriendAddRequestPacket messengerFriendAddRequest = new C2SMessengerFriendAddRequestPacket(packet);

            int friendCharacterId =
                client.DatabaseContext.Character.First(c => c.Name == messengerFriendAddRequest.FriendCharacterName).CharacterId;

            if (friendCharacterId > 0)
            {
                client.DatabaseContext.MessengerFriendList.Add(new MessengerFriend()
                    {CharacterId = client.ActiveCharacter.CharacterId, FriendCharacterId = friendCharacterId });
                client.DatabaseContext.SaveChanges();
            }
        }

        public static void HandleMessengerFriendDeleteRequest(Client client, Packet.Packet packet)
        {
            C2SMessengerFriendDeleteRequestPacket messengerFriendDeleteRequest = new C2SMessengerFriendDeleteRequestPacket(packet);

            MessengerFriend messengerFriend = client.DatabaseContext.MessengerFriendList
                .First(mf =>
                    mf.CharacterId == client.ActiveCharacter.CharacterId &&
                    mf.FriendCharacterId == messengerFriendDeleteRequest.FriendCharacterId);

            if (messengerFriend != null)
            {
                client.DatabaseContext.MessengerFriendList.Remove(messengerFriend);
                client.DatabaseContext.SaveChanges();

                List<MessengerFriend> friendList =
                    client.ActiveCharacter.MessengerFriendList ?? new List<MessengerFriend>();

                S2CMessengerFriendDataAnswerPacket friendDataAnswerPacket = new S2CMessengerFriendDataAnswerPacket(friendList);
                client.PacketStream.Write(friendDataAnswerPacket);
            }
        }

        public static void HandleMessengerProposalDataRequest(Client client, Packet.Packet packet)
        {
            List<MessengerProposal> proposals = client.DatabaseContext.MessengerProposal?
                .Where(mp => mp.To.CharacterId == client.ActiveCharacter.CharacterId).ToList() ?? new List<MessengerProposal>();

            S2CMessengerProposalDataAnswerPacket proposalDataAnswerPacket = new S2CMessengerProposalDataAnswerPacket(proposals);
            client.PacketStream.Write(proposalDataAnswerPacket);
        }

        public static void HandleMessengerParcelDataRequest(Client client, Packet.Packet packet)
        {
            List<MessengerParcel> parcelList = client.DatabaseContext.MessengerParcel?
                .Where(mp => mp.To.CharacterId == client.ActiveCharacter.CharacterId)?.ToList() ?? new List<MessengerParcel>();

            S2CMessengerParcelDataAnswerPacket messengerParcelDataAnswer = new S2CMessengerParcelDataAnswerPacket(parcelList);
            client.PacketStream.Write(messengerParcelDataAnswer);
        }

        public static void HandleMessengerMessageDataRequest(Client client, Packet.Packet packet)
        {
            List<MessengerMessage> messages = client.DatabaseContext.MessengerMessage?
                .Where(mm => mm.To.CharacterId == client.ActiveCharacter.CharacterId).ToList();

            if (messages?.Count > 0)
            {
                Dictionary<MessageType, List<MessengerMessage>> messageList = messages
                    .GroupBy(mm => mm.Type)
                    .ToDictionary(grp => grp.Key, grp => grp.ToList());

                foreach (KeyValuePair<MessageType, List<MessengerMessage>> messageGroup in messageList)
                {
                    S2CMessengerMessageDataAnswerPacket messageDataPacket = new S2CMessengerMessageDataAnswerPacket(messageGroup.Key, messageGroup.Value);
                    client.PacketStream.Write(messageDataPacket);
                }
            }
            else
            {
                S2CMessengerMessageDataAnswerPacket messageDataPacket = new S2CMessengerMessageDataAnswerPacket(0, new List<MessengerMessage>());
                client.PacketStream.Write(messageDataPacket);
            }
        }

        public static void HandleMessengerMessageDeleteRequest(Client client, Packet.Packet packet)
        {
            C2SMessengerMessageDeleteRequestPacket messageDeleteRequestPacket = new C2SMessengerMessageDeleteRequestPacket(packet);
            client.DatabaseContext.MessengerMessage.Remove(client.DatabaseContext.MessengerMessage.Find(messageDeleteRequestPacket.MessageId));
            client.DatabaseContext.SaveChanges();
        }

        public static void HandleMessengerMessageReadRequest(Client client, Packet.Packet packet)
        {
            C2SMessengerMessageReadRequestPacket messageReadRequestPacket = new C2SMessengerMessageReadRequestPacket(packet);
            client.DatabaseContext.MessengerMessage.Find(messageReadRequestPacket.MessageId).Read = true;
            client.DatabaseContext.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnCoFT.Database.Models;
using AnCoFT.Game.Messenger;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class S2CMessengerMessageDataAnswerPacket : Packet
    {
        public S2CMessengerMessageDataAnswerPacket(MessageType messageType, List<MessengerMessage> messageList)
            : base(Networking.Packet.PacketId.S2CMessengerMessageDataAnswer)
        {
            List<MessengerMessage> messageReceivedList = messageList.Where(mm => mm.Type == messageType).ToList();
            this.Write((byte)messageType);
            this.Write((byte)messageReceivedList.Count);

            foreach (MessengerMessage messageReceived in messageReceivedList)
            {
                this.Write(messageReceived.Id); // MessageId
                this.Write(messageReceived.From.Name);
                this.Write(messageReceived.Read); // Read
                this.Write(messageReceived.Message);
                this.Write(0); // Unknown
                this.Write(63135360); // SendDate Unknown Format
                this.Write(0); // Unknown
                this.Write((byte)0);
            }
        }
    }
}

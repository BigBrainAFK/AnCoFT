using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class S2CMessengerParcelDataAnswerPacket : Packet
    {
        public S2CMessengerParcelDataAnswerPacket(List<MessengerParcel> parcelList)
            : base(Networking.Packet.PacketId.S2CMessengerParcelDataAnswer)
        {
            this.Write((byte)0);
            this.Write((byte)parcelList.Count);
            foreach (MessengerParcel parcel in parcelList)
            {
                this.Write(parcel.Id); // MessageId
                this.Write(parcel.From.Name);
                this.Write(parcel.Message);
                this.Write(parcel.Read); // Read
                this.Write(0); // Unknown
                this.Write((byte) 0); // Unknown
                this.Write(0); // Unknown
                this.Write((byte) 0); // Unknown
                this.Write(0); // Unknown
                this.Write((byte) 0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown

                /*
                    Packet.Packet testAnswer = new Packet.Packet(0x219D);
                    testAnswer.Write((byte)0);
                    testAnswer.Write((byte)1);
                    testAnswer.Write(20); // MessageId
                    testAnswer.Write("Name20");
                    testAnswer.Write("Message");
                    testAnswer.Write((byte)0); // Read

                    testAnswer.Write(13); // Unknown
                    testAnswer.Write((byte)1); // Read

                    testAnswer.Write(13); // SendDate Unknown Format

                    testAnswer.Write((byte)2); // 0 = nothing, 1 = Gold, 2 = Cash on delivery
                    testAnswer.Write(3); // Amount

                    testAnswer.Write((byte)2); // Cash on Delivery
                    testAnswer.Write(13); // SendDate Unknown Format

                    testAnswer.Write(444444); // SendDate Unknown Format
                 */
            }
        }
    }
}

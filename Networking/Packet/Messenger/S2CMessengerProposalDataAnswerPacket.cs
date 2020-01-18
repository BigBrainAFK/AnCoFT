using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class S2CMessengerProposalDataAnswerPacket : Packet
    {
        public S2CMessengerProposalDataAnswerPacket(List<MessengerProposal> proposals)
            : base(Networking.Packet.PacketId.S2CMessengerProposalDataAnswer)
        {
            this.Write((byte)0);
            this.Write((byte)proposals.Count);

            foreach (MessengerProposal proposal in proposals)
            {
                this.Write(proposal.Id); // MessageId
                this.Write(proposal.From.Name);
                this.Write(proposal.Read); // Read
                this.Write(proposal.Message);
                this.Write(0); // Unknown
                this.Write(0); // SendDate Unknown Format
                this.Write(0); // Unknown
            }
        }
    }
}

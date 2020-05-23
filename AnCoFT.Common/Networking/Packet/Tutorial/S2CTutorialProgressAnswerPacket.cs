using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Tutorial
{
    public class S2CTutorialProgressAnswerPacket : Packet
    {
        public S2CTutorialProgressAnswerPacket(List<TutorialProgress> tutorialProgressList)
            : base(Networking.Packet.PacketId.S2CTutorialProgressAck)
        {
            this.Write((ushort)tutorialProgressList.Count);
            foreach (TutorialProgress tutorialProgress in tutorialProgressList)
            {
                this.Write((ushort)tutorialProgress.TutorialId);
                this.Write(Convert.ToUInt16(tutorialProgress.Success));
                this.Write(Convert.ToUInt16(tutorialProgress.Attempts));
            }
        }
    }
}

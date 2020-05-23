using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Challenge
{
    public class S2CChallengeProgressAnswerPacket : Packet
    {
        public S2CChallengeProgressAnswerPacket(List<ChallengeProgress> challengeProgressList)
            : base(Networking.Packet.PacketId.S2CChallengeProgressAck)
        {
            this.Write((ushort)challengeProgressList.Count);
            foreach (ChallengeProgress challengeProgress in challengeProgressList)
            {
                this.Write(challengeProgress.ChallengeId);
                this.Write(challengeProgress.Success);
                this.Write(challengeProgress.Attempts);
            }
        }
    }
}

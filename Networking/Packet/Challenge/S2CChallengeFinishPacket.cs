using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Game.Item;

namespace AnCoFT.Networking.Packet.Challenge
{
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
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Game.Messenger
{
    public enum MessageType : byte
    {
        MessageReceived = 0,
        MessageSent = 1,
        GiftReceived = 2,
        GiftSent = 3
    }
}

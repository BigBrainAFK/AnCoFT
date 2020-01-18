using System;
using System.Collections.Generic;
using System.Linq;
using AnCoFT.Database.Models;
using AnCoFT.Game.Item;
using AnCoFT.Networking.Packet.Challenge;
using AnCoFT.Networking.Packet.Tutorial;

namespace AnCoFT.Networking.PacketHandler
{
    public static class TutorialPacketHandler
    {
        public static void HandleTutorialProgressRequestPacket(Client client, Packet.Packet packet)
        {
            List<TutorialProgress> tutorialProgress = client.DatabaseContext.TutorialProgress
                .Where(t => t.CharacterId == client.Account.Characters[0].CharacterId).ToList();

            S2CTutorialProgressAnswerPacket tutorialProgressAnswerPacket = new S2CTutorialProgressAnswerPacket(tutorialProgress);
            client.PacketStream.Write(tutorialProgressAnswerPacket);
        }

        public static void HandleTutorialBeginPacket(Client client, Packet.Packet packet)
        {
        }

        public static void HandleTutorialEndPacket(Client client, Packet.Packet packet)
        {
            C2STutorialFinishRequestPacket tutorialEndPacket = new C2STutorialFinishRequestPacket(packet);
            S2CChallengeFinishPacket challengeFinishPacket = new S2CChallengeFinishPacket(true, 1, 1, 2, 10, new List<ItemReward>());
            client.PacketStream.Write(challengeFinishPacket);

            TutorialProgress tutorialProgress = new TutorialProgress();
            tutorialProgress.CharacterId = client.ActiveCharacter.CharacterId;
            tutorialProgress.TutorialId = tutorialEndPacket.TutorialId;
            tutorialProgress.Success += Convert.ToInt16(1);
            tutorialProgress.Attempts += 1;

            var dbTutorialProgressContext = client.DatabaseContext.TutorialProgress.Find(new object[]
                {tutorialProgress.CharacterId, tutorialProgress.TutorialId});

            if (dbTutorialProgressContext == null)
                client.DatabaseContext.TutorialProgress.Add(tutorialProgress);
            else
            {
                dbTutorialProgressContext.Success = tutorialProgress.Success;
                dbTutorialProgressContext.Attempts = tutorialProgress.Attempts;
                client.DatabaseContext.TutorialProgress.Update(dbTutorialProgressContext);
            }

            client.DatabaseContext.SaveChanges();

            List<TutorialProgress> tutorialProgressList = client.DatabaseContext.TutorialProgress
                .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

            client.ActiveCharacter.TutorialProgress = tutorialProgressList;
            S2CTutorialProgressAnswerPacket tutorialProgressAnswerPacket = new S2CTutorialProgressAnswerPacket(tutorialProgressList);
            client.PacketStream.Write(tutorialProgressAnswerPacket);
        }
    }
}

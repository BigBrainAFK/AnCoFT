using System;
using System.Collections.Generic;
using System.Linq;
using AnCoFT.Database.Models;
using AnCoFT.Game.Item;
using AnCoFT.Game.SinglePlay.Challenge;
using AnCoFT.Networking.Packet.Challenge;

namespace AnCoFT.Networking.PacketHandler
{
    public static class ChallengePacketHandler
    {
        public static void HandleChallengeProgressRequestPacket(Client client, Packet.Packet packet)
        {
            List<ChallengeProgress> challengeProgressList = client.DatabaseContext.ChallengeProgress?
                .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

            S2CChallengeProgressAnswerPacket challengeProgressAnswerPacket = new S2CChallengeProgressAnswerPacket(challengeProgressList ?? new List<ChallengeProgress>());
            client.PacketStream.Write(challengeProgressAnswerPacket);
        }

        public static void HandleChallengeBeginRequestPacket(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SChallengeBeginRequestPacket challengeBeginRequestPacket = new C2SChallengeBeginRequestPacket(packet);

            Challenge currentChallenge = gameHandler.GetChallenge(challengeBeginRequestPacket.ChallengeId);

            if (currentChallenge.GameMode == GameMode.Basic)
                client.ActiveChallengeGame = new ChallengeBasicGame(challengeBeginRequestPacket.ChallengeId);
            else if (currentChallenge.GameMode == GameMode.Battle)
                client.ActiveChallengeGame = new ChallengeBattleGame(challengeBeginRequestPacket.ChallengeId);

            Packet.Packet answer = new Packet.Packet(0x220D);
            answer.Write((short)1);
            client.PacketStream.Write(answer);
        }

        public static void HandleChallengeHpPacket(Client client, Packet.Packet packet)
        {
            C2SChallengeHpPacket challengeHpPacket = new C2SChallengeHpPacket(packet);
            if (client.ActiveChallengeGame.GetType() == typeof(ChallengeBattleGame))
            {
                ((ChallengeBattleGame)client.ActiveChallengeGame).NpcHp = challengeHpPacket.NpcHp;
                ((ChallengeBattleGame)client.ActiveChallengeGame).PlayerHp = challengeHpPacket.PlayerHp;
            }
        }

        public static void HandleChallengePointPacket(Client client, Packet.Packet packet)
        {
            C2SChallengePointPacket challengePointPacket = new C2SChallengePointPacket(packet);
            ((ChallengeBasicGame)client.ActiveChallengeGame).SetPoints(challengePointPacket.PointsPlayer, challengePointPacket.PointsNpc);

            if (((ChallengeBasicGame)client.ActiveChallengeGame).Finished)
            {
                bool win = ((ChallengeBasicGame)client.ActiveChallengeGame).SetsPlayer == 2;
                int timeNeeded = (((ChallengeBasicGame)client.ActiveChallengeGame).EndTime
                                  - ((ChallengeBasicGame)client.ActiveChallengeGame).StartTime).Seconds;

                // To Do
                S2CChallengeFinishPacket challengeFinishPacket = new S2CChallengeFinishPacket(win, 1, 1, 2, timeNeeded, new List<ItemReward>());
                client.PacketStream.Write(challengeFinishPacket);

                ChallengeProgress challengeProgress = new ChallengeProgress();
                challengeProgress.CharacterId = client.ActiveCharacter.CharacterId;
                challengeProgress.ChallengeId = client.ActiveChallengeGame.ChallengeId;
                challengeProgress.Success += Convert.ToInt16(win);
                challengeProgress.Attempts += 1;

                var dbChallengeProgressContext = client.DatabaseContext.ChallengeProgress.Find(new object[]
                    {challengeProgress.CharacterId, challengeProgress.ChallengeId});

                if (dbChallengeProgressContext == null)
                    client.DatabaseContext.ChallengeProgress.Add(challengeProgress);
                else
                {
                    dbChallengeProgressContext.Success = challengeProgress.Success;
                    dbChallengeProgressContext.Attempts = challengeProgress.Attempts;
                    client.DatabaseContext.ChallengeProgress.Update(dbChallengeProgressContext);
                }

                client.DatabaseContext.SaveChanges();
                client.ActiveChallengeGame = null;

                List<ChallengeProgress> challengeProgressList = client.DatabaseContext.ChallengeProgress?
                    .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

                client.ActiveCharacter.ChallengeProgress = challengeProgressList;
                S2CChallengeProgressAnswerPacket challengeProgressAnswerPacket = new S2CChallengeProgressAnswerPacket(challengeProgressList ?? new List<ChallengeProgress>());
                client.PacketStream.Write(challengeProgressAnswerPacket);
            }
        }

        public static void HandleChallengeSetPacket(Client client, Packet.Packet packet)
        {
        }

        public static void HandleChallengeDamagePacket(Client client, Packet.Packet packet)
        {
            C2SChallengeDamagePacket challengeDamagePacket = new C2SChallengeDamagePacket(packet);
            ((ChallengeBattleGame)client.ActiveChallengeGame).SetHp(challengeDamagePacket.Player,
                challengeDamagePacket.Hp);

            if (((ChallengeBattleGame)client.ActiveChallengeGame).Finished)
            {
                bool win = ((ChallengeBattleGame)client.ActiveChallengeGame).PlayerHp > 0;
                int timeNeeded = (((ChallengeBattleGame)client.ActiveChallengeGame).EndTime
                                  - ((ChallengeBattleGame)client.ActiveChallengeGame).StartTime).Seconds;

                S2CChallengeFinishPacket challengeFinishPacket = new S2CChallengeFinishPacket(win, 1, 1, 2, timeNeeded, new List<ItemReward>());
                client.PacketStream.Write(challengeFinishPacket);

                ChallengeProgress challengeProgress = new ChallengeProgress();
                challengeProgress.CharacterId = client.ActiveCharacter.CharacterId;
                challengeProgress.ChallengeId = client.ActiveChallengeGame.ChallengeId;
                challengeProgress.Success += Convert.ToInt16(win);
                challengeProgress.Attempts += 1;

                var dbChallengeProgressContext = client.DatabaseContext.ChallengeProgress.Find(new object[]
                    {challengeProgress.CharacterId, challengeProgress.ChallengeId});

                if (dbChallengeProgressContext == null)
                    client.DatabaseContext.ChallengeProgress.Add(challengeProgress);
                else
                {
                    dbChallengeProgressContext.Success = challengeProgress.Success;
                    dbChallengeProgressContext.Attempts = challengeProgress.Attempts;
                    client.DatabaseContext.ChallengeProgress.Update(dbChallengeProgressContext);
                }

                client.DatabaseContext.SaveChanges();
                client.ActiveChallengeGame = null;

                List<ChallengeProgress> challengeProgressList = client.DatabaseContext.ChallengeProgress?
                    .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

                client.ActiveCharacter.ChallengeProgress = challengeProgressList;
                S2CChallengeProgressAnswerPacket challengeProgressAnswerPacket = new S2CChallengeProgressAnswerPacket(challengeProgressList ?? new List<ChallengeProgress>());
                client.PacketStream.Write(challengeProgressAnswerPacket);
            }
        }
    }
}

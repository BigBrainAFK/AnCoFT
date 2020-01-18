using System.Collections.Generic;
using System.Linq;
using AnCoFT.Database.Models;
using AnCoFT.Networking.Packet.Guild;

namespace AnCoFT.Networking.PacketHandler
{
    public static class GuildPacketHandler
    {
        public static void HandleGuildListRequest(Client client, Packet.Packet packet)
        {
            List<Guild> guildList = client.DatabaseContext.Guild.ToList();
            S2CGuildListAnswerPacket guildListAnswerPacket = new S2CGuildListAnswerPacket(guildList);
            client.PacketStream.Write(guildListAnswerPacket);
        }

        public static void HandleGuildLeagueRankingRequest(Client client, Packet.Packet packet)
        {
            List<Guild> guildList = client.DatabaseContext.Guild.OrderByDescending(g => g.LeaguePoints).ToList();
            S2CGuildLeagueRankingAnswerPacket guildLeagueRankingAnswerPacket = new S2CGuildLeagueRankingAnswerPacket(guildList);
            client.PacketStream.Write(guildLeagueRankingAnswerPacket);
        }

        public static void HandleGuildDataRequest(Client client, Packet.Packet packet)
        {
            if (client.ActiveCharacter.GuildMember == null)
            {
                S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(1);
                client.PacketStream.Write(guildDataPacket);
            }
            else if (client.ActiveCharacter.GuildMember.WaitingForApproval)
            {
                S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(-1);
                client.PacketStream.Write(guildDataPacket);
            }
            else
            {
                S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(0, client.ActiveCharacter.GuildMember.Guild);
                client.PacketStream.Write(guildDataPacket);
            }
        }

        public static void HandleGuildChangeInformationRequest(Client client, Packet.Packet packet)
        {
            C2SGuildChangeInformationRequestPacket guildChangeInformationRequestPacket = new C2SGuildChangeInformationRequestPacket(packet);
            Guild guild = client.DatabaseContext.Guild.Find(client.ActiveCharacter.GuildMember.GuildId);
            guild.Introduction = guildChangeInformationRequestPacket.Introduction;
            guild.LevelRestriction = guildChangeInformationRequestPacket.MinLevel;
            guild.Public = guildChangeInformationRequestPacket.Public;
            guild.AllowedCharacterType = guildChangeInformationRequestPacket.AllowCharacterType;
            client.DatabaseContext.SaveChanges();
        }

        public static void HandleGuildMemberDataRequest(Client client, Packet.Packet packet)
        {
            List<GuildMember> memberList = client.DatabaseContext.GuildMember
                ?.Where(gm => gm.GuildId == client.ActiveCharacter.GuildMember.GuildId)?.ToList() ?? new List<GuildMember>();

            S2CGuildMemberDataPacket guildMemberDataPacket = new S2CGuildMemberDataPacket(memberList);
            client.PacketStream.Write(guildMemberDataPacket);
        }

        public static void HandleGuildReserveMemberDataRequest(Client client, Packet.Packet packet)
        {
            S2CGuildReserveMemberDataPacket guildReserveMemberDataPacket = new S2CGuildReserveMemberDataPacket();
            client.PacketStream.Write(guildReserveMemberDataPacket);
        }

        public static void HandleGuildGoldDataRequest(Client client, Packet.Packet packet)
        {
            Packet.Packet testAnswer = new Packet.Packet(0x202D);
            testAnswer.Write((short)0);
            testAnswer.Write((byte)1);
            testAnswer.Write(1); // Acc or Num
            testAnswer.Write(1); // Char Id
            testAnswer.Write(1); // Date
            testAnswer.Write(4); // Gold Used
            testAnswer.Write("Name");
            testAnswer.Write(6); // Gold Remaining

            client.PacketStream.Write(testAnswer);
        }
    }
}

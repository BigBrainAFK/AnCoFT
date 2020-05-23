namespace AnCoFT.Networking.PacketHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AnCoFT.Database.Models;
    using AnCoFT.Game.Guild;
    using AnCoFT.Networking.Packet;
    using AnCoFT.Networking.Packet.Guild;

    public static class GuildPacketHandler
    {
        public static void HandleGuildListRequest(Client client, Packet packet)
        {
            List<Guild> guildList = client.DatabaseContext.Guild.ToList();
            S2CGuildListAnswerPacket guildListAnswerPacket = new S2CGuildListAnswerPacket(guildList);
            client.PacketStream.Write(guildListAnswerPacket);
        }

        public static void HandleGuildSearchRequest(Client client, Packet packet)
        {
            C2SGuildSearchRequestPacket guildSearchRequest = new C2SGuildSearchRequestPacket(packet);
            List<Guild> guildList = new List<Guild>();
            if (guildSearchRequest.SearchType == GuildSearchType.Number)
            {
                Guild searchResult = client.DatabaseContext.Guild.Find(guildSearchRequest.Number);
                if (searchResult != null && searchResult.Id > 0)
                    guildList.Add(searchResult);
            }
            else
            {
                List<Guild> searchResult = client.DatabaseContext.Guild?
                    .Where(g => g.Name.ToLower().Contains(guildSearchRequest.Name.ToLower())).ToList();

                if (searchResult != null && searchResult.Count > 0)
                    guildList.AddRange(searchResult);
            }

            S2CGuildSearchAnswerPacket guildSearchAnswer = new S2CGuildSearchAnswerPacket(guildList);
            client.PacketStream.Write(guildSearchAnswer);
        }

        public static void HandleGuildJoinRequest(Client client, Packet packet)
        {
            C2SGuildJoinRequestPacket guildJoinRequest = new C2SGuildJoinRequestPacket(packet);

            Guild guild = client.DatabaseContext.Guild.Find(guildJoinRequest.GuildId);
            if (guild == null || guild.Id <= 0 || guild.Members.Count == guild.MaxMemberCount)
            {
                S2CGuildJoinAnswerPacket guildJoinAnswer = new S2CGuildJoinAnswerPacket(-1);
                client.PacketStream.Write(guildJoinAnswer);
            }
            else
            {
                GuildMember guildMember = new GuildMember
                {
                    CharacterId = client.ActiveCharacter.CharacterId,
                    Division = GuildDivision.ClubMember,
                    RequestDate = DateTime.Now,
                    GuildId = guild.Id,
                    WaitingForApproval = !guild.Public,
                };
                client.DatabaseContext.GuildMember.Add(guildMember);
                client.DatabaseContext.SaveChanges();

                client.ActiveCharacter.GuildMember = guildMember;

                if (guild.Public)
                {
                    S2CGuildJoinAnswerPacket guildJoinAnswer = new S2CGuildJoinAnswerPacket(1);
                    client.PacketStream.Write(guildJoinAnswer);

                    S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(GuildStatus.InGuild, guild);
                    client.PacketStream.Write(guildDataPacket);
                }
                else
                {
                    S2CGuildJoinAnswerPacket guildJoinAnswer = new S2CGuildJoinAnswerPacket(0);
                    client.PacketStream.Write(guildJoinAnswer);

                    S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(GuildStatus.WaitingForApproval);
                    client.PacketStream.Write(guildDataPacket);
                }
            }
        }

        public static void HandleGuildLeaveRequest(Client client, Packet packet)
        {
            if (client.ActiveCharacter.GuildMember != null)
            {
                client.DatabaseContext.GuildMember.Remove(client.ActiveCharacter.GuildMember);
                client.DatabaseContext.SaveChanges();
                client.ActiveCharacter.GuildMember = null;

                S2CGuildDataPacket guildData = new S2CGuildDataPacket(GuildStatus.NoGuild);
                client.PacketStream.Write(guildData);
            }
        }

        public static void HandleGuildDataRequest(Client client)
        {
            if (client.ActiveCharacter.GuildMember == null)
            {
                S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(GuildStatus.NoGuild);
                client.PacketStream.Write(guildDataPacket);
            }
            else if (client.ActiveCharacter.GuildMember.WaitingForApproval)
            {
                S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(GuildStatus.WaitingForApproval);
                client.PacketStream.Write(guildDataPacket);
            }
            else
            {
                S2CGuildDataPacket guildDataPacket = new S2CGuildDataPacket(GuildStatus.InGuild, client.ActiveCharacter.GuildMember.Guild);
                client.PacketStream.Write(guildDataPacket);
            }
        }

        public static void HandleGuildNameCheckRequest(Client client, Packet packet)
        {
            C2SGuildNameCheckRequestPacket guildNameCheckRequest = new C2SGuildNameCheckRequestPacket(packet);
            if (client.DatabaseContext.Guild?.FirstOrDefault(g => g.Name == guildNameCheckRequest.GuildName)?.Id > 0)
            {
                S2CGuildNameCheckAnswerPacket guildNameCheckAnswer = new S2CGuildNameCheckAnswerPacket(-1);
                client.PacketStream.Write(guildNameCheckAnswer);
            }
            else
            {
                S2CGuildNameCheckAnswerPacket guildNameCheckAnswer = new S2CGuildNameCheckAnswerPacket(0);
                client.PacketStream.Write(guildNameCheckAnswer);
            }
        }

        public static void HandleGuildCreateRequest(Client client, Packet packet)
        {
            C2SGuildCreateRequestPacket guildCreateRequest = new C2SGuildCreateRequestPacket(packet);

            Guild newGuild = new Guild
            {
                Name = guildCreateRequest.Name,
                Introduction = guildCreateRequest.Introduction,
                Public = guildCreateRequest.Public,
                LevelRestriction = guildCreateRequest.LevelRestriction,
                AllowedCharacterType = guildCreateRequest.AllowedCharacterType.ToArray(),
                CreationDay = DateTime.Now,
            };

            client.DatabaseContext.Guild.Add(newGuild);
            client.DatabaseContext.SaveChanges();

            GuildMember newGuildMember = new GuildMember
            {
                CharacterId = client.ActiveCharacter.CharacterId,
                Guild = newGuild,
                RequestDate = DateTime.Now,
                Division = GuildDivision.ClubMaster,
            };
            client.DatabaseContext.GuildMember.Add(newGuildMember);
            client.DatabaseContext.SaveChanges();

            S2CGuildCreateAnswerPacket guildCreateAnswer = new S2CGuildCreateAnswerPacket(0);
            client.PacketStream.Write(guildCreateAnswer);

            S2CGuildDataPacket guildData = new S2CGuildDataPacket(GuildStatus.InGuild, newGuild);
            client.PacketStream.Write(guildData);
        }

        public static void HandleGuildChangeInformationRequest(Client client, Packet packet)
        {
            C2SGuildChangeInformationRequestPacket guildChangeInformationRequestPacket = new C2SGuildChangeInformationRequestPacket(packet);

            Guild guild = client.DatabaseContext.Guild.Find(client.ActiveCharacter.GuildMember.GuildId);
            guild.Introduction = guildChangeInformationRequestPacket.Introduction;
            guild.LevelRestriction = guildChangeInformationRequestPacket.MinLevel;
            guild.Public = guildChangeInformationRequestPacket.Public;
            guild.AllowedCharacterType = guildChangeInformationRequestPacket.AllowCharacterType;
            client.DatabaseContext.SaveChanges();
        }

        public static void HandleGuildChangeNoticeRequest(Client client, Packet packet)
        {
            C2SGuildChangeNoticeRequestPacket guildChangeNoticeRequest = new C2SGuildChangeNoticeRequestPacket(packet);

            Guild guild = client.DatabaseContext.Guild.Find(client.ActiveCharacter.GuildMember.GuildId);
            if (client.ActiveCharacter.GuildMember.GuildId == guild.Id &&
                client.ActiveCharacter.GuildMember.Division == GuildDivision.ClubMaster)
            {
                guild.Notice = guildChangeNoticeRequest.Notice;
                client.DatabaseContext.SaveChanges();
            }

            S2CGuildChangeNoticeAnswerPacket guildChangeNoticeAnswer = new S2CGuildChangeNoticeAnswerPacket(0);
            client.PacketStream.Write(guildChangeNoticeAnswer);
        }

        public static void HandleGuildMemberDataRequest(Client client)
        {
            List<GuildMember> memberList = client.DatabaseContext.GuildMember
                ?.Where(gm => gm.GuildId == client.ActiveCharacter.GuildMember.GuildId && !gm.WaitingForApproval).ToList() ?? new List<GuildMember>();

            S2CGuildMemberDataPacket guildMemberDataPacket = new S2CGuildMemberDataPacket(memberList);
            client.PacketStream.Write(guildMemberDataPacket);
        }

        public static void HandleGuildReserveMemberDataRequest(Client client)
        {
            List<GuildMember> memberList = client.DatabaseContext.GuildMember
                                               ?.Where(gm => gm.GuildId == client.ActiveCharacter.GuildMember.GuildId && gm.WaitingForApproval).ToList() ?? new List<GuildMember>();

            S2CGuildReserveMemberDataPacket guildReserveMemberDataPacket = new S2CGuildReserveMemberDataPacket(memberList);
            client.PacketStream.Write(guildReserveMemberDataPacket);
        }

        public static void HandleGuildChangeReverseMemberRequest(Client client, Packet packet)
        {
            C2SGuildChangeReverseMemberRequestPacket guildChangeReverseMemberRequest = new C2SGuildChangeReverseMemberRequestPacket(packet);

            GuildMember guildMember = client.DatabaseContext.GuildMember.FirstOrDefault(
                gm => gm.CharacterId == guildChangeReverseMemberRequest.CharacterId);

            if (guildMember != null)
            {
                if (guildChangeReverseMemberRequest.Status == GuildMemberStatus.Approve)
                    guildMember.WaitingForApproval = false;
                else
                    client.DatabaseContext.GuildMember.Remove(guildMember);

                client.DatabaseContext.SaveChanges();
            }

            S2CGuildChangeReverseMemberAnswerPacket guildChangeReverseMemberAnswer = new S2CGuildChangeReverseMemberAnswerPacket(guildChangeReverseMemberRequest.Status);
            client.PacketStream.Write(guildChangeReverseMemberAnswer);
        }

        public static void HandleGuildChangeSubMasterRequest(Client client, Packet packet)
        {
            C2SGuildChangeSubMasterRequestPacket guildChangeSubMasterRequest = new C2SGuildChangeSubMasterRequestPacket(packet);

            GuildMember guildMember = client.DatabaseContext.GuildMember.FirstOrDefault(
                gm => gm.CharacterId == guildChangeSubMasterRequest.CharacterId);

            if (guildMember != null)
            {
                if (guildChangeSubMasterRequest.Status == GuildMemberStatus.Approve)
                    guildMember.Division = GuildDivision.SubClubMaster;
                else
                    guildMember.Division = GuildDivision.ClubMember;

                client.DatabaseContext.SaveChanges();
            }

            S2CGuildChangeSubMasterAnswerPacket guildChangeSubMasterAnswer = new S2CGuildChangeSubMasterAnswerPacket(guildChangeSubMasterRequest.Status);
            client.PacketStream.Write(guildChangeSubMasterAnswer);
        }

        public static void HandleGuildChangeMasterRequest(Client client, Packet packet)
        {
            C2SGuildChangeMasterRequestPacket guildChangeSubMasterRequest = new C2SGuildChangeMasterRequestPacket(packet);

            GuildMember guildMember = client.DatabaseContext.GuildMember.FirstOrDefault(
                gm => gm.CharacterId == guildChangeSubMasterRequest.CharacterId);

            if (guildMember != null && client.ActiveCharacter.GuildMember.Division == GuildDivision.ClubMaster)
            {
                GuildMember currentMaster =
                    client.DatabaseContext.GuildMember.FirstOrDefault(gm =>
                        gm.CharacterId == client.ActiveCharacter.CharacterId);
                currentMaster.Division = GuildDivision.SubClubMaster;

                guildMember.Division = GuildDivision.ClubMaster;
                client.DatabaseContext.SaveChanges();
            }

            S2CGuildChangeMasterAnswerPacket guildChangeMasterAnswer = new S2CGuildChangeMasterAnswerPacket(0);
            client.PacketStream.Write(guildChangeMasterAnswer);
        }

        public static void HandleGuildDismissMemberRequest(Client client, Packet packet)
        {
            C2SGuildDismissMemberRequestPacket guildDismissMemberRequest = new C2SGuildDismissMemberRequestPacket(packet);

            if (client.ActiveCharacter.GuildMember.Division == GuildDivision.ClubMember)
            {
                S2CGuildDismissMemberAnswerPacket guildDismissMemberAnswer = new S2CGuildDismissMemberAnswerPacket(-1);
                client.PacketStream.Write(guildDismissMemberAnswer);
            }
            else
            {
                GuildMember guildMember = client.DatabaseContext.GuildMember.FirstOrDefault(gm => gm.CharacterId == guildDismissMemberRequest.CharacterId);
                if (guildMember != null)
                {
                    client.DatabaseContext.GuildMember.Remove(guildMember);
                    client.DatabaseContext.SaveChanges();

                    S2CGuildDismissMemberAnswerPacket guildDismissMemberAnswer = new S2CGuildDismissMemberAnswerPacket(0);
                    client.PacketStream.Write(guildDismissMemberAnswer);
                }
            }
        }

        public static void HandleGuildGoldDataRequest(Client client, Packet packet)
        {
            Guild guild = client.ActiveCharacter.GuildMember.Guild;
            S2CGuildGoldDataPacket guildGoldData = new S2CGuildGoldDataPacket(guild);
            client.PacketStream.Write(guildGoldData);
        }

        public static void HandleGuildChatRequest(Client client, Packet packet)
        {
            C2SGuildChatRequestPacket guildChatRequest = new C2SGuildChatRequestPacket(packet);
            S2CGuildChatAnswerPacket guildChatAnswer = new S2CGuildChatAnswerPacket(client.ActiveCharacter.Name, guildChatRequest.Message);
            client.PacketStream.Write(guildChatAnswer);
        }

        public static void HandleGuildDeleteRequest(Client client, Packet packet)
        {
            S2CGuildDeleteAnswerPacket guildDeleteAnswer = new S2CGuildDeleteAnswerPacket(0);
            client.PacketStream.Write(guildDeleteAnswer);

            if (client.ActiveCharacter.GuildMember.Division == GuildDivision.ClubMaster)
            {
                client.DatabaseContext.Guild.Remove(client.ActiveCharacter.GuildMember.Guild);
                client.DatabaseContext.SaveChanges();

                S2CGuildDataPacket guildData = new S2CGuildDataPacket(GuildStatus.InGuild);
                client.PacketStream.Write(guildData);
            }
        }
    }
}

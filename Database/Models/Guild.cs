﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class Guild
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Notice { get; set; }
        public int Emblem { get; set; }
        public byte MaxMemberCount { get; set; }
        public byte Level { get; set; }
        public int ClubPoints { get; set; }
        public int LeaguePoints { get; set; }
        public int ClubGold { get; set; }
        public int BattleRecordWin { get; set; }
        public int BattleRecordLoose { get; set; }
        public int LeagueRecordWin { get; set; }
        public int LeagueRecordLoose { get; set; }
        public byte LevelRestriction { get; set; }
        public bool Public { get; set; }
        public int CreationDay { get; set; }
        public List<byte> AllowedCharacterType { get; set; }
        public List<GuildMember> Members { get; set; }
        public List<GuildGoldUsage> GoldUsages { get; set; }
    }
}

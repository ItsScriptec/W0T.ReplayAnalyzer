using System;
using System.Collections.Generic;
using System.Text;

namespace W0T.ReplayAnalyzer
{
    //   P L A Y E R
    //   The Player class contains the most important
    //   informations about a player of a battle

    public class Player
    {
        public int id;
        public string name;
        public string clanAbbrev;
        public int? clanDBID;
        public int? crewGroup;
        public int team;
        public string vehicle;
        public int preBattleID;
        public string igrType;
        public bool? forbidInBattleInvitations;
        public results results;
    }
}

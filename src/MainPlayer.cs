﻿using System;
using System.Collections.Generic;
using System.Text;

namespace W0T.ReplayAnalyzer
{
    //   M A I N P L A Y E R
    //   The MainPlayer class contains all informations about
    //   the player who recorded a replay

    public class MainPlayer
    {
        public string id;
        public string clientVersionFromXml;
        public string clientVersionFromExe;
        public string name;
        public string clanAbbrev;
        public int? clanDBID;
        public int? crewGroup;
        public int team;
        public string vehicle;
        public int preBattleID;
        public string igrType = null;
        public bool? forbidInBattleInvitations;
        public bool? hasMods;
        public numbers results = null;
        public List<PersonalDetails> details = null;
    }

    public class PersonalDetails
    {
        public string id;
        public int spotted;
        public int crits;
        public int damageAssistedTrack;
        public int? damageAssistedStun;
        public int fire;
        public int deathReason;
        public int damageReceived;
        public int damageDealt;
        public int damageAssistedRadio;
        public int rickochetsReceived;
        public double? stunDuration;
        public int piercings;
        public int explosionHits;
        public int damageBlockedByArmor;
        public int noDamageDirectHitsReceived;
        public int targetKills;
        public int? stunNum;
        public int directHits;
    }

    public class numbers
    {
        public int vehTypeLockTime;
        public int? stunned;
        public int creditsToDraw;
        public int orderFreeXPFactor100;
        public int orderXPFactor100;
        public int damageAssistedRadio;
        public int? stunDuration;
        public int? freeXPReplay;
        public int winPoints;
        public bool stopRespawn;
        public int creditsContributionIn;
        public int eventCredits;
        public int[] eventXPList;
        public int? xpReplay;
        public int achievementXP;
        public int igrXPFactor10;
        public int aogasFactor10;
        public int originalCreditsContributionIn;
        public int originalCreditsPenalty;
        public int damagedWhileMoving;
        public int kills;
        public int eventTMenXP;
        public double percentFromTotalTeamDamage;
        public int originalTMenXP;
        public int markOfMastery;
        public int noDamageDirectHitsReceived;
        public int boosterCredits;
        public int originalGold;
        public int[] eventFreeXPList;
        public int tkills;
        public int shots;
        public int deathCount;
        public int directHits;
        public int spotted;
        public object extPublic;
        public int subtotalTMenXP;
        public int? damageReceivedFromInvisibles;
        public int boosterCreditsFactor100;
        public int premiumCreditsFactor10;
        public int soloFlagCapture;
        public int marksOnGun;
        public int? premiumVehicleXPFactor100;
        public int factualXP;
        public int killedAndDamagedByAllSquadmates;
        public int eventFreeXP;
        public int[] eventGoldFactor100List;
        public int creditsContributionOut;
        public int? damageEventList;
        public int health;
        public int orderFreeXP;
        public int[] eventGoldList;
        public int boosterTMenXPFactor100;
        public int tdamageDealt;
        public int resourceAbsorbed;
        public int? goldReplay;
        public int originalXP;
        public int credits;
        public int damagedWhileEnemyMoving;
        public int creditsPenalty;
        public int damageDealt;
        public double percentFromSecondBestDamage;
        public bool committedSuicide;
        public int boosterXP;
        public int lifeTime;
        public int[] eventTMenXPList;
        public int dailyXPFactor10;
        public int damageRating;
        public int repair;
        public int originalCredits;
        public int damageAssistedTrack;
        public int xpPenalty;
        public int sniperDamageDealt;
        public int fairplayFactor10;
        public int orderCreditsFactor100;
        public int? originalCrystal;
        public int damageBlockedByArmor;
        public int xp;
        public int boosterXPFactor100;
        public int killerID;
        public int refSystemXPFactor10;
        public int orderTMenXP;
        public int originalXPPenalty;
        public int orderTMenXPFactor100;
        public object eventXPFactor100List;
        public int subtotalXP;
        public int? squadXP;
        public int originalCreditsContributionOut;
        public int originalFreeXP;
        public int orderCredits;
        public int freeXP;
        public int orderXP;
        public int premiumVehicleXP;
        public int flagCapture;
        public int[] eventCreditsList;
        public int eventGold;
        public int gold;
        public int eventXP;
        public int factualCredits;
        public int subtotalFreeXP;
        public int? crystal;
        public int? crystalReplay;
        public int? stunNum;
        public int achievementFreeXP;
        public int subtotalCredits;
        public int killsBeforeTeamWasDamaged;
        public int boosterTMenXP;
        public int potentialDamageReceived;
        public int directTeamHits;
        public int damageReceived;
        public int piercingsReceived;
        public int movingAvgDamage;
        public int premiumXPFactor10;
        public int? creditsReplay;
        public int piercings;
        public int prevMarkOfMastery;
        public int[] eventFreeXPFactor100List;
        public int serviceProviderID;
        public int droppedCapturePoints;
        public int directHitsReceived;
        public int typeCompDescr;
        public int deathReason;
        public int capturePoints;
        public int damageBeforeTeamWasDamaged;
        public int? subtotalCrystal;
        public int explosionHitsReceived;
        public object questsProgress;
        public object eventTMenXPFactor100List;
        public int[] achievements;
        public object dossierPopUps;
        public int[] autoEquipCost;
        public int[][] xpByTmen;
        public int[] flagActions;
        public string[][] eventCrystalList;
        public int[] autoLoadCost;
        public int? squadXPFactor100;
        public int achievementCredits;
        public int? originalCreditsToDraw;
        public bool isPremium;
        public int mileage;
        public int explosionHits;
        public int rolloutsCount;
        public int index;
        public int?[] avatarDamageEventList;
        public int subtotalGold;
        public int appliedPremiumCreditsFactor10;
        public int damaged;
        public int accountDBID;
        public int? eventCrystal;
        public int? tmenXPReplay;
        public int autoRepairCost;
        public int[] eventCreditsFactor100List;
        public bool isTeamKiller;
        public int tmenXP;
        public int factualFreeXP;
        public int? capturingBase;
        public int? damageAssistedStun;
        public int appliedPremiumXPFactor10;
        public int boosterFreeXPFactor100;
        public int boosterFreeXP;
        public int tdestroyedModules;
        public int battleNum;
    }
}

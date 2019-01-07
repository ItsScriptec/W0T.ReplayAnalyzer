using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace W0T.ReplayAnalyzer
{
    //   R E P L A Y
    //   The class Replay gets all important informations from the json blocks.
    //   Methods: Battle, Players, MainPlayer

    public class Replay
    {
        // variables
        JObject blockOne;
        JObject blockTwo;
        List<Player> players = new List<Player>();

        public int isFileOK;

        // constructor
        public Replay(string replayFilePath)
        {
            ReplayJson converter = new Converter(replayFilePath).ToJson();

            // check if the file is ok
            isFileOK = converter.isFileOK;

            // get the first json block
            blockOne = converter.BlockOne;

            // get the second json block
            if (converter.isFileOK == 2)
            {
                JArray blockTwoArray = JsonConvert.DeserializeObject<JArray>(converter.BlockTwo.ToString());
                blockTwo = (JObject)blockTwoArray.First;
            }
        }

        // get the settings of the battle
        public BattleSettings Battle()
        {
            BattleSettings set = new BattleSettings();

            set.regionCode = (string)blockOne["regionCode"];
            set.serverName = (string)blockOne["serverName"];
            set.dateTime = (string)blockOne["dateTime"];
            set.mapDisplayName = (string)blockOne["mapDisplayName"];
            set.mapName = (string)blockOne["mapName"];
            set.gameplayID = (string)blockOne["gameplayID"];
            set.battleType = (string)blockOne["battleType"];

            return set;
        }

        // get the players (with battle-results)
        public List<Player> Players()
        {
            if (players.Count > 0)
            {
                return players;
            }

            // get the "vehicles" json-part
            JObject vehiclesJson = JObject.Parse(blockOne["vehicles"].ToString());

            // for each player in "vehicles"
            foreach (var player in vehiclesJson)
            {
                // create a new instance of Player class
                Player currentPlayer = new Player();

                // get the players json part
                JObject playerJson = JObject.Parse(player.Value.ToString());

                // STANDARD INFORMATIONS
                currentPlayer.id = Convert.ToInt32(player.Key);
                currentPlayer.name = (string)playerJson["name"];
                currentPlayer.team = (int)playerJson["team"];
                currentPlayer.vehicle = (string)playerJson["vehicleType"];
                currentPlayer.crewGroup = (int?)playerJson["crewGroup"];
                currentPlayer.clanAbbrev = (string)playerJson["clanAbbrev"];
                currentPlayer.forbidInBattleInvitations = (bool?)playerJson["forbidInBattleInvitations"];

                // get the player-results json part
                JObject allresults = blockTwo?.ToObject<JObject>();
                allresults = allresults?["vehicles"]?.ToObject<JObject>();

                // if the results part is not null (older replays don't have this part)
                if (allresults != null)
                {
                    // get the right json part
                    JObject playerresults = JObject.Parse(JArray.Parse(allresults[player.Key].ToString())[0].ToString());

                    // create new instance of results
                    results res = new results();

                    // XP
                    res.xp = (int)playerresults["xp"];
                    res.achievementXP = (int)playerresults["achievementXP"];
                    res.achievementFreeXP = (int)playerresults["achievementFreeXP"];

                    // CREDITS
                    res.credits = (int)playerresults["credits"];
                    res.achievementCredits = (int)playerresults["achievementCredits"];

                    // DID
                    res.shots = (int)playerresults["shots"];
                    res.explosionHits = (int)playerresults["explosionHits"];
                    res.directHits = (int)playerresults["directHits"];
                    res.piercings = (int)playerresults["piercings"];
                    res.directTeamHits = (int)playerresults["directTeamHits"];
                    res.kills = (int)playerresults["kills"];
                    res.damaged = (int)playerresults["damaged"];
                    res.damageDealt = (int)playerresults["damageDealt"];
                    res.sniperDamageDealt = (int)playerresults["sniperDamageDealt"];
                    res.damageEventList = playerresults["damageEventList"].ToObject<int?[]>();

                    res.isTeamKiller = (bool)playerresults["isTeamKiller"];
                    res.tkills = (int)playerresults["tkills"];
                    res.tdamageDealt = (int)playerresults["tdamageDealt"];
                    res.tdestroyedModules = (int)playerresults["tdestroyedModules"];

                    // ASSIST
                    res.spotted = (int)playerresults["spotted"];
                    res.damageAssistedRadio = (int)playerresults["damageAssistedRadio"];
                    res.damageAssistedTrack = (int)playerresults["damageAssistedTrack"];
                    res.stunNum = (int?)playerresults["stunNum"];
                    res.stunDuration = (double?)playerresults["stunDuration"];
                    res.stunned = (int?)playerresults["stunned"];
                    res.damageAssistedStun = (int?)playerresults["damageAssistedStun"];

                    // RECEIVED & BLOCKED
                    res.directHitsReceived = (int)playerresults["directHitsReceived"];
                    res.potentialDamageReceived = (int)playerresults["potentialDamageReceived"];
                    res.damageReceived = (int)playerresults["damageReceived"];
                    res.damageReceivedFromInvisible = (int?)playerresults["damageReceivedFromInvisible"];
                    res.piercingsReceived = (int)playerresults["piercingsReceived"];
                    res.noDamageDirectHitsReceived = (int)playerresults["noDamageDirectHitsReceived"];
                    res.explosionHitsReceived = (int)playerresults["explosionHitsReceived"];
                    res.damageBlockedByArmor = (int)playerresults["damageBlockedByArmor"];

                    // FLAG
                    res.soloFlagCapture = (int)playerresults["soloFlagCapture"];
                    res.capturePoints = (int)playerresults["capturePoints"];
                    res.flagCapture = (int)playerresults["flagCapture"];
                    res.capturingBase = (int?)playerresults["capturingBase"];
                    res.flagActions = playerresults["flagActions"].ToObject<int[]>();
                    res.droppedCapturePoints = (int)playerresults["droppedCapturePoints"];

                    // DEATH
                    res.killerID = (int)playerresults["killerID"];
                    res.deathReason = (int)playerresults["deathReason"];
                    res.deathCount = (int)playerresults["deathCount"];
                    res.lifeTime = (int)playerresults["lifeTime"];

                    // OTHERS
                    res.health = (int)playerresults["health"];
                    res.extPublic = (object)playerresults["extPublic"];
                    res.stopRespawn = (bool)playerresults["stopRespawn"];
                    res.index = (int)playerresults["index"];
                    res.achievements = playerresults["achievements"].ToObject<string[]>();
                    res.mileage = (int)playerresults["mileage"];
                    res.resourceAbsorbed = (int)playerresults["resourceAbsorbed"];
                    res.accountDBID = (int)playerresults["accountDBID"];
                    res.typeCompDescr = (int)playerresults["typeCompDescr"];
                    res.rolloutsCount = (int)playerresults["rolloutsCount"];
                    res.winPoints = (int)playerresults["winPoints"];

                    // set the result of the current player
                    currentPlayer.results = res;
                }

                // add the current player to the Players List
                players.Add(currentPlayer);
            }

            // return the List of all players
            return players;
        }

        // get all informations about the recording player
        public MainPlayer MainPlayer()
        {
            // get the players name
            string name = Convert.ToString(blockOne["playerName"]);

            // create instance for the main player from the players list
            Player thisplayer = new Player();

            // create instance for the main player
            MainPlayer myplayer = new MainPlayer();

            // start the Players functon to get a list of all players
            if (players.Count < 1)
            {
                Players();
            }

            // for each player in the players list
            foreach (Player player in players)
            {
                // if the players name == mainplayers name
                if (player.name == name)
                {
                    // set thisplayer as the current player
                    thisplayer = player;
                    break;
                }
            }

            // STANDARD INFORMATIONS
            myplayer.id = (string)blockOne["playerID"];
            myplayer.name = thisplayer.name;
            myplayer.clanAbbrev = thisplayer.clanAbbrev;
            myplayer.clanDBID = thisplayer.clanDBID;
            myplayer.crewGroup = thisplayer.crewGroup;
            myplayer.team = thisplayer.team;
            myplayer.vehicle = thisplayer.vehicle;
            myplayer.preBattleID = thisplayer.preBattleID;
            myplayer.igrType = thisplayer.igrType;
            myplayer.forbidInBattleInvitations = thisplayer.forbidInBattleInvitations;
            myplayer.hasMods = (bool?)blockOne["hasMods"];
            myplayer.clientVersionFromXml = (string)blockOne["clientVersionFromXml"];
            myplayer.clientVersionFromExe = (string)blockOne["clientVersionFromExe"];

            // get the "personal" json part
            if (blockTwo != null)
            {
                var personal = JObject.Parse(blockTwo.ToString());
                personal = personal["personal"]?.ToObject<JObject>();
                personal = personal?.First?.First?.ToObject<JObject>();

                numbers res = new numbers();

                ////////// XP

                // XP FACTOR 100
                res.orderFreeXPFactor100 = (int)personal["orderFreeXPFactor100"];
                res.orderXPFactor100 = (int)personal["orderXPFactor100"];
                res.boosterTMenXPFactor100 = (int)personal["boosterTMenXPFactor100"];
                res.premiumVehicleXPFactor100 = (int?)personal["premiumVehicleXPFactor100"];
                res.boosterXPFactor100 = (int)personal["boosterXPFactor100"];
                res.orderTMenXPFactor100 = (int)personal["orderTMenXPFactor100"];
                res.squadXPFactor100 = (int?)personal["squadXPFactor100"];
                res.boosterFreeXPFactor100 = (int)personal["boosterFreeXPFactor100"];

                // XP FACTOR 10
                res.premiumXPFactor10 = (int)personal["premiumXPFactor10"];
                res.igrXPFactor10 = (int)personal["igrXPFactor10"];
                res.dailyXPFactor10 = (int)personal["dailyXPFactor10"];
                res.refSystemXPFactor10 = (int)personal["refSystemXPFactor10"];
                res.appliedPremiumXPFactor10 = (int)personal["appliedPremiumXPFactor10"];

                // XP VALUES
                res.xp = (int)personal["xp"];
                res.originalXP = (int)personal["originalXP"];
                res.orderXP = (int)personal["orderXP"];
                res.achievementXP = (int)personal["achievementXP"];
                res.eventTMenXP = (int)personal["eventTMenXP"];
                res.eventXP = (int)personal["eventXP"];
                res.tmenXP = (int)personal["tmenXP"];
                res.originalTMenXP = (int)personal["originalTMenXP"];
                res.subtotalTMenXP = (int)personal["subtotalTMenXP"];
                res.factualXP = (int)personal["factualXP"];
                res.eventFreeXP = (int)personal["eventFreeXP"];
                res.orderFreeXP = (int)personal["orderFreeXP"];
                res.freeXP = (int)personal["freeXP"];
                res.subtotalFreeXP = (int)personal["subtotalFreeXP"];
                res.originalFreeXP = (int)personal["originalFreeXP"];
                res.achievementFreeXP = (int)personal["achievementFreeXP"];
                res.orderTMenXP = (int)personal["orderTMenXP"];
                res.boosterTMenXP = (int)personal["boosterTMenXP"];
                res.boosterXP = (int)personal["boosterXP"];
                res.boosterFreeXP = (int)personal["boosterFreeXP"];
                res.subtotalXP = (int)personal["subtotalXP"];
                res.squadXP = (int?)personal["squadXP"];
                res.premiumVehicleXP = (int)personal["premiumVehicleXP"];
                res.factualFreeXP = (int)personal["factualFreeXP"];

                // XP PENALTY
                res.originalXPPenalty = (int)personal["originalXPPenalty"];
                res.xpPenalty = (int)personal["xpPenalty"];

                // XP REPLAY
                res.freeXPReplay = (int?)personal["freeXPReplay"];
                res.xpReplay = (int?)personal["xpReplay"];
                res.tmenXPReplay = (int?)personal["tmenXPReplay"];

                // XP LISTS
                res.eventXPList = personal["eventXPList"].ToObject<int[]>();
                res.eventFreeXPList = personal["eventFreeXPList"].ToObject<int[]>();
                res.eventTMenXPList = personal["eventTMenXPList"].ToObject<int[]>();
                res.eventXPFactor100List = (object)personal["eventXPFactor100List"];
                res.eventFreeXPFactor100List = personal["eventFreeXPFactor100List"].ToObject<int[]>();
                res.eventTMenXPFactor100List = (object)personal["eventTMenXPFactor100List"];
                res.xpByTmen = personal["xpByTmen"].ToObject<int[][]>();

                ////////// CREDITS

                // CREDIT FACTOR 100
                res.boosterCreditsFactor100 = (int)personal["boosterCreditsFactor100"];
                res.orderCreditsFactor100 = (int)personal["orderCreditsFactor100"];

                // CREDIT FACTOR 10
                res.premiumCreditsFactor10 = (int)personal["premiumCreditsFactor10"];
                res.appliedPremiumCreditsFactor10 = (int)personal["appliedPremiumCreditsFactor10"];

                // CREDIT VALUES
                res.credits = (int)personal["credits"];
                res.originalCredits = (int)personal["originalCredits"];
                res.boosterCredits = (int)personal["boosterCredits"];
                res.creditsToDraw = (int)personal["creditsToDraw"];
                res.originalCreditsToDraw = (int?)personal["originalCreditsToDraw"];
                res.eventCredits = (int)personal["eventCredits"];
                res.orderCredits = (int)personal["orderCredits"];
                res.factualCredits = (int)personal["factualCredits"];
                res.subtotalCredits = (int)personal["subtotalCredits"];
                res.creditsReplay = (int?)personal["creditsReplay"];
                res.achievementCredits = (int)personal["achievementCredits"];

                // CREDIT PENALTY
                res.originalCreditsPenalty = (int)personal["originalCreditsPenalty"];
                res.creditsPenalty = (int)personal["creditsPenalty"];

                // CREDIT CONTRIBUTION
                res.creditsContributionIn = (int)personal["creditsContributionIn"];
                res.creditsContributionOut = (int)personal["creditsContributionOut"];
                res.originalCreditsContributionIn = (int)personal["originalCreditsContributionIn"];
                res.originalCreditsContributionOut = (int)personal["originalCreditsContributionOut"];

                // CREDIT LISTS
                res.eventCreditsList = personal["eventCreditsList"].ToObject<int[]>();
                res.eventCreditsFactor100List = personal["eventCreditsFactor100List"].ToObject<int[]>();

                ////////// DID

                // SHOTS
                res.shots = (int)personal["shots"];
                res.directHits = (int)personal["directHits"];
                res.explosionHits = (int)personal["explosionHits"];
                res.piercings = (int)personal["piercings"];
                res.damagedWhileMoving = (int)personal["damagedWhileMoving"];
                res.damagedWhileEnemyMoving = (int)personal["damagedWhileEnemyMoving"];
                res.killedAndDamagedByAllSquadmates = (int)personal["killedAndDamagedByAllSquadmates"];
                res.killsBeforeTeamWasDamaged = (int)personal["killsBeforeTeamWasDamaged"];
                res.damaged = (int)personal["damaged"];
                res.kills = (int)personal["kills"];

                // DAMAGE
                res.percentFromTotalTeamDamage = (double)personal["percentFromTotalTeamDamage"];
                res.percentFromSecondBestDamage = (double)personal["percentFromSecondBestDamage"];
                res.damageRating = (int)personal["damageRating"];
                res.damageEventList = (int?)personal["damageEventList"];
                res.damageDealt = (int)personal["damageDealt"];
                res.sniperDamageDealt = (int)personal["sniperDamageDealt"];
                res.movingAvgDamage = (int)personal["movingAvgDamage"];
                res.damageBeforeTeamWasDamaged = (int)personal["damageBeforeTeamWasDamaged"];
                res.avatarDamageEventList = personal["avatarDamageEventList"].ToObject<int?[]>();

                // AT TEAM
                res.tdamageDealt = (int)personal["tdamageDealt"];
                res.tkills = (int)personal["tkills"];
                res.isTeamKiller = (bool)personal["isTeamKiller"];
                res.tdestroyedModules = (int)personal["tdestroyedModules"];
                res.directTeamHits = (int)personal["directTeamHits"];

                // ASSISTED
                res.damageAssistedTrack = (int)personal["damageAssistedTrack"];
                res.damageAssistedRadio = (int)personal["damageAssistedRadio"];
                res.spotted = (int)personal["spotted"];
                res.damageAssistedStun = (int?)personal["damageAssistedStun"];
                res.stunned = (int?)personal["stunned"];
                res.stunDuration = (int?)personal["stunDuration"];
                res.stunNum = (int?)personal["stunNum"];

                // FLAG
                res.flagActions = personal["flagActions"].ToObject<int[]>();
                res.flagCapture = (int)personal["flagCapture"];
                res.capturePoints = (int)personal["capturePoints"];
                res.droppedCapturePoints = (int)personal["droppedCapturePoints"];
                res.capturingBase = (int?)personal["capturingBase"];
                res.soloFlagCapture = (int)personal["soloFlagCapture"];

                ///////// RECEIVED & BLOCKED

                // SHOTS
                res.noDamageDirectHitsReceived = (int)personal["noDamageDirectHitsReceived"];
                res.directHitsReceived = (int)personal["directHitsReceived"];
                res.piercingsReceived = (int)personal["piercingsReceived"];
                res.explosionHitsReceived = (int)personal["explosionHitsReceived"];

                // DAMAGE
                res.damageReceivedFromInvisibles = (int?)personal["damageReceivedFromInvisibles"];
                res.damageBlockedByArmor = (int)personal["damageBlockedByArmor"];
                res.potentialDamageReceived = (int)personal["potentialDamageReceived"];
                res.damageReceived = (int)personal["damageReceived"];

                ////////// OTHERS

                // GOLD
                res.originalGold = (int)personal["originalGold"];
                res.eventGoldFactor100List = personal["eventGoldFactor100List"].ToObject<int[]>();
                res.eventGoldList = personal["eventGoldList"].ToObject<int[]>();
                res.goldReplay = (int?)personal["goldReplay"];
                res.eventGold = (int)personal["eventGold"];
                res.gold = (int)personal["gold"];
                res.subtotalGold = (int)personal["subtotalGold"];

                // MARKS
                res.markOfMastery = (int)personal["markOfMastery"];
                res.marksOnGun = (int)personal["marksOnGun"];
                res.prevMarkOfMastery = (int)personal["prevMarkOfMastery"];

                // DEATH & HP
                res.deathCount = (int)personal["deathCount"];
                res.lifeTime = (int)personal["lifeTime"];
                res.deathReason = (int)personal["deathReason"];
                res.health = (int)personal["health"];
                res.committedSuicide = (bool)personal["committedSuicide"];
                res.killerID = (int)personal["killerID"];

                // CRYSTAL
                res.originalCrystal = (int?)personal["originalCrystal"];
                res.crystal = (int?)personal["crystal"];
                res.crystalReplay = (int?)personal["crystalReplay"];
                res.subtotalCrystal = (int?)personal["subtotalCrystal"];
                res.eventCrystalList = personal["eventCrystalList"]?.ToObject<string[][]>();
                res.eventCrystal = (int?)personal["eventCrystal"];

                // COSTS
                res.autoEquipCost = personal["autoEquipCost"].ToObject<int[]>();
                res.autoLoadCost = personal["autoLoadCost"].ToObject<int[]>();
                res.autoRepairCost = (int)personal["autoRepairCost"];
                res.repair = (int)personal["repair"];

                // OTHERS
                res.vehTypeLockTime = (int)personal["vehTypeLockTime"];
                res.winPoints = (int)personal["winPoints"];
                res.stopRespawn = (bool)personal["stopRespawn"];
                res.aogasFactor10 = (int)personal["aogasFactor10"];
                res.extPublic = personal["extPublic"];
                res.resourceAbsorbed = (int)personal["resourceAbsorbed"];
                res.fairplayFactor10 = (int)personal["fairplayFactor10"];
                res.serviceProviderID = (int)personal["serviceProviderID"];
                res.typeCompDescr = (int)personal["typeCompDescr"];
                res.questsProgress = (object)personal["questsProgress"];
                res.achievements = personal["achievements"].ToObject<int[]>();
                res.dossierPopUps = personal["dossierPopUps"].ToObject<object>();
                res.isPremium = (bool)personal["isPremium"];
                res.mileage = (int)personal["mileage"];
                res.rolloutsCount = (int)personal["rolloutsCount"];
                res.index = (int)personal["index"];
                res.accountDBID = (int)personal["accountDBID"];
                res.battleNum = (int)personal["battleNum"];

                myplayer.results = res;

                // create list for each player where the mainplayer did something
                List<PersonalDetails> personalDetails = new List<PersonalDetails>();

                // for each player in the mainplayers "details"
                foreach (JToken item in personal?["details"])
                {
                    // get the right json part for player
                    JObject myitem = JObject.Parse(item.First.ToString());

                    // create instance of PersonalDetails
                    PersonalDetails details = new PersonalDetails();

                    // get the path of the detailed player and set id
                    var path = (string)item.Path.Remove(0, 10);
                    details.id = (string)path.Remove(path.Length - 6, 6);

                    // RECEIVED & BLOCKED
                    details.damageReceived = (int)myitem["damageReceived"];
                    details.rickochetsReceived = (int)myitem["rickochetsReceived"];
                    details.damageBlockedByArmor = (int)myitem["damageBlockedByArmor"];
                    details.noDamageDirectHitsReceived = (int)myitem["noDamageDirectHitsReceived"];

                    // DID
                    details.crits = (int)myitem["crits"];
                    details.fire = (int)myitem["fire"];
                    details.piercings = (int)myitem["piercings"];
                    details.explosionHits = (int)myitem["explosionHits"];
                    details.targetKills = (int)myitem["targetKills"];
                    details.directHits = (int)myitem["directHits"];
                    details.damageDealt = (int)myitem["damageDealt"];

                    // ASISTED
                    details.spotted = (int)myitem["spotted"];
                    details.damageAssistedTrack = (int)myitem["damageAssistedTrack"];
                    details.damageAssistedStun = (int?)myitem["damageAssistedStun"];
                    details.stunDuration = (double?)myitem["stunDuration"];
                    details.stunNum = (int?)myitem["stunNum"];
                    details.damageAssistedRadio = (int)myitem["damageAssistedRadio"];

                    // DEATH
                    details.deathReason = (int)myitem["deathReason"];

                    // add this player to the list
                    personalDetails.Add(details);
                }

                // set the mainplayer details to this list
                myplayer.details = personalDetails;
            }

            // return the mainplayer
            return myplayer;
        }
    }
}
